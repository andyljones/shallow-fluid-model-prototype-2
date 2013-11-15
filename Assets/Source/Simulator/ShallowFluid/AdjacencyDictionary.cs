using System.Collections.Generic;

namespace Simulator.ShallowFluid
{
    /// <summary>
    /// AdjacencyDictionary is synactic sugar for a dictionary that maps objects of type T to a list of their neighbours.
    /// </summary>
    /// <typeparam name="T">The type of the graph's nodes</typeparam>
    public class AdjacencyDictionary<T> : Dictionary<T, List<T>>
    {
    }
}
