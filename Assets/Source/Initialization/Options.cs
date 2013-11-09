using Atmosphere.MonolayerAtmosphere;
using Grids.IcosahedralGridGenerator;
using Renderer;
using Surfaces.FlatSurface;

namespace Initialization
{
    public class Options : IIcosahedralGridOptions, IFlatSurfaceOptions, IMonolayerAtmosphereOptions, IShallowFluidRendererOptions
    {
        public float Radius { get; set; }
        public float Resolution { get; set; }
        public float Height { get; set; }
    }
}
