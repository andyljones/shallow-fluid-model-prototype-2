using System.Collections.Generic;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public interface IGraphCoarsener<T>
    {
        List<Dictionary<T, List<T>>> CoarsenedGraphs { get; }
    }
}
