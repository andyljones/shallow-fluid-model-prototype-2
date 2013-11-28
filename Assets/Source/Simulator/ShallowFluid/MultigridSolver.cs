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
        public List<Graph<T>> CoarsenedGraphs { get; set; }
        private readonly IGeometry<T> _geometry;

        public MultigridSolver(Graph<T> graph, IGeometry<T> geometry )
        {
            CoarsenedGraphs = new List<Graph<T>> { graph };
            _geometry = geometry;
        }

        public ScalarField<T> Solve(ScalarField<T> U, ScalarField<T> f)
        {
            var solution = new ScalarField<T>(U.Keys);

            return solution;
        }


    }
}
