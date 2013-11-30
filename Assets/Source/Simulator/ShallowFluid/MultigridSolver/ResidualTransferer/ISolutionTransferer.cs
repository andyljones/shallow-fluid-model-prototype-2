namespace Simulator.ShallowFluid.MultigridSolver.ResidualTransferer
{
    public interface ISolutionTransferer<T>
    {
        void Transfer(ScalarField<T> fineField, ref ScalarField<T> coarseField);
    }
}
