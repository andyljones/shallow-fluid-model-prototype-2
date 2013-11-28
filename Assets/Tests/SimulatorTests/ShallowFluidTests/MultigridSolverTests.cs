using System;
using System.Collections.Generic;
using NUnit.Framework;
using Simulator.ShallowFluid;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidTests
{
    [TestFixture]
    public class MultigridSolverTests
    {
        [Test]
        public void Constructor_SetsCoarsenedGraphsToOneElementListContainingProvidedGraph()
        {
            var graph = new AdjacencyDictionary<int> { { 0, default(List<int>) } };
            var positions = new VectorField<int> { { 0, default(Vector3) } };
            var solver = new MultigridSolver<int>(graph, positions);

            var expectedCoarsenedGraphs = new List<AdjacencyDictionary<int>> {graph}; 
            var coarsenedGraphs = solver.CoarsenedGraphs;

            Assert.That(coarsenedGraphs, Is.EquivalentTo(expectedCoarsenedGraphs));
        }

        [Test]
        public void Solve_GivenAPairOfScalarFieldsWithSameKeys_ReturnsFieldWithSameKeysAsU()
        {
            var graph = new AdjacencyDictionary<int> {{0, default(List<int>)}};
            var positions = new VectorField<int> {{0, default(Vector3)}};
            var solver = new MultigridSolver<int>(graph, positions);

            var U = new ScalarField<int> {{0, 0f}};
            var f = new ScalarField<int> {{0, 0f}};

            var resultKeys = solver.Solve(U, f).Keys;

            Assert.That(resultKeys, Is.EquivalentTo(U.Keys));
        }
    }
}
