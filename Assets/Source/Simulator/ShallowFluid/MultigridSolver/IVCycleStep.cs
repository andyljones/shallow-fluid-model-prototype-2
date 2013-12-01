namespace Simulator.ShallowFluid.MultigridSolver
{
    public interface IVCycleStep<T>
    {
        void Process(ref ScalarField<T> fineField, ScalarField<T> laplacianOfFineField);
    }
}
