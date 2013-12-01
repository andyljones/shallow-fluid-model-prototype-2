using System.Collections.Generic;

namespace Simulator.ShallowFluid
{
    public interface IGeometry<T>
    {
        Graph<T> Graph { get; }
        VectorField<T> Positions { get; }
        ScalarField<T> Areas { get; }
        ScalarFieldMap<T> Widths { get; }
        ScalarFieldMap<T> InternodeDistances { get; }
        VectorFieldMap<T> RelativePositions { get; } 
    }
}
