namespace Simulator.ShallowFluid.MultigridSolver.ResidualTransferer
{
    public interface IResidualTransferer<T>
    {
        IGeometry<T> Geometry { set; } 
        void Transfer(ScalarField<T> fineField, ref ScalarField<T> coarseField);
    }
}
