using Renderer.Heightmap;
using UnityEngine;

namespace Tests.Fakes
{
    public class FakeHeightmap : IHeightmap
    {
        public Vector3 VisualPositionFromActualPosition(Vector3 position)
        {
            return position;
        }
    }
}
