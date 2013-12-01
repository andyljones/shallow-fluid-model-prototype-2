using System.Collections.Generic;

namespace Simulator.ShallowFluid
{
    /// <summary>
    /// Syntactic sugar for a dictionary of vector fields.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VectorFieldMap<T> : Dictionary<T, VectorField<T>>
    {
    }
}
