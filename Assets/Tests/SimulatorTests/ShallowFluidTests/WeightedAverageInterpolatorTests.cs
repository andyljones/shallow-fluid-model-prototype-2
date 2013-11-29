using System.Collections.Generic;
using NUnit.Framework;
using Simulator.ShallowFluid;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidTests
{
    [TestFixture]
    public class WeightedAverageInterpolatorTests
    {
        [Test]
        public void Interpolate_OnANodeWithOneNeighbour_ShouldCopyTheNeighboursValue()
        {
            var relativePositions = new Dictionary<int, VectorField<int>>
            {
                {0, new VectorField<int> {{1, new Vector3(1, 0, 0)}}}
            };
            var interpolator = new WeightedAverageInterpolator<int>(relativePositions);
            
            var field = new ScalarField<int> {{1, 7f}};
            var result = interpolator.Interpolate(field);

            var interpolatedValue = result[0];

            Assert.That(interpolatedValue, Is.EqualTo(7));
        }

        [Test]
        public void Interpolate_OnANodeWithTwoNeighboursAtEqualDistances_ShouldAverageTheNeighboursValues()
        {
            var relativePositions = new Dictionary<int, VectorField<int>>
            {
                {0, new VectorField<int> {{1, new Vector3(1, 0, 0)}, {2, new Vector3(0, 1, 0)}}}
            };
            var interpolator = new WeightedAverageInterpolator<int>(relativePositions);

            var field = new ScalarField<int> { { 1, 7f }, {2, 13f} };
            var result = interpolator.Interpolate(field);

            var interpolatedValue = result[0];

            Assert.That(interpolatedValue, Is.EqualTo(10));
        }

        [Test]
        public void Interpolate_OnANodeWithTwoNeighboursAtDifferentDistances_ShouldAverageTheNeighboursValues()
        {
            var relativePositions = new Dictionary<int, VectorField<int>>
            {
                {0, new VectorField<int> {{1, new Vector3(1, 0, 0)}, {2, new Vector3(0, 2, 0)}}}
            };
            var interpolator = new WeightedAverageInterpolator<int>(relativePositions);

            var field = new ScalarField<int> { { 1, 7f }, { 2, 10f } };
            var result = interpolator.Interpolate(field);

            var interpolatedValue = result[0];

            Assert.That(interpolatedValue, Is.EqualTo(8f));
        }
    }
}
