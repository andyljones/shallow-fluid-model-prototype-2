using Grids.IcosahedralGrid;
using Surfaces.FlatSurface;

namespace Initialization
{
    public class Options : IIcosahedralGridOptions, IFlatSurfaceOptions
    {
        public float Radius { get; set; }
        public float Resolution { get; set; }
    }
}
