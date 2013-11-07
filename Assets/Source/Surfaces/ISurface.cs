using System.Collections.Generic;
using Foam;

namespace Surfaces
{
    public interface ISurface
    {
        List<Face> Faces { get; }
    }

}