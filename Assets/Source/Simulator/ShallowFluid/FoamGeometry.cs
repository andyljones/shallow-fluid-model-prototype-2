using Foam;

namespace Simulator.ShallowFluid
{
    public class FoamGeometry : IGeometry<Cell>
    {
        public ScalarField<Cell> Areas { get; set; }
        public ScalarField<Pair<Cell>> Widths { get; set; }
        public ScalarField<Pair<Cell>> InternodeDistances { get; set; }
    }
}
