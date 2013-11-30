using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Simulator.ShallowFluid;
using Simulator.ShallowFluid.MultigridSolver;

namespace Tests.SimulatorTests.ShallowFluidTests.MultigridSolverTests
{
    [TestFixture]
    public class GreedyGraphCoarsenerTests
    {
        [Test]
        public void Constructor_GivenSingleNodeGraph_ShouldGenerateOneCoarsenedGraph()
        {
            var singleNode = new Graph<int> { { 0, new List<int>() } };
            var graphCoarsener = new GreedyGraphCoarsener<int>(singleNode); 

            var numberOfCoarsenedGraphs = graphCoarsener.CoarsenedGraphs;

            Assert.That(numberOfCoarsenedGraphs.Count, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_GivenSingleNodeGraph_ShouldSetLeastCoarsenedGraphToIt()
        {
            var singleNode = new Graph<int> { { 0, new List<int>() } };
            var graphCoarsener = new GreedyGraphCoarsener<int>(singleNode);

            var leastCoarsenedGraph = graphCoarsener.CoarsenedGraphs.First();

            Assert.That(leastCoarsenedGraph, Is.EquivalentTo(singleNode));
        }

        [Test]
        public void Constructor_GivenTwoAdjacentNodes_ShouldGenerateTwoCoarsenedGraphs()
        {
            var adjacentPair = new Graph<int> { { 0, new List<int> { 1 } }, { 1, new List<int> { 0 } } };
            var graphCoarsener = new GreedyGraphCoarsener<int>(adjacentPair);

            var numberOfCoarsenedGraphs = graphCoarsener.CoarsenedGraphs;

            Assert.That(numberOfCoarsenedGraphs.Count, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_GivenTwoAdjacentNodes_ShouldGenerateMostCoarseGraphWithOneElement()
        {
            var adjacentPair = new Graph<int> { { 0, new List<int> { 1 } }, { 1, new List<int> { 0 } } };
            var graphCoarsener = new GreedyGraphCoarsener<int>(adjacentPair);

            var mostCoarsenedGraph = graphCoarsener.CoarsenedGraphs.Last();

            Assert.That(mostCoarsenedGraph.Count, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_GivenTwoAdjacentNodes_ShouldGenerateMostCoarseGraphWhoseFirstNodeHasNoNeighbours()
        {
            var adjacentPair = new Graph<int> { { 0, new List<int> { 1 } }, { 1, new List<int> { 0 } } };
            var graphCoarsener = new GreedyGraphCoarsener<int>(adjacentPair);

            var mostCoarsenedGraph = graphCoarsener.CoarsenedGraphs.Last();
            var firstNodesNeighbours = mostCoarsenedGraph.First().Value;

            Assert.That(firstNodesNeighbours.Count, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_GivenFourNodesInALine_ShouldGenerateThreeCoarsenedGraphs()
        {
            var fourNodesInALine = new Graph<int> 
            {
                {0, new List<int> {1}},
                {1, new List<int> {0, 2}},
                {2, new List<int> {1, 3}},
                {3, new List<int> {2}}
            };

            var graphCoarsener = new GreedyGraphCoarsener<int>(fourNodesInALine);

            var numberOfCoarsenedGraphs = graphCoarsener.CoarsenedGraphs;

            Assert.That(numberOfCoarsenedGraphs.Count, Is.EqualTo(3));
        }

        [Test]
        public void Constructor_GivenFourNodesInALine_ShouldGenerateSecondLeastCoarsenedGraphWithTwoNodes()
        {
            var fourNodesInALine = new Graph<int> 
            {
                {0, new List<int> {1}},
                {1, new List<int> {0, 2}},
                {2, new List<int> {1, 3}},
                {3, new List<int> {2}}
            };

            var graphCoarsener = new GreedyGraphCoarsener<int>(fourNodesInALine);

            var secondLeastCoarsenedGraph = graphCoarsener.CoarsenedGraphs[1];

            Assert.That(secondLeastCoarsenedGraph.Count, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_GivenFourNodesInALine_ShouldGenerateSecondLeastCoarsenedGraphWhereEachNodeHasOneNeighbour()
        {
            var fourNodesInALine = new Graph<int> 
            {
                {0, new List<int> {1}},
                {1, new List<int> {0, 2}},
                {2, new List<int> {1, 3}},
                {3, new List<int> {2}}
            };

            var graphCoarsener = new GreedyGraphCoarsener<int>(fourNodesInALine);

            var secondLeastCoarsenedGraph = graphCoarsener.CoarsenedGraphs[1];
            var neighboursPerNode = secondLeastCoarsenedGraph.Values.Select(neighboursList => neighboursList.Count);

            Assert.That(neighboursPerNode, Has.All.EqualTo(1));
        }

        [Test]
        public void Constructor_GivenCompleteGraphOnThreeNodes_ShouldGenerateTwoCoarsenedGraphs()
        {
            var completeGraphOnThreeNodes = new Graph<int> 
            {
                {0, new List<int> {1, 2}},
                {1, new List<int> {0, 2}},
                {2, new List<int> {0, 1}}
            };

            var graphCoarsener = new GreedyGraphCoarsener<int>(completeGraphOnThreeNodes);

            var numberOfCoarsenedGraphs = graphCoarsener.CoarsenedGraphs;

            Assert.That(numberOfCoarsenedGraphs.Count, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_GivenSingleNodeGraph_ShouldGenerateNoCoarseNeighbourGraphs()
        {
            var singleNode = new Graph<int> { { 0, new List<int>() } };
            var graphCoarsener = new GreedyGraphCoarsener<int>(singleNode);

            var numberOfCoarseNeighbourGraphs = graphCoarsener.CoarseNeighbourGraphs;

            Assert.That(numberOfCoarseNeighbourGraphs.Count, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_GivenTwoAdjacentNodes_ShouldGenerateOneCoarseNeighbourGraph()
        {
            var adjacentPair = new Graph<int> { { 0, new List<int> { 1 } }, { 1, new List<int> { 0 } } };
            var graphCoarsener = new GreedyGraphCoarsener<int>(adjacentPair);

            var numberOfCoarseNeighbourGraphs = graphCoarsener.CoarseNeighbourGraphs;

            Assert.That(numberOfCoarseNeighbourGraphs.Count, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_GivenTwoAdjacentNodes_ShouldGenerateCoarseNeighbourGraphWithTwoNodes()
        {
            var adjacentPair = new Graph<int> { { 0, new List<int> { 1 } }, { 1, new List<int> { 0 } } };
            var graphCoarsener = new GreedyGraphCoarsener<int>(adjacentPair);

            var firstCoarseNeighbourGraph = graphCoarsener.CoarseNeighbourGraphs.First();

            Assert.That(firstCoarseNeighbourGraph.Count, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_GivenTwoAdjacentNodes_ShouldGenerateCoarseNeighbourGraphWhereEachNodeHasOneNeighbour()
        {
            var adjacentPair = new Graph<int> { { 0, new List<int> { 1 } }, { 1, new List<int> { 0 } } };
            var graphCoarsener = new GreedyGraphCoarsener<int>(adjacentPair);

            var firstCoarseNeighbourGraph = graphCoarsener.CoarseNeighbourGraphs.First();
            var neighboursPerNode = firstCoarseNeighbourGraph.Values.Select(neighbourList => neighbourList.Count);

            Assert.That(neighboursPerNode, Has.All.EqualTo(1));
        }

        [Test]
        public void Constructor_GivenCompleteGraphOnThreeNodes_ShouldGenerateCoarseNeighbourGraphWhereEachNodeHasOneNeighbour()
        {
            var completeGraphOnThreeNodes = new Graph<int> 
            {
                {0, new List<int> {1, 2}},
                {1, new List<int> {0, 2}},
                {2, new List<int> {0, 1}}
            };
            var graphCoarsener = new GreedyGraphCoarsener<int>(completeGraphOnThreeNodes);

            var firstCoarseNeighbourGraph = graphCoarsener.CoarseNeighbourGraphs.First();
            var neighboursPerNode = firstCoarseNeighbourGraph.Values.Select(neighbourList => neighbourList.Count);

            Assert.That(neighboursPerNode, Has.All.EqualTo(1));
        }
    }
}
