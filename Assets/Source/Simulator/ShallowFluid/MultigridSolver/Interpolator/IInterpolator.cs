namespace Simulator.ShallowFluid.MultigridSolver.Interpolator
{
    public interface IInterpolator<T>
    {
        void Interpolate(ScalarField<T> sourceField, ref ScalarField<T> targetField);
    }
}
