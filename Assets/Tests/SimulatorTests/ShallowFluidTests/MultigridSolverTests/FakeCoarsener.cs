using System.Collections.Generic;
using Simulator.ShallowFluid.MultigridSolver;

namespace Tests.SimulatorTests.ShallowFluidTests.MultigridSolverTests
{
    public class FakeCoarsener : IGraphCoarsener<int>
    {
        public List<Dictionary<int, List<int>>> CoarsenedGraphs { get; set; }

        public FakeCoarsener(Dictionary<int, List<int>> graph)
        {
            CoarsenedGraphs = new List<Dictionary<int, List<int>>> { graph };
        }
    }
}
