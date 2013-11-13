using UnityEngine;

namespace Initialization.ShallowFluidInitialization
{
    public interface IInitialConditions
    {
        Vector2 VelocityAtPoint(Vector3 point);

        float HeightPerturbationAtPoint(Vector3 point);
    }
}