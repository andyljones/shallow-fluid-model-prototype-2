using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClimateSim.Grids
{
    public interface IGrid
    {
        List<Face> Faces { get; }
    }
}
