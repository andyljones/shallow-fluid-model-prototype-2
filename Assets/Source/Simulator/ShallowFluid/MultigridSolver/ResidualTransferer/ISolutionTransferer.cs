namespace Simulator.ShallowFluid.MultigridSolver.ResidualTransferer
{
    public interface ISolutionTransfererFactory<T>
    {
        ISolutionTransferer<T> GetSolutionTransferer(IGeometry<T> geometry);
    }
    public interface ISolutionTransferer<T>
    {
        void Transfer(ScalarField<T> fineField, ref ScalarField<T> coarseField);
    }
}
