using UnityEngine;

namespace Renderer
{
    public interface IRenderer
    {
        Vector3[] Vectors { get; }
        int[] AtmosphereTriangles { get; }
        int[] SurfaceTriangles { get; }
    }
}