using System.Collections.Generic;

namespace Simulator.ShallowFluid
{
    /// <summary>
    /// Syntactic sugar for a dictionary of scalar fields.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScalarFieldMap<T> : Dictionary<T, ScalarField<T>> 
    {
    }
}
