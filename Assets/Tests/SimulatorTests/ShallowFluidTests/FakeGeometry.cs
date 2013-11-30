using Simulator.ShallowFluid;

namespace Tests.SimulatorTests.ShallowFluidTests
{
    public class FakeGeometry : IGeometry<int>
    {
        public VectorField<int> Positions { get; set; } 
        public ScalarField<int> Areas { get; set; }
        public ScalarFieldMap<int> Widths { get; set; }
        public ScalarFieldMap<int> InternodeDistances { get; set; }
    }
}
