using System;
using System.Collections.Generic;
using Atmosphere.MonolayerAtmosphere;
using Grids.IcosahedralGridGenerator;
using Renderer.ShallowFluid;
using Surfaces.FlatSurface;

namespace Initialization
{
    public class Options : 
        IIcosahedralGridOptions, 
        IFlatSurfaceOptions, 
        IMonolayerAtmosphereOptions, 
        IShallowFluidRendererOptions
    {
        public float Radius { get; set; }
        public float Resolution { get; set; }
        public float Height { get; set; }
        public float DayLength { get; set; }
        public float Timestep { get; set; }
        public List<String> LayerMaterials { get; set; }
        public String BoundaryMaterial { get; set; }
        public String ArrowMaterial { get; set; }
        public float ArrowLengthMultiplier { get; set; }
        public float DetailMultiplier { get; set; }
    }
}
