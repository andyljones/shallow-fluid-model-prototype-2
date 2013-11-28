using System.Collections.Generic;
using Simulator.ShallowFluid;

namespace Tests.AtmosphereTests.MonolayerAtmosphereTests
{
    public class FakeGeometry : IGeometry<int>
    {
        public VectorField<int> Positions { get; set; } 
        public ScalarField<int> Areas { get; set; }
        public Dictionary<int, ScalarField<int>> Widths { get; set; }
        public Dictionary<int, ScalarField<int>> InternodeDistances { get; set; }
    }
}
