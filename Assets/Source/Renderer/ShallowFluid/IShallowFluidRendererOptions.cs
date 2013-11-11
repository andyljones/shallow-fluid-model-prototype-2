using System;
using System.Collections.Generic;

namespace Renderer
{
    public interface IShallowFluidRendererOptions
    {
        List<String> LayerMaterials { get; }
        String BoundaryMaterial { get; }
        String ArrowMaterial { get; }
        float Radius { get; }
        float DetailMultiplier { get; }
    }
}