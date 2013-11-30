using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax;
using NUnit.Framework;
using Simulator.ShallowFluid;
using Simulator.ShallowFluid.MultigridSolver.Interpolator;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidTests.MultigridSolverTests.InterpolatorTests
{
    [TestFixture]
    public class WeightedAverageInterpolatorTests
    {
        [Test]
        public void Interpolate_OnANodeWithOneNeighbour_ShouldCopyTheNeighboursValue()
        {
            var graph = new Graph<int> {{0, new List<int> {1}}};
            var relativePositions = new VectorFieldMap<int>
            {
                {0, new VectorField<int> {{1, new Vector3(1, 0, 0)}}}
            };
            var geometry = A.Fake<IGeometry<int>>();
            A.CallTo(() => geometry.Graph).Returns(graph);
            A.CallTo(() => geometry.RelativePositions).Returns(relativePositions);
            var interpolator = new WeightedAverageInterpolator<int>(geometry);

            var targetField = new ScalarField<int> { { 0, 3f } };            
            var sourceField = new ScalarField<int> {{1, 7f}};
            interpolator.Interpolate(sourceField, ref targetField);

            Assert.That(targetField[0], Is.EqualTo(7));
        }

        [Test]
        public void Interpolate_OnANodeWithTwoNeighboursAtEqualDistances_ShouldAverageTheNeighboursValues()
        {
            var graph = new Graph<int> { { 0, new List<int> { 1, 2 } } };
            var relativePositions = new VectorFieldMap<int>
            {
                {0, new VectorField<int> {{1, new Vector3(1, 0, 0)}, {2, new Vector3(0, 1, 0)}}}
            };
            var geometry = A.Fake<IGeometry<int>>();
            A.CallTo(() => geometry.Graph).Returns(graph);
            A.CallTo(() => geometry.RelativePositions).Returns(relativePositions);
            var interpolator = new WeightedAverageInterpolator<int>(geometry);

            var sourceField = new ScalarField<int> { { 1, 7f }, {2, 13f} };
            var targetField = new ScalarField<int> { { 0, 11f } };
            interpolator.Interpolate(sourceField, ref targetField);

            Assert.That(targetField[0], Is.EqualTo(10));
        }

        [Test]
        public void Interpolate_OnANodeWithTwoNeighboursAtDifferentDistances_ShouldAverageTheNeighboursValues()
        {
            var graph = new Graph<int> { { 0, new List<int> { 1, 2 } } };
            var relativePositions = new VectorFieldMap<int>
            {
                {0, new VectorField<int> {{1, new Vector3(1, 0, 0)}, {2, new Vector3(0, 2, 0)}}}
            };
            var geometry = A.Fake<IGeometry<int>>();
            A.CallTo(() => geometry.Graph).Returns(graph);
            A.CallTo(() => geometry.RelativePositions).Returns(relativePositions);
            var interpolator = new WeightedAverageInterpolator<int>(geometry);

            var sourceField = new ScalarField<int> { { 1, 7f }, { 2, 10f } };
            var targetField = new ScalarField<int> { { 0, 11f } };
            interpolator.Interpolate(sourceField, ref targetField);

            Assert.That(targetField[0], Is.EqualTo(8));
        }
    }
}
