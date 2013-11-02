using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ClimateSim.Grids;
using ClimateSim.Grids.IcosahedralGrid;
using FakeItEasy;
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
            var options = A.Fake<IIcosahedralGridOptions>();
            A.CallTo(() => options.Resolution).Returns(1f);
            A.CallTo(() => options.Radius).Returns(1f);

            _gridGenerator = new IcosahedralGridGenerator(options);
        }

        [TestMethod]
        public void Specifying_Angular_Resolution_Greater_Than_1_Point_06_Should_Return_An_Icosahedron()
        {
            var icosahedronOptions = A.Fake<IIcosahedralGridOptions>();
            A.CallTo(() => icosahedronOptions.Resolution).Returns(2f);
            A.CallTo(() => icosahedronOptions.Radius).Returns(1f);

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
        public void Should_Be_60_Edges()
        {
            var expectedNumberOfEdges = 60;
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
            var expectedVerticesOnEachEdge = Enumerable.Repeat(2, 60).ToList();
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
        public void There_Should_Be_30_Vertices_With_Two_Edges()
        {
            var expectedNumberOfVertices = 30;
            var actualNumberOfVertices = _gridGenerator.Vertices.Count(vertex => vertex.Edges.Count == 2);

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

        [TestMethod]
        public void Edge_Indices_Should_Form_Contiguous_Range()
        {
            var expectedIndices = Enumerable.Range(0, 60).ToList();
            var actualIndices = _gridGenerator.Edges.Select(edge => edge.Index).ToList();

            CollectionAssert.AreEquivalent(expectedIndices, actualIndices);
        }


    }
}