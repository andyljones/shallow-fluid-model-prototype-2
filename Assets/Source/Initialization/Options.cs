using System;
using System.Collections.Generic;
using Atmosphere.MonolayerAtmosphere;
using Grids.IcosahedralGridGenerator;
using Renderer;
using Simulator.ShallowFluidSimulator;
using Surfaces.FlatSurface;

namespace Initialization
{
    public class Options : 
        IIcosahedralGridOptions, 
        IFlatSurfaceOptions, 
        IMonolayerAtmosphereOptions, 
        IShallowFluidRendererOptions, 
        IShallowFluidSimulatorOptions
    {
        public float Radius { get; set; }
        public float Resolution { get; set; }
        public float Height { get; set; }
        public float DayLength { get; set; }
        public float Timestep { get; set; }
        public List<String> LayerMaterials { get; set; }
        public String BoundaryMaterial { get; set; }
        public String ArrowMaterial { get; set; }
        public float DetailMultiplier { get; set; }
    }
}
