namespace Simulator.ShallowFluid.MultigridSolver.Interpolator
{
    public interface IInterpolator<T>
    {
        ScalarField<T> Interpolate(ScalarField<T> field);
    }
}
