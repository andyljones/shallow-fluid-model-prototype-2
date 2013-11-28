using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Atmosphere.MonolayerAtmosphere;
using FakeItEasy;
using NUnit.Framework;
using strange.extensions.injector.impl;
using Surfaces;
using Tests.Fakes;

namespace Tests.AtmosphereTests.MonolayerAtmosphereTests
{
    [TestFixture]
    public class MonolayerAtmosphereTests
    {
        private IAtmosphere _atmosphere;

        [SetUp]
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

        [Test]
        public void Atmosphere_Should_Have_Two_Cells()
        {
            var expectedNumberOfCells = 2;
            var actualNumberOfCells = _atmosphere.Cells.Count;

            Assert.AreEqual(expectedNumberOfCells, actualNumberOfCells);
        }

        [Test]
        public void Each_Cell_Should_Have_Five_Faces()
        {
            var expectedFaceCounts = 5;
            var actualFaceCounts = _atmosphere.Cells.Select(cell => cell.Faces.Count);

            Assert.IsTrue(actualFaceCounts.All(count => count == expectedFaceCounts));
        }

        [Test]
        public void Each_Cell_Should_Have_Six_Vertices()
        {
            var expectedVertexCount = 6;
            var actualVertexCount = _atmosphere.Cells.Select(cell => cell.Vertices.Count);

            Assert.IsTrue(actualVertexCount.All(count => count == expectedVertexCount));
        }

        [Test]
        public void Four_Distinct_Vertices_Should_Have_Magnitude_10_And_Four_Should_Have_Magnitude_13()
        {
            var vertices = _atmosphere.Cells.SelectMany(cell => cell.Vertices).Distinct();
            
            var expectedMagnitudes = new[] {10f, 10f, 10f, 10f, 13f, 13f, 13f, 13f};
            var actualMagnitudes = vertices.Select(vertex => vertex.Position.magnitude).ToList();

            CollectionAssert.AreEquivalent(expectedMagnitudes, actualMagnitudes);
        }

        [Test]
        public void There_Should_Be_Fourteen_Distinct_Edges_Total()
        {
            var expectedEdgeCount = 14;
            var actualEdgeCount = _atmosphere.Cells.SelectMany(cell => cell.Edges).Distinct().Count();

            Assert.AreEqual(expectedEdgeCount, actualEdgeCount);
        }

        [Test]
        public void Each_Cell_Should_Have_Nine_Distinct_Edges()
        {
            var expectedEdgeCount = 9;
            var actualEdgeCount = _atmosphere.Cells.Select(cell => cell.Edges.Count).Distinct();

            Assert.IsTrue(actualEdgeCount.All(count => count == expectedEdgeCount));
        }

        [Test]
        public void Ten_Distinct_Edges_Should_Link_Vertices_Of_The_Same_Magnitude()
        {
            var edges = _atmosphere.Cells.SelectMany(cell => cell.Edges).Distinct();
            var edgesLinkingVerticesOfSameMagnitude =
                edges.Where(edge => edge.Vertices.First().Position.magnitude == edge.Vertices.Last().Position.magnitude);

            var expectedNumberOfEdges = 10;
            var actualNumberOfEdges = edgesLinkingVerticesOfSameMagnitude.Count();

            Assert.AreEqual(expectedNumberOfEdges, actualNumberOfEdges);
        }

        [Test]
        public void Five_Distinct_Edges_Should_Link_Vertices_Of_Different_Magnitude()
        {
            var edges = _atmosphere.Cells.SelectMany(cell => cell.Edges).Distinct();
            var edgesLinkingVerticesOfDifferentMagnitude =
                edges.Where(edge => edge.Vertices.First().Position.magnitude != edge.Vertices.Last().Position.magnitude);

            var expectedNumberOfEdges = 4;
            var actualNumberOfEdges = edgesLinkingVerticesOfDifferentMagnitude.Count();

            Assert.AreEqual(expectedNumberOfEdges, actualNumberOfEdges);
        }

        [Test]
        public void There_Should_Be_Nine_Faces_Total()
        {
            var expectedNumberOfFaces = 9;
            var actualNumberOfFaces = _atmosphere.Cells.SelectMany(Cell => Cell.Faces).Distinct().Count();

            Assert.AreEqual(expectedNumberOfFaces, actualNumberOfFaces);
        }

        [Test]
        public void There_Should_Be_Eight_Vertices_Total()
        {
            var expectedNumberOfVertices = 8;
            var actualNumberOfVertices = _atmosphere.Cells.SelectMany(Cell => Cell.Vertices).Distinct().Count();

            Assert.AreEqual(expectedNumberOfVertices, actualNumberOfVertices);
        }
    }
}
