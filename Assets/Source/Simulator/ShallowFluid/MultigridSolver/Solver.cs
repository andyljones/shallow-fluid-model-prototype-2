using System.Collections.Generic;
using System.Linq;
using Foam;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public class Solver
    {
        public int SmallestGraphSizeToConsider = 20;

        private readonly IVCycleLevel<Cell> _headOfVCycleLevelChain;

        //TODO: testttt
        public Solver(Graph<Cell> finestGraph)
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
                currentLevel = new RestrictRefineLevel(currentLevel, currentGeometry, currentInterpolationGraph);
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

        //TODO: Test test test test
        public void Solve(ref ScalarField<Cell> field, ScalarField<Cell> laplacianOfField)
        {
            _headOfVCycleLevelChain.Process(ref field, laplacianOfField);
        }
    }
}
