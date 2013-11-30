using System.Collections.Generic;
using NUnit.Framework;
using Simulator.ShallowFluid;
using Simulator.ShallowFluid.MultigridSolver;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidTests.MultigridSolverTests
{
    [TestFixture]
    public class RelativePositionCalculatorTests
    {
        [Test]
        public void RelativePositionList_OfADisconnectedNode_ShouldBeEmpty()
        {
            var graph = new Graph<int> {{0, new List<int>()}};
            var positions = new VectorField<int> {{0, default(Vector3)}};
            var calculator = new RelativePositionCalculator<int>(graph, positions);

            var relativePositions = calculator.RelativePositions[0];

            Assert.That(relativePositions, Is.Empty);
        }

        [Test]
        public void RelativePosition_OfOneNodeFromAnother_ShouldBeOfCorrectMagnitude()
        {
            var graph = new Graph<int> {{0, new List<int> {1}}, {1, new List<int> {0}}};
            var positions = new VectorField<int> {{0, new Vector3(1, 0, 0)}, {1, new Vector3(0, 1, 0)}};
            var calculator = new RelativePositionCalculator<int>(graph, positions);
            var relativePosition = calculator.RelativePositions[0][1];

            var expectedMagnitude = Mathf.PI / 2;
            var actualMagnitude = relativePosition.magnitude;

            var tolerance = 0.001f;

            Assert.That(actualMagnitude, Is.InRange(expectedMagnitude - tolerance, expectedMagnitude + tolerance));
        }

        [Test]
        public void RelativePosition_OfOneNodeFromAnother_ShouldBePerpendicularToOriginNodesPosition()
        {
            var graph = new Graph<int> { { 0, new List<int> { 1 } }, { 1, new List<int> { 0 } } };
            var positions = new VectorField<int> { { 0, new Vector3(1, 0, 0) }, { 1, new Vector3(0, 1, 0) } };
            var calculator = new RelativePositionCalculator<int>(graph, positions);

            var originPosition = positions[0];
            var relativePosition = calculator.RelativePositions[0][1];

            var tolerance = 0.001f;

            Assert.That(Vector3.Dot(originPosition, relativePosition), Is.InRange(0 - tolerance, 0 + tolerance));
        }
    }
}
