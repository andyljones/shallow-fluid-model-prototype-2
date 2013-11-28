using System.Collections.Generic;
using Simulator.ShallowFluid;

namespace Tests.SimulatorTests.ShallowFluidTests
{
    public class RelativePositions<T>
    {
        Dictionary<T, VectorField<T>> Positions { get; private set; }

        public RelativePositions(Graph<T> graph, VectorField<T> positions)
        {
            
        }
    }
}
