using System.Collections.Generic;
using Simulator.ShallowFluid.MultigridSolver.Interpolator;

namespace Simulator.ShallowFluid.MultigridSolver
{
    /// <summary>
    /// Contains methods for solving the elliptic equation Lapacian(U) = f over a two-dimensional field using a multigrid method. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultigridSolver<T>
    {
        private readonly List<Graph<T>> _coarsenedGraphs;
        private readonly IGeometry<T> _geometry;
        private readonly IInterpolator<T> _interpolator; 

        public MultigridSolver(Graph<T> graph, IGeometry<T> geometry )
        {
            _coarsenedGraphs = new List<Graph<T>> { graph };
            _geometry = geometry;
        }

        public ScalarField<T> Solve(ScalarField<T> U, ScalarField<T> f)
        {
            var solution = new ScalarField<T>(U.Keys);

            return solution;
        }


    }
}
