using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Simulator.ShallowFluid;

namespace Tests.SimulatorTests.ShallowFluidTests
{
    [TestFixture]
    public class GraphCoarsenerTests
    {
        [Test]
        public void Constructor_GivenSingleNodeGraph_ShouldGenerateOneCoarsenedGraph()
        {
            var singleNode = new Dictionary<int, List<int>> { { 0, new List<int>() } };
            var graphCoarsener = new GraphCoarsener<int>(singleNode);

            var numberOfCoarsenedGraphs = graphCoarsener.CoarsenedGraphs;

            Assert.That(numberOfCoarsenedGraphs.Count, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_GivenSingleNodeGraph_ShouldSetLeastCoarsenedGraphToIt()
        {
            var singleNode = new Dictionary<int, List<int>> {{0, new List<int>()}};
            var graphCoarsener = new GraphCoarsener<int>(singleNode);

            var leastCoarsenedGraph = graphCoarsener.CoarsenedGraphs.First();

            Assert.That(leastCoarsenedGraph, Is.EquivalentTo(singleNode));
        }

        [Test]
        public void Constructor_GivenTwoAdjacentNodes_ShouldGenerateTwoCoarsenedGraphs()
        {
            var adjacentPair = new Dictionary<int, List<int>> { { 0, new List<int> {1} }, { 1, new List<int> {0} } };
            var graphCoarsener = new GraphCoarsener<int>(adjacentPair);

            var numberOfCoarsenedGraphs = graphCoarsener.CoarsenedGraphs;

            Assert.That(numberOfCoarsenedGraphs.Count, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_GivenTwoAdjacentNodes_ShouldGenerateMostCoarseGraphWithOneElement()
        {
            var adjacentPair = new Dictionary<int, List<int>> { { 0, new List<int>{1} }, { 1, new List<int>{0} } };
            var graphCoarsener = new GraphCoarsener<int>(adjacentPair);

            var mostCoarsenedGraph = graphCoarsener.CoarsenedGraphs.Last();

            Assert.That(mostCoarsenedGraph.Count, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_GivenTwoAdjacentNodes_ShouldGenerateMostCoarseGraphWhoseFirstNodeHasOneNeighbour()
        {
            var adjacentPair = new Dictionary<int, List<int>> { { 0, new List<int> { 1 } }, { 1, new List<int> { 0 } } };
            var graphCoarsener = new GraphCoarsener<int>(adjacentPair);

            var mostCoarsenedGraph = graphCoarsener.CoarsenedGraphs.Last();
            var firstNodesNeighbours = mostCoarsenedGraph.First().Value;

            Assert.That(firstNodesNeighbours.Count, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_GivenFourNodesInALine_ShouldGenerateThreeCoarsenedGraphs()
        {
            var fourNodesInALine = new Dictionary<int, List<int>>
            {
                {0, new List<int> {1}},
                {1, new List<int> {0, 2}},
                {2, new List<int> {1, 3}},
                {3, new List<int> {2}}
            };

            var graphCoarsener = new GraphCoarsener<int>(fourNodesInALine);

            var numberOfCoarsenedGraphs = graphCoarsener.CoarsenedGraphs;

            Assert.That(numberOfCoarsenedGraphs.Count, Is.EqualTo(3));
        }

        [Test]
        public void Constructor_GivenFourNodesInALine_ShouldGenerateSecondLeastCoarsenedGraphWithTwoElements()
        {
            var fourNodesInALine = new Dictionary<int, List<int>>
            {
                {0, new List<int> {1}},
                {1, new List<int> {0, 2}},
                {2, new List<int> {1, 3}},
                {3, new List<int> {2}}
            };

            var graphCoarsener = new GraphCoarsener<int>(fourNodesInALine);

            var secondLeastCoarsenedGraph = graphCoarsener.CoarsenedGraphs[1];

            Assert.That(secondLeastCoarsenedGraph.Count, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_GivenCompleteGraphOnThreeNodes_ShouldGenerateTwoCoarsenedGraphs()
        {
            var completeGraphOnThreeNodes = new Dictionary<int, List<int>>
            {
                {0, new List<int> {1, 2}},
                {1, new List<int> {0, 2}},
                {2, new List<int> {0, 1}}
            };

            var graphCoarsener = new GraphCoarsener<int>(completeGraphOnThreeNodes);

            var numberOfCoarsenedGraphs = graphCoarsener.CoarsenedGraphs;

            Assert.That(numberOfCoarsenedGraphs.Count, Is.EqualTo(2));
        }
    }
}
