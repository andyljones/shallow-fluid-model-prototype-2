using System.Linq;
using Foam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.ShallowFluidSimulator;
using Tests.Fakes;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidSimulatorTests
{
    [TestClass]
    public class DifferenceOperatorTests
    {
        private FakeAtmosphere _fakeAtmo;

        [TestInitialize]
        public void Create_Fake_Atmosphere_And_Simulator()
        {
            _fakeAtmo = new FakeAtmosphere();

            _fakeAtmo.Cells[0].Faces[4].DistanceBetweenFaceCenters = 5;
            _fakeAtmo.Cells[0].Faces[4].Width = 0.5f;
        }

        //[TestMethod]
        //public void Average_Of_Velocity_Potential_At_Outer_Edge_Should_Be_4f()
        //{
        //    var field = new FloatFieldInfo(typeof(Cell).GetField("VelocityPotential"));
        //    var edge = _fakeAtmo.Cells[0].Edges[0];

        //    var expectedAverage = 4f;
        //    var actualAverage = field.AverageAtEdge(edge);

        //    Assert.AreEqual(expectedAverage, actualAverage);
        //}

        //[TestMethod]
        //public void Average_Of_Velocity_Potential_At_Middle_Edge_Should_Be_6f()
        //{
        //    var field = new FloatFieldInfo(typeof(Cell).GetField("VelocityPotential"));
        //    var edge = _fakeAtmo.Cells[0].Edges[2];

        //    var expectedAverage = 6f;
        //    var actualAverage = field.AverageAtEdge(edge);

        //    Assert.AreEqual(expectedAverage, actualAverage);
        //}
    }
}