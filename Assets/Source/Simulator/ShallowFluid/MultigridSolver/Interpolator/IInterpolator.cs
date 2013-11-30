namespace Simulator.ShallowFluid.MultigridSolver.Interpolator
{
    public interface IInterpolatorFactory<T>
    {
        IInterpolator<T> GetInterpolator(IGeometry<T> geometry);
    }

    public interface IInterpolator<T>
    {
        void Interpolate(ScalarField<T> sourceField, ref ScalarField<T> targetField);
    }
}
