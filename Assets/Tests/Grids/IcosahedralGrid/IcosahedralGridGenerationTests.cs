using System.Linq;
using ClimateSim.Assets.Source.Grids;
using ClimateSim.Grids.IcosahedralGrid;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;

namespace ClimateSim.Tests.Grids.IcosahedralGrid
{
    [TestClass]
    public class IcosahedralGridGeneratorTests
    {
        private IcosahedralGridGenerator _gridGenerator;

        [TestInitialize]
        public void Create_Icosahedral_Grid_With_Angular_Resolution_Of_1f()
        {
            var options = new Options() {Radius = 1f, Resolution = 1f};
            _gridGenerator = new IcosahedralGridGenerator(options);
        }

        [TestMethod]
        public void Specifying_Angular_Resolution_Greater_Than_1_Point_06_Should_Return_An_Icosahedron()
        {
            var icosahedronOptions = new Options() { Radius = 1f, Resolution = 2f };

            _gridGenerator = new IcosahedralGridGenerator(icosahedronOptions);

            var expectedNumberOfFaces = 20;
            var actualNumberOfFaces = _gridGenerator.Faces.Count;
            Assert.AreEqual(expectedNumberOfFaces, actualNumberOfFaces);
        }

        [TestMethod]
        public void All_Old_Edges_Should_Be_Deleted()
        {
            var edgesLongerThanPoint5 = _gridGenerator.Edges
                                        .Where(edge => (edge.Vertices[0].Position - edge.Vertices[1].Position).magnitude > 1)
                                        .ToList();

            Assert.AreEqual(0, edgesLongerThanPoint5.Count);
        }

        [TestMethod]
        public void Should_Be_120_Edges()
        {
            var expectedNumberOfEdges = 120;
            var actualNumberOfEdges = _gridGenerator.Edges.Count;

            Assert.AreEqual(expectedNumberOfEdges, actualNumberOfEdges);
        }

        [TestMethod]
        public void Should_Be_42_Vertices()
        {
            var expectedNumberOfVertices = 42;
            var actualNumberOfEdges = _gridGenerator.Vertices.Count;
            Assert.AreEqual(expectedNumberOfVertices, actualNumberOfEdges);
        }

        [TestMethod]
        public void Every_Edge_Should_Have_Two_Vertices()
        {
            var expectedVerticesOnEachEdge = Enumerable.Repeat(2, 120).ToList();
            var actualVerticesOnEachEdge = _gridGenerator.Edges.Select(edge => edge.Vertices.Count).ToList();

            CollectionAssert.AreEqual(expectedVerticesOnEachEdge, actualVerticesOnEachEdge);
        }

        [TestMethod]
        public void Every_Edge_Should_Be_Of_Roughly_The_Same_Length()
        {
            var expectedLength = 0.546533048f; //No neat closed form unfortunately.
            var actualLengthsOfEachEdge = _gridGenerator.Edges
                                          .Select(edge => (edge.Vertices[0].Position - edge.Vertices[1].Position).magnitude).ToList();

            var errors = actualLengthsOfEachEdge.Select(actualLength => Mathf.Abs(actualLength - expectedLength));

            Assert.IsTrue(errors.All(error => error < 0.01f));
        }

        [TestMethod]
        public void There_Should_Be_12_Vertices_With_Five_Edges()
        {
            var expectedNumberOfVertices = 12;
            var actualNumberOfVertices = _gridGenerator.Vertices.Count(vertex => vertex.Edges.Count == 5);

            Assert.AreEqual(expectedNumberOfVertices, actualNumberOfVertices);
        }

        [TestMethod]
        public void There_Should_Be_30_Vertices_With_Six_Edges()
        {
            var expectedNumberOfVertices = 30;
            var actualNumberOfVertices = _gridGenerator.Vertices.Count(vertex => vertex.Edges.Count == 6);

            Assert.AreEqual(expectedNumberOfVertices, actualNumberOfVertices);
        }

        [TestMethod]
        public void There_Should_Be_A_North_Pole_And_A_South_Pole()
        {
            var expectedDirection = new Vector3(0, 0, 1);
            var numberOfNearbyVertices = _gridGenerator.Vertices
                                         .Count(vertex => Vector3.Cross(expectedDirection, vertex.Position).magnitude < 0.01f);

            Assert.AreEqual(2, numberOfNearbyVertices);
        }

        [TestMethod]
        public void There_Should_Be_Five_Vertices_Half_Way_Between_The_Pole_And_The_Old_North_Latitude()
        {
            var expectedZValue = 0.8506508f;
            var numberOfNearbyVertices = _gridGenerator.Vertices
                                         .Count(vertex => Mathf.Abs(vertex.Position.z - expectedZValue) < 0.01f);

            Assert.AreEqual(5, numberOfNearbyVertices);
        }

        [TestMethod]
        public void Vertex_Indices_Should_Form_Contiguous_Range()
        {
            var expectedIndices = Enumerable.Range(0, 42).ToList();
            var actualIndices = _gridGenerator.Vertices.Select(vertex => vertex.Index).ToList();

            CollectionAssert.AreEquivalent(expectedIndices, actualIndices);
        }

        //[TestMethod]
        //public void Edge_Indices_Should_Form_Contiguous_Range()
        //{
        //    var expectedIndices = Enumerable.Range(0, 120).ToList();
        //    var actualIndices = _gridGenerator.Edges.Select(edge => edge.Index).ToList();

        //    CollectionAssert.AreEquivalent(expectedIndices, actualIndices);
        //}

        [TestMethod]
        public void Every_Face_Should_Have_Three_Vertices()
        {
            var expectedVertexCount = 3;
            var actualVertexCounts = _gridGenerator.Faces.Select(face => face.Vertices.Count);

            Assert.IsTrue(actualVertexCounts.All(count => count == expectedVertexCount));
        }

        [TestMethod]
        public void Every_Face_Should_Have_Three_Edges()
        {
            var expectedEdgeCount = 3;
            var actualEdgeCounts = _gridGenerator.Faces.Select(face => face.Edges.Count).ToList();

            Assert.IsTrue(actualEdgeCounts.All(count => count == expectedEdgeCount));
        }

        [TestMethod]
        public void Should_Be_80_Faces()
        {
            var expectedNumberOfFaces = 80;
            var actualNumberOfFaces = _gridGenerator.Faces.Count;

            Assert.AreEqual(expectedNumberOfFaces, actualNumberOfFaces);
        }

        [TestMethod]
        public void Every_Edge_Should_Have_Two_Faces()
        {
            var expectedFaceCount = 2;
            var actualFaceCounts = _gridGenerator.Edges.Select(edge => edge.Faces.Count);

            Assert.IsTrue(actualFaceCounts.All(count => count == expectedFaceCount));
        }

        [TestMethod]
        public void Twelve_Vertices_Should_Have_Five_Faces_Each()
        {
            var expectedNumberOfVertices = 12;
            var actualNumberOfVertices = _gridGenerator.Vertices.Count(vertex => vertex.Faces.Count == 5);

            Assert.AreEqual(expectedNumberOfVertices, actualNumberOfVertices);
        }

        [TestMethod]
        public void Thirty_Vertices_Should_Have_Six_Faces_Each()
        {
            var expectedNumberOfVertices = 30;
            var actualNumberOfVertices = _gridGenerator.Vertices.Count(vertex => vertex.Faces.Count == 6);

            Assert.AreEqual(expectedNumberOfVertices, actualNumberOfVertices);
        }

        //[TestMethod]
        //public void quicktest()
        //{
        //    var icosahedronOptions = new Options() { Radius = 1f, Resolution = 0.5f };

        //    _gridGenerator = new IcosahedralGridGenerator(icosahedronOptions);

        //    var expectedNumberOfFaces = 320;
        //    var actualNumberOfFaces = _gridGenerator.Faces.Count;
        //    Assert.AreEqual(expectedNumberOfFaces, actualNumberOfFaces);

        //    Assert.AreEqual(120, _gridGenerator.Edges.Count);
        //    Assert.AreEqual(102, _gridGenerator.Vertices.Count);
        //    Assert.AreEqual(12, _gridGenerator.Vertices.Count(vertex => vertex.Edges.Count == 5));
        //}

        [TestMethod]
        public void Vertices_Should_Hold_80_Distinct_Faces()
        {
            var expectedNumberOfDistinctFaces = 80;
            var actualNumberOfDistinctFaces = _gridGenerator.Vertices.SelectMany(vertex => vertex.Faces).Distinct().Count();

            Assert.AreEqual(expectedNumberOfDistinctFaces, actualNumberOfDistinctFaces);
        }

        [TestMethod]
        public void Vertices_Should_Hold_120_Distinct_Edges()
        {
            var expectedNumberOfDistinctEdges = 120;
            var actualNumberOfDistinctEdges = _gridGenerator.Vertices.SelectMany(vertex => vertex.Edges).Distinct().Count();

            Assert.AreEqual(expectedNumberOfDistinctEdges, actualNumberOfDistinctEdges);
        }

        [TestMethod]
        public void Edges_Should_Hold_80_Distinct_Faces()
        {
            var expectedNumberOfDistinctFaces = 80;
            var actualNumberOfDistinctFaces = _gridGenerator.Edges.SelectMany(edge => edge.Faces).Distinct().Count();

            Assert.AreEqual(expectedNumberOfDistinctFaces, actualNumberOfDistinctFaces);
        }

        [TestMethod]
        public void Edges_Should_Hold_42_Distinct_Vertices()
        {
            var expectedNumberOfDistinctVertices = 42;
            var actualNumberOfDistinctVertices = _gridGenerator.Edges.SelectMany(edge => edge.Vertices).Distinct().Count();

            Assert.AreEqual(expectedNumberOfDistinctVertices, actualNumberOfDistinctVertices);
        }

        [TestMethod]
        public void Faces_Should_Hold_42_Distinct_Vertices()
        {
            var expectedNumberOfDistinctVertices = 42;
            var actualNumberOfDistinctVertices = _gridGenerator.Faces.SelectMany(face => face.Vertices).Distinct().Count();

            Assert.AreEqual(expectedNumberOfDistinctVertices, actualNumberOfDistinctVertices);
        }

        [TestMethod]
        public void Faces_Should_Hold_120_Distinct_Edges()
        {
            var expectedNumberOfDistinctEdges = 120;
            var actualNumberOfDistinctEdges = _gridGenerator.Faces.SelectMany(face => face.Edges).Distinct().Count();

            Assert.AreEqual(expectedNumberOfDistinctEdges, actualNumberOfDistinctEdges);
        }
    }
}