using Simulator.ShallowFluid;

namespace Tests.AtmosphereTests.MonolayerAtmosphereTests
{
    public class FakeGeometry : IGeometry<int>
    {
        public ScalarField<int> Areas { get; set; }
        public ScalarField<Pair<int>> Widths { get; set; }
        public ScalarField<Pair<int>> InternodeDistances { get; set; }
    }
}
