namespace Simulator.ShallowFluid.MultigridSolver
{
    public interface IVCycleLevel<T>
    {
        void Process(ref ScalarField<T> fineField, ScalarField<T> laplacianOfFineField);
    }
}
