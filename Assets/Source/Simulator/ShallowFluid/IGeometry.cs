using System.Collections.Generic;

namespace Simulator.ShallowFluid
{
    public interface IGeometry<T>
    {
        VectorField<T> Positions { get; }
        ScalarField<T> Areas { get; }
        ScalarFieldMap<T> Widths { get; }
        ScalarFieldMap<T> InternodeDistances { get; }
    }
}
