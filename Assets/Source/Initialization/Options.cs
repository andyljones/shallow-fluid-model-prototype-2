using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClimateSim.Grids.IcosahedralGrid;
using ClimateSim.Surfaces.FlatSurface;

namespace ClimateSim.Assets.Source.Grids
{
    public class Options : IIcosahedralGridOptions, IFlatSurfaceOptions
    {
        public float Radius { get; set; }
        public float Resolution { get; set; }
    }
}
