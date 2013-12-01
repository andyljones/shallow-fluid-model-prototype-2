using System.Collections.Generic;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public interface IGraphCoarsener<T>
    {
        List<Graph<T>> CoarsenedGraphs { get; }
        Dictionary<Graph<T>, Graph<T>> CoarseNeighbourGraphs { get; } 
    }
}
