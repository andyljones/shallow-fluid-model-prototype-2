using System.Collections.Generic;

namespace Simulator.ShallowFluid
{
    public interface IGeometry<T>
    {
        ScalarField<T> Areas { get; }
        Dictionary<T, ScalarField<T>> Widths { get; }
        Dictionary<T, ScalarField<T>> InternodeDistances { get; }
    }
}
