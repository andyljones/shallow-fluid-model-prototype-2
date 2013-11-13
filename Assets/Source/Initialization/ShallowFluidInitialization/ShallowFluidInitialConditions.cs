using UnityEngine;

namespace Initialization.ShallowFluidInitialization
{
    public class ShallowFluidInitialConditions : IInitialConditions
    {
        public Vector2 VelocityAtPoint(Vector3 point)
        {
            return new Vector2(.01f, 0);
        }

        public float HeightPerturbationAtPoint(Vector3 point)
        {
            return 0f;
        }
    }
}