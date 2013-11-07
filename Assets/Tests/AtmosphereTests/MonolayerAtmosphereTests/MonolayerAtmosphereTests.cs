using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Atmosphere.MonolayerAtmosphere;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using strange.extensions.injector.impl;
using Surfaces;
using Tests.Fakes;

namespace Tests.AtmosphereTests.MonolayerAtmosphereTests
{
    [TestClass]
    public class MonolayerAtmosphereTests
    {
        private IAtmosphere _atmosphere;

        [TestInitialize]
        public void Create_Monolayer_Atmosphere_From_Fake_Surface()
        {
            var options = A.Fake<IMonolayerAtmosphereOptions>();
            A.CallTo(() => options.Height).Returns(3);

            var binder = new InjectionBinder();
            binder.Bind<ISurface>().To<FakeSurface>();
            binder.Bind<IMonolayerAtmosphereOptions>().ToValue(options);
            binder.Bind<IAtmosphere>().To<MonolayerAtmosphere>();

            _atmosphere = binder.GetInstance<IAtmosphere>() as IAtmosphere;
        }

        [TestMethod]
        public void Atmosphere_Should_Have_Two_Cells()
        {
            var expectedNumberOfCells = 2;
            var actualNumberOfCells = _atmosphere.Cells.Count;

            Assert.AreEqual(expectedNumberOfCells, actualNumberOfCells);
        }

        [TestMethod]
        public void Each_Cell_Should_Have_Two_Faces()
        {
            var expectedFaceCounts = 2;
            var actualFaceCounts = _atmosphere.Cells.Select(cell => cell.Faces.Count);

            Assert.IsTrue(actualFaceCounts.All(count => count == expectedFaceCounts));
        }

        [TestMethod]
        public void Each_Cell_Should_Have_Six_Vertices()
        {
            var expectedVertexCount = 6;
            var actualVertexCount = _atmosphere.Cells.Select(cell => cell.Vertices.Count);

            Assert.IsTrue(actualVertexCount.All(count => count == expectedVertexCount));
        }

        [TestMethod]
        public void Four_Vertices_Should_Have_Magnitude_10_And_Four_Should_Have_Magnitude_13()
        {
            var vertices = _atmosphere.Cells.SelectMany(cell => cell.Vertices).Distinct();
            
            var expectedMagnitudes = new[] {10f, 10f, 10f, 10f, 13f, 13f, 13f, 13f};
            var actualMagnitudes = vertices.Select(vertex => vertex.Position.magnitude).ToList();

            CollectionAssert.AreEquivalent(expectedMagnitudes, actualMagnitudes);
        }

        [TestMethod]
        public void Each_Cell_Should_Have_Nine_Edges()
        {
            var expectedEdgeCount = 6;
            var actualEdgeCount = _atmosphere.Cells.Select(cell => cell.Edges.Count);

            Assert.IsTrue(actualEdgeCount.All(count => count == expectedEdgeCount));
        }
    }
}
