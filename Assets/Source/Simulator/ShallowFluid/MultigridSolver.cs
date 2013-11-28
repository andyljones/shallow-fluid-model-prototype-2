using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Simulator.ShallowFluid
{
    /// <summary>
    /// Contains methods for solving the elliptic equation Lapacian(U) = f over a two-dimensional field using a multigrid method. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultigridSolver<T>
    {
        public List<AdjacencyDictionary<T>> CoarsenedGraphs { get; set; }
        private readonly VectorField<T> _positions;

        public MultigridSolver(AdjacencyDictionary<T> graph, VectorField<T> positions)
        {
            Debug.Assert(graph.Keys.SequenceEqual(positions.Keys), "Graph and position arguments concern different sets of objects!");

            CoarsenedGraphs = new List<AdjacencyDictionary<T>> { graph };
            _positions = positions;
        }

        public ScalarField<T> Solve(ScalarField<T> U, ScalarField<T> f)
        {
            Debug.Assert(U.Keys.SequenceEqual(f.Keys), "Fields U and f concern different sets of objects!");
            Debug.Assert(U.Keys.SequenceEqual(_positions.Keys), "Fields U and f concern a different set of objects to the position field!");

            var solution = new ScalarField<T>(U.Keys);

            return solution;
        }


    }
}
