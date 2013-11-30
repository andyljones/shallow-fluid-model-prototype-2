namespace Simulator.ShallowFluid.MultigridSolver.Relaxer
{
    public interface IRelaxerFactory<T>
    {
        IRelaxationCalculator<T> GetRelaxationCalculator(IGeometry<T> geometry);
    }

    public interface IRelaxationCalculator<T>
    {
        void Relax(ref ScalarField<T> field, ScalarField<T> laplacianOfField);
    }
}
