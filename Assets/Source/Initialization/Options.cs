using Atmosphere.MonolayerAtmosphere;
using Grids.IcosahedralGrid;
using Surfaces.FlatSurface;

namespace Initialization
{
    public class Options : IIcosahedralGridOptions, IFlatSurfaceOptions, IMonolayerAtmosphereOptions
    {
        public float Radius { get; set; }
        public float Resolution { get; set; }
        public float Height { get; set; }
    }
}
