using System;
using System.Collections.Generic;
using NUnit.Framework;
using Simulator.ShallowFluid;
using Tests.AtmosphereTests.MonolayerAtmosphereTests;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidTests
{
    [TestFixture]
    public class MultigridSolverTests
    {
        [Test]
        public void Constructor_SetsCoarsenedGraphsToOneElementListContainingProvidedGraph()
        {
            var graph = new Graph<int> { { 0, default(List<int>) } };
            var geometry = new FakeGeometry();
            var solver = new MultigridSolver<int>(graph, geometry);

            var expectedCoarsenedGraphs = new List<Graph<int>> {graph}; 
            var coarsenedGraphs = solver.CoarsenedGraphs;

            Assert.That(coarsenedGraphs, Is.EquivalentTo(expectedCoarsenedGraphs));
        }

        [Test]
        public void Solve_GivenAPairOfScalarFieldsWithSameKeys_ReturnsFieldWithSameKeysAsU()
        {
            var graph = new Graph<int> {{0, default(List<int>)}};
            var geometry = new FakeGeometry();
            var solver = new MultigridSolver<int>(graph, geometry);

            var U = new ScalarField<int> {{0, 0f}};
            var f = new ScalarField<int> {{0, 0f}};

            var resultKeys = solver.Solve(U, f).Keys;

            Assert.That(resultKeys, Is.EquivalentTo(U.Keys));
        }
    }
}
