namespace Simulator.ShallowFluid.MultigridSolver
{
    public class SolutionStep<T> : IVCycleStep<T>
    {
        public void Process(ref ScalarField<T> fineField, ScalarField<T> laplacianOfFineField)
        {
            throw new System.NotImplementedException();
        }
    }
}
