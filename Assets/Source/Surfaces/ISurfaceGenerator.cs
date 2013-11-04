using System.Collections.Generic;
using ClimateSim.Grids;

namespace ClimateSim.Surfaces
{
    public interface ISurfaceGenerator
    {
        List<Face> Faces { get; }
    }

}