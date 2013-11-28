namespace Simulator.ShallowFluid
{
    public interface IInterpolator<T>
    {
        ScalarField<T> Interpolate(ScalarField<T> field);
    }
}
