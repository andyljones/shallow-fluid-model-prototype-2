using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClimateSim.Assets.Source.Grids;
using ClimateSim.Grids;
using ClimateSim.Surfaces;
using Ninject;

namespace ClimateSim.Initialization
{
    public class SimulationBuilder
    {
        public List<Face> Surface;

        [Inject]
        public SimulationBuilder(IGrid gridGen, Options options)
        {
            
        }
    }
}
