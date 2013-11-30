namespace Simulator.ShallowFluid.MultigridSolver.ResidualTransferer
{
    public interface IResidualTransferer<T>
    {
        void Transfer(ScalarField<T> fineField, ref ScalarField<T> coarseField);
    }
}
