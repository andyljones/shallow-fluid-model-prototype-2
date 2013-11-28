using System.Linq;
using Foam;
using Grids;
using Grids.GeodesicGridGenerator;
using Initialization;
using NUnit.Framework;
using UnityEngine;

//TODO: Add some more tests
namespace Tests.GridTests.GeodesicGridTests
{
    [TestFixture]
    public class GeodesicGridGeneratorTests
    {
        private IGrid _grid;

        [SetUp]
        public void Create_Icosahedral_Grid_With_Angular_Resolution_Of_1f()
        {
            var options = new Options { Radius = 1f, Resolution = 1f };
            _grid = new GeodesicGrid(options);
        }

        [Test]
        public void Should_Be_120_Edges()
        {
            var edges = _grid.Faces.SelectMany(face => face.Edges).Distinct();

            var expectedNumberOfEdges = 120;
            var actualNumberOfEdges = edges.Count();

            Assert.AreEqual(expectedNumberOfEdges, actualNumberOfEdges);
        }

        [Test]
        public void Should_Be_80_Vertices()
        {
            var vertices = _grid.Faces.SelectMany(face => face.Vertices).Distinct();

            var expectedNumberOfVertices = 80;
            var actualNumberOfEdges = vertices.Count();
            Assert.AreEqual(expectedNumberOfVertices, actualNumberOfEdges);
        }

        [Test]
        public void Every_Edge_Should_Have_Two_Vertices()
        {
            var edges = _grid.Faces.SelectMany(face => face.Edges).Distinct();

            var expectedVerticesOnEachEdge = Enumerable.Repeat(2, 120).ToList();
            var actualVerticesOnEachEdge = edges.Select(edge => edge.Vertices.Count).ToList();

            CollectionAssert.AreEqual(expectedVerticesOnEachEdge, actualVerticesOnEachEdge);
        }

        [Test]
        public void There_Should_Be_12_Faces_With_Five_Edges()
        {
            var expectedNumberOfFaces = 12;
            var actualNumberOfFaces = _grid.Faces.Count(face => face.Edges.Count == 5);

            Assert.AreEqual(expectedNumberOfFaces, actualNumberOfFaces);
        }

        [Test]
        public void There_Should_Be_30_Faces_With_Six_Edges()
        {
            var expectedNumberOfFaces = 30;
            var actualNumberOfFaces = _grid.Faces.Count(face => face.Edges.Count == 6);

            Assert.AreEqual(expectedNumberOfFaces, actualNumberOfFaces);
        }

        [Test]
        public void Every_Edge_Should_Have_Two_Faces()
        {
            var edges = _grid.Faces.SelectMany(face => face.Edges).Distinct();

            var expectedFaceCount = 2;
            var actualFaceCounts = edges.Select(edge => edge.Faces.Count);

            Assert.IsTrue(actualFaceCounts.All(count => count == expectedFaceCount));
        }
    }
}
