using System.Collections.Generic;
using Foam;

namespace Atmosphere
{
    public interface IAtmosphere
    {
        List<Cell> Cells { get; }
    }
}