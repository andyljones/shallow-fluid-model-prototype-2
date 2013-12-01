using Simulator.ShallowFluid.MultigridSolver.Relaxer;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public class SolutionLevel<T> : IVCycleLevel<T>
    {
        public int NumberOfRelaxationsDuringRestriction = 100;
        
        private readonly IRelaxer<T> _relaxer; 

        public SolutionLevel(IGeometry<T> geometry)
        {
            _relaxer = new Relaxer<T>(geometry);
        }

        //TODO: Test.
        public void Process(ref ScalarField<T> fineField, ScalarField<T> laplacianOfFineField)
        {
            for (int i = 0; i < NumberOfRelaxationsDuringRestriction; i++)
            {
                _relaxer.Relax(ref fineField, laplacianOfFineField);
            }
        }
    }
}
