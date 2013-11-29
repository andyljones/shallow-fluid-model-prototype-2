using System.Collections.Generic;
using Simulator.ShallowFluid;

namespace Tests.AtmosphereTests.MonolayerAtmosphereTests
{
    public class FakeGeometry : IGeometry<int>
    {
        public VectorField<int> Positions { get; set; } 
        public ScalarField<int> Areas { get; set; }
        public ScalarFieldMap<int> Widths { get; set; }
        public ScalarFieldMap<int> InternodeDistances { get; set; }
    }
}
