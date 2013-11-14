using UnityEngine;

namespace Renderer.Heightmap
{
    public interface IHeightmap
    {
        Vector3 VisualPositionFromActualPosition(Vector3 position);
    }
}
