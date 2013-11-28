﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foam;

namespace Simulator.ShallowFluid
{
    /// <summary>
    /// Generates a list of ever-more-coarsened graphs from an initial graph using a greedy algorithm.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GreedyGraphCoarsener<T>
    {
        /// <summary>
        /// A list of the coarsened graphs generated from the graph given to the constructor. Given in 
        /// finest-to-coarsest order. 
        /// </summary>
        public List<Graph<T>> CoarsenedGraphs { get; private set; }

        /// <summary>
        /// The ith graph in this list associates the nodes of the ith graph in CoarsenedGraphs with the nearest
        ///  elements of the next coarsest graph.
        /// </summary>
        public List<Graph<T>> CoarseNeighbourGraphs { get; private set; } 

        /// <summary>
        /// Takes an adjacency dictionary describing a graph, and generates a list of coarsened  graphs from it. 
        /// These graphs can be found in the CoarsenedGraph field.
        /// </summary>
        /// <param name="graph">The graph to be coarsened</param>
        public GreedyGraphCoarsener(Graph<T> graph)
        {
            var currentMostCoarseGraph = graph;
            CoarsenedGraphs = new List<Graph<T>> { currentMostCoarseGraph };
            CoarseNeighbourGraphs = new List<Graph<T>>();

            // The coarsest possible graph has a single node, so we keep coarsening until we get an adjacency 
            // dictionary with one entry.
            while (currentMostCoarseGraph.Count > 1)
            {
                var moreCoarseGraph = CoarsenGraph(currentMostCoarseGraph);
                var coarseNeighbourGraph = CoarseNeighboursOf(currentMostCoarseGraph, moreCoarseGraph);

                CoarsenedGraphs.Add(moreCoarseGraph);
                CoarseNeighbourGraphs.Add(coarseNeighbourGraph);
                currentMostCoarseGraph = moreCoarseGraph;

            }
        }

        // Associates each node in graph with the nearest nodes in coarseGraph.
        private Graph<T> CoarseNeighboursOf(Graph<T> graph, Graph<T> coarseGraph)
        {
            var coarseNodes = new HashSet<T>(coarseGraph.Keys);
            var coarseNeighbours = new Graph<T>();

            foreach (var node in graph.Keys)
            {
                if (coarseNodes.Contains(node)) // Case where the node itself is present in the coarsened graph.
                {
                    coarseNeighbours.Add(node, new List<T> {node});
                }
                else // Case where the node is the neighbour of a node in the coarsened graph.
                {
                    var firstNeighbours = graph[node];
                    var firstCoarseNeighbours = firstNeighbours.Intersect(coarseNodes).ToList();
                    coarseNeighbours.Add(node, firstCoarseNeighbours);
                }
            }

            return coarseNeighbours;
        }

        // Coarsens a graph by finding a maximal independent set of nodes and linking those that are at distance 2 from eachother. 
        private Graph<T> CoarsenGraph(Graph<T> graph)
        {
            var maximalIndependentSet = FindMaximalIndependentSetIn(graph);
            var graphOfMaximalIndependentSet = SecondNeighbourSubgraphOf(maximalIndependentSet, graph);

            return graphOfMaximalIndependentSet;
        }

        // Constructs a graph from the given subset where two nodes are adjacent if they're at distance 2 from eachother in the original graph.
        private Graph<T> SecondNeighbourSubgraphOf(List<T> subsetOfNodes, Graph<T> graph)
        {
            var subgraph = new Graph<T>();

            foreach (var node in subsetOfNodes)
            {
                var secondNeighbours = NthNeighbourhoodOf(node, graph, 2);
                var secondNeighboursInSubset = secondNeighbours.Intersect(subsetOfNodes).ToList();
                secondNeighboursInSubset.Remove(node); // One of the second neighbours of a node is itself, but we don't want that.
                subgraph.Add(node, secondNeighboursInSubset);
            }

            return subgraph;
        }

        // Returns a list of all nodes within a given distance of the specified node. 
        private List<T> NthNeighbourhoodOf(T node, Graph<T> graph, int distance)
        {
            var neighbours = new List<T> {node};

            while (distance > 0)
            {
                //TODO: Search alg here is a combinatorial explosion waiting to happen.
                neighbours = neighbours.SelectMany(neighbour => graph[neighbour]).Distinct().ToList();
                distance--;
            }

            return neighbours;
        }

        // Greedily finds a maximal independent set of nodes in the graph.
        private List<T> FindMaximalIndependentSetIn(Graph<T> graph)
        {
            var independentSet = new List<T>();            
            // This is the set of nodes who aren't in the independent set and don't have neighbours in it.
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
