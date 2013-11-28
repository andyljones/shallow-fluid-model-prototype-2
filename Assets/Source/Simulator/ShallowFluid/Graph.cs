using System.Collections.Generic;

namespace Simulator.ShallowFluid
{
    /// <summary>
    /// Syntactic sugar for a dictionary mapping objects of type T to lists of objects of type T 
    /// (interpreted as their neighbours).
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public class Graph<TNode> : Dictionary<TNode, List<TNode>>
    {

    }
}
