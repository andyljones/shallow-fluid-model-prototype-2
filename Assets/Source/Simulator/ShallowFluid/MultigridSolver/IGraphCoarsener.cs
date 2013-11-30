using System.Collections.Generic;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public interface IGraphCoarsener<T>
    {
        IGeometry<T> Geometry { set; }
        List<Graph<T>> CoarsenedGraphs { get; }
    }
}
