using Renderer.ShallowFluid;
using UnityEngine;

namespace Renderer.Heightmap
{
    public class MonolayerHeightmap : IHeightmap
    {
        private readonly float _radius;
        private readonly float _scaleFactor;

        public MonolayerHeightmap(IShallowFluidRendererOptions options)
        {
            _radius = options.Radius;
            _scaleFactor = options.DetailMultiplier;
        }

        public Vector3 VisualPositionFromActualPosition(Vector3 position)
        {
            return (_radius + (position.magnitude - _radius)*_scaleFactor) * position.normalized;
        }
    }
}
