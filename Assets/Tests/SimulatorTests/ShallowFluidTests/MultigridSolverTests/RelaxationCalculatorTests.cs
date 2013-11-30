using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using Simulator.ShallowFluid;
using Simulator.ShallowFluid.MultigridSolver;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidTests.MultigridSolverTests
{
    [TestFixture]
    public class RelaxationCalculatorTests
    {
        [Test]
        public void Relax_OnAnIsolatedNode_ShouldRelaxAnyFieldToZero()
        {
            var graph = new Graph<int> {{0, new List<int>()}};
            var geometry = A.Fake<IGeometry<int>>();
            var calculator = new RelaxationCalculator<int>(graph, geometry);

            var field = new ScalarField<int> {{0, 2f}};
            var laplacianOfField = new ScalarField<int> {{0, 3f}};

            calculator.Relax(ref field, laplacianOfField);

            Assert.That(field.Values, Has.All.EqualTo(0));
        }

        [Test]
        public void Relax_OnANodeWithASingleNeighbour_ShouldRelaxTheFieldCorrectly()
        {
            var graph = new Graph<int> { { 0, new List<int> {1} } };
            var geometry = A.Fake<IGeometry<int>>();
            A.CallTo(() => geometry.Areas).Returns(new ScalarField<int> { { 0, 4f } });
            A.CallTo(() => geometry.InternodeDistances).Returns(new ScalarFieldMap<int> { {0, new ScalarField<int> { { 1, 5f } } } });
            A.CallTo(() => geometry.Widths).Returns(new ScalarFieldMap<int> { { 0, new ScalarField<int> { { 1, 6f } } } });

            var calculator = new RelaxationCalculator<int>(graph, geometry);

            var field = new ScalarField<int> { { 1, 2f } };
            var laplacianOfField = new ScalarField<int> { { 0, 3f } };

            calculator.Relax(ref field, laplacianOfField);

            Assert.That(field[0], Is.EqualTo(-8f));
        }

        //TODO: Add test for two neighbours, and a test for two nodes.
    }
}
