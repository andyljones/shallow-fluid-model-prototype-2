using System;
using System.Collections.Generic;
using System.Linq;
using Foam;

namespace Simulator.ShallowFluid
{
    /// <summary>
    /// Generates a list of ever-more-coarsened graphs from an initial graph.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GraphCoarsener<T>
    {
        /// <summary>
        /// A list of the coarsened graphs generated from the graph given to the constructor. Given in 
        /// finest-to-coarsest order. 
        /// </summary>
        public List<Dictionary<T, List<T>>> CoarsenedGraphs { get; private set; }

        /// <summary>
        /// Takes an adjacency dictionary describing a graph, and generates a list of coarsened  graphs from it. 
        /// These graphs can be found in the CoarsenedGraph field.
        /// </summary>
        /// <param name="graph">The graph to be coarsened</param>
        public GraphCoarsener(Dictionary<T, List<T>> graph)
        {
            var mostCoarseGraph = graph;
            CoarsenedGraphs = new List<Dictionary<T, List<T>>> { mostCoarseGraph };

            // The coarsest possible graph has a single node, so we keep coarsening until we get an adjacency 
            // dictionary with one element.
            while (mostCoarseGraph.Count > 1)
            {
                mostCoarseGraph = CoarsenGraph(mostCoarseGraph);
                CoarsenedGraphs.Add(mostCoarseGraph);
            }
        }

        // Coarsens a graph by finding a maximal independent set of nodes and linking those that are at distance 2 from eachother. 
        private Dictionary<T, List<T>> CoarsenGraph(Dictionary<T, List<T>> graph)
        {
            var maximalIndependentSet = FindMaximalIndependentSetIn(graph);
            var graphOfMaximalIndependentSet = SecondNeighbourSubgraphOf(maximalIndependentSet, graph);

            return graphOfMaximalIndependentSet;
        }

        // Constructs a graph from the given subset where two nodes are adjacent if they're at distance 2 from eachother in the original graph.
        private Dictionary<T, List<T>> SecondNeighbourSubgraphOf(List<T> subsetOfNodes, Dictionary<T, List<T>> graph)
        {
            var subgraph = new Dictionary<T, List<T>>();

            foreach (var node in subsetOfNodes)
            {
                var firstNeighbours = graph[node];
                var secondNeighbours = firstNeighbours.SelectMany(neighbour => graph[neighbour]).Distinct();
                var secondNeighboursInSubset = secondNeighbours.Intersect(subsetOfNodes).ToList();
                subgraph.Add(node, secondNeighboursInSubset);
            }

            return subgraph;
        }

        // Greedily finds a maximal independent set of nodes in the graph.
        private List<T> FindMaximalIndependentSetIn(Dictionary<T, List<T>> graph)
        {
            var independentSet = new List<T>();            
            // Set of nodes who aren't in the independent set and don't have neighbours in it.
            var uncoveredNodes = new HashSet<T>(graph.Keys); 

            while (uncoveredNodes.Count > 0)
            {
                var independentNode = uncoveredNodes.First();
                independentSet.Add(independentNode);
                uncoveredNodes.Remove(independentNode);

                var neighboursOfIndependentNode = graph[independentNode];
                uncoveredNodes.ExceptWith(neighboursOfIndependentNode);
            }

            return independentSet;
        }
    }
}
