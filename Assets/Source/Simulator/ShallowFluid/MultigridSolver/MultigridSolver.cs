using System;
using System.Collections.Generic;
using System.Linq;
using Foam;
using Simulator.ShallowFluid.MultigridSolver.Interpolator;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public class MultigridSolver
    {
        public int SmallestGraphSizeToConsider = 20;

        private readonly IVCycleLevel<Cell> _headOfVCycleLevelChain;

        public MultigridSolver(Graph<Cell> finestGraph)
        {
            _headOfVCycleLevelChain = GenerateVCycleLevelChain(finestGraph);
        }

        private IVCycleLevel<Cell> GenerateVCycleLevelChain(Graph<Cell> finestGraph)
        {
            var coarsener = new GreedyGraphCoarsener<Cell>(finestGraph);
            var geometries = CreateCoarsenedGeometries(coarsener);
            var interpolationGraphs = CreateInterpolationGraphs(geometries, coarsener);

            var currentGeometry = geometries.Last();
            geometries.Remove(currentGeometry);
            IVCycleLevel<Cell> currentLevel = new SolutionLevel<Cell>(currentGeometry);

            while (geometries.Count > 0)
            {
                currentGeometry = geometries.Last();
                geometries.Remove(currentGeometry);
                var currentInterpolationGraph = interpolationGraphs[currentGeometry];
                currentLevel = new RestrictRefineLevel<Cell>(currentLevel, currentGeometry, currentInterpolationGraph);
            }

            return currentLevel;
        }

        private List<IGeometry<Cell>> CreateCoarsenedGeometries(IGraphCoarsener<Cell> coarsener)
        {
            var graphs = FetchCoarsenedGraphs(coarsener);
            var coarsenedGeometries = new List<IGeometry<Cell>>();

            foreach (var graph in graphs)
            {
                var geometryOfGraph = new FoamGeometry(graph);
                coarsenedGeometries.Add(geometryOfGraph);
            }

            return coarsenedGeometries;
        }

        private List<Graph<Cell>> FetchCoarsenedGraphs(IGraphCoarsener<Cell> coarsener)
        {
            var coarsenedGraphs = coarsener.CoarsenedGraphs;
            var graphsOfSufficientSize = coarsenedGraphs.TakeWhile(graph => graph.Count > SmallestGraphSizeToConsider);
            var graphsToConsider = graphsOfSufficientSize.DefaultIfEmpty(coarsenedGraphs.First());

            return graphsToConsider.ToList();
        }

        private Dictionary<IGeometry<Cell>, Graph<Cell>> CreateInterpolationGraphs(List<IGeometry<Cell>> geometries, 
                                                                                   IGraphCoarsener<Cell> coarsener)
        {
            return geometries.ToDictionary(geometry => geometry,
                                           geometry => coarsener.CoarseNeighbourGraphs[geometry.Graph]);
        }

        public void Solve(ref ScalarField<Cell> field, ScalarField<Cell> laplacianOfField)
        {
            _headOfVCycleLevelChain.Process(ref field, laplacianOfField);
        }
    }
}
