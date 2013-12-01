using System.Collections.Generic;
using Atmosphere;
using Foam;
using Simulator.ShallowFluid.MultigridSolver;

namespace Simulator.ShallowFluid
{
    class ShallowFluidSimulator : ISimulator
    {
        public List<Cell> Cells { get; private set; }

        private Solver _solver;
        private bool _initialized = false;

        public ShallowFluidSimulator(IAtmosphere atmosphere)
        {
            Cells = atmosphere.Cells;
            var graph = AdjacencyGraphOf(Cells);
            _solver = new Solver(graph);
        }

        private Graph<Cell> AdjacencyGraphOf(List<Cell> cells)
        {
            var graph = new Graph<Cell>();

            foreach (var cell in cells)
            {
                graph.Add(cell, cell.Neighbours());
            }

            return graph;
        }

        public void StepSimulation()
        {
            
        }

        public void UpdateCellConditions()
        {
            
        }
    }
}
