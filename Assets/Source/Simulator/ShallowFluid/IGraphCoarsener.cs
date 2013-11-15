using System.Collections.Generic;

namespace Simulator.ShallowFluid
{
    public interface IGraphCoarsener<T>
    {
        List<Dictionary<T, List<T>>> CoarsenedGraphs { get; }
    }
}
