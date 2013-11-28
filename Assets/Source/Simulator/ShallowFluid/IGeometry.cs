namespace Simulator.ShallowFluid
{
    public interface IGeometry<T>
    {
        ScalarField<T> Areas { get; }
        ScalarField<Pair<T>> Widths { get; }
        ScalarField<Pair<T>> InternodeDistances { get; }
    }
}
