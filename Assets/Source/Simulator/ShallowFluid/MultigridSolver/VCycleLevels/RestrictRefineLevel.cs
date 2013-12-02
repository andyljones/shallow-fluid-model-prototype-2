using System.Diagnostics;
using System.Linq;
using Foam;
using Simulator.ShallowFluid.MultigridSolver.Interpolator;
using Simulator.ShallowFluid.MultigridSolver.Relaxer;
using Simulator.ShallowFluid.MultigridSolver.ResidualTransferer;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public class RestrictRefineLevel: IVCycleLevel<Cell>
    {
        public int NumberOfRelaxationsDuringRestriction = 2;
        public int NumberOfRelaxationsDuringInterpolation = 1;

        private IVCycleLevel<Cell> _nextLevel;
        private IGeometry<Cell> _fineGeometry;
        private IGeometry<Cell> _coarseGeometry;
        private Graph<Cell> _interpolationGraph;

        private readonly IRelaxer<Cell> _relaxer;
        private readonly SolutionTransferer<Cell> _transferer;
        private readonly WeightedAverageInterpolator<Cell> _interpolator;
        private LinearOperators _operators;

        public RestrictRefineLevel(IVCycleLevel<Cell> nextLevel, IGeometry<Cell> fineGeometry, Graph<Cell> interpolationGraph)
        {
            _nextLevel = nextLevel;
            _fineGeometry = fineGeometry;
            _interpolationGraph = interpolationGraph;

            _relaxer = new Relaxer<Cell>(fineGeometry);
            _transferer = new SolutionTransferer<Cell>(interpolationGraph);
            _interpolator = new WeightedAverageInterpolator<Cell>(interpolationGraph, fineGeometry);
            _operators = new LinearOperators(fineGeometry);
        }

        //TODO: Holy shit test this
        public void Process(ref ScalarField<Cell> fineField, ScalarField<Cell> laplacianOfFineField)
        {
            for (int i = 0; i < NumberOfRelaxationsDuringRestriction; i++)
            {
                _relaxer.Relax(ref fineField, laplacianOfFineField);
            }

            var coarseField = new ScalarField<Cell>(_interpolationGraph.Values.SelectMany(coarseNodes => coarseNodes));

            var residualOnFineGeometry = laplacianOfFineField - _operators.Laplacian(fineField);
            var residualOnCoarseGeometry = new ScalarField<Cell>(_interpolationGraph.Values.SelectMany(coarseNodes => coarseNodes));
            _transferer.Transfer(residualOnFineGeometry, ref residualOnCoarseGeometry);

            _nextLevel.Process(ref coarseField, residualOnCoarseGeometry);

            var errorInFineField = new ScalarField<Cell>(_interpolationGraph.Keys);
            _interpolator.Interpolate(coarseField, ref errorInFineField);

            fineField = fineField + errorInFineField;

            for (int i = 0; i < NumberOfRelaxationsDuringInterpolation; i++)
            {
                _relaxer.Relax(ref fineField, laplacianOfFineField);
            }
        }
    }
}
