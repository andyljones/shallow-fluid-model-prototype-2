using System;
using System.Collections.Generic;

namespace Renderer.ShallowFluid
{
    public interface IShallowFluidRendererOptions
    {
        List<String> LayerMaterials { get; }
        String BoundaryMaterial { get; }
        String ArrowMaterial { get; }
        float Radius { get; }
        float Resolution { get; }
        float DetailMultiplier { get; }
        float ArrowLengthMultiplier { get; }

    }
}