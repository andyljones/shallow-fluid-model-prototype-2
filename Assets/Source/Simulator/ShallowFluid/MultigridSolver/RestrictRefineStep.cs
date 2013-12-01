using System.Diagnostics;
using System.Linq;
using Simulator.ShallowFluid.MultigridSolver.Interpolator;
using Simulator.ShallowFluid.MultigridSolver.Relaxer;
using Simulator.ShallowFluid.MultigridSolver.ResidualTransferer;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public class RestrictRefineStep<T> : IVCycleStep<T>
    {
        public int NumberOfRelaxationsDuringRestriction = 2;
        public int NumberOfRelaxationsDuringInterpolation = 1;

        private IVCycleStep<T> _nextStep;
        private IGeometry<T> _fineGeometry;
        private IGeometry<T> _coarseGeometry;
        private Graph<T> _interpolationGraph;

        private readonly IRelaxer<T> _relaxer;
        private readonly SolutionTransferer<T> _transferer;
        private readonly WeightedAverageInterpolator<T> _interpolator;

        public RestrictRefineStep(IVCycleStep<T> nextStep, IGeometry<T> fineGeometry, Graph<T> interpolationGraph)
        {
            _nextStep = nextStep;
            _fineGeometry = fineGeometry;
            _interpolationGraph = interpolationGraph;

            _relaxer = new Relaxer<T>(fineGeometry);
            _transferer = new SolutionTransferer<T>(interpolationGraph);
            _interpolator = new WeightedAverageInterpolator<T>(interpolationGraph, fineGeometry);
        }

        //TODO: Holy shit test this
        public void Process(ref ScalarField<T> fineField, ScalarField<T> laplacianOfFineField)
        {
            for (int i = 0; i < NumberOfRelaxationsDuringRestriction; i++)
            {
                _relaxer.Relax(ref fineField, laplacianOfFineField);
            }

            var coarseField = new ScalarField<T>(_interpolationGraph.Values.SelectMany(coarseNodes => coarseNodes));

            var residualOnFineGeometry = laplacianOfFineField.Subtract(fineField.LaplacianWithRespectTo(_fineGeometry));
            var residualOnCoarseGeometry = new ScalarField<T>(_interpolationGraph.Values.SelectMany(coarseNodes => coarseNodes));
            _transferer.Transfer(residualOnFineGeometry, ref residualOnCoarseGeometry);

            _nextStep.Process(ref coarseField, residualOnCoarseGeometry);

            var errorInFineField = new ScalarField<T>(_interpolationGraph.Keys);
            _interpolator.Interpolate(coarseField, ref errorInFineField);

            fineField = fineField.Add(errorInFineField);

            for (int i = 0; i < NumberOfRelaxationsDuringInterpolation; i++)
            {
                _relaxer.Relax(ref fineField, laplacianOfFineField);
            }
        }
    }
}
