using System.Collections.Generic;
using NUnit.Framework;
using Simulator.ShallowFluid;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidTests
{
    [TestFixture]
    public class LinearInterpolatorTests
    {
        [Test]
        public void Constructor_GivenATwoNodeGraph_CorrectlyCalculatesLocalCoordinates()
        {
            var graph = new Graph<int> {{0, new List<int> {1}}, {1, new List<int>()}};
            var positions = new VectorField<int> {{0, new Vector3(1, 0, 0)}, {1, new Vector3(0, 1, 0)}};

            //var interpolator = new LinearInterpolator<int>(graph, positions);

        }
    }
}
