using System.Collections.Generic;
using System.Linq;
using ClimateSim.Grids.IcosahedralGrid;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;

namespace ClimateSim.Tests.Grids.IcosahedralGrid
{
    [TestClass]
    public class IcosahedronTests
    {
        private Icosahedron _icosahedron;

        [TestInitialize]
        public void Create_Icosahedron()
        {
            _icosahedron = new Icosahedron();
        }

        [TestMethod]
        public void Should_Have_20_Initialized_Faces()
        {
            Assert.AreEqual(20, _icosahedron.Faces.Length);
            CollectionAssert.AllItemsAreNotNull(_icosahedron.Faces);
        }

        [TestMethod]
        public void Should_Have_30_Initialized_Edges()
        {
            Assert.AreEqual(30, _icosahedron.Edges.Length);
            CollectionAssert.AllItemsAreNotNull(_icosahedron.Edges);
        }

        [TestMethod]
        public void Should_Have_12_Initialized_Vertices()
        {
            Assert.AreEqual(12, _icosahedron.Vertices.Length);
            CollectionAssert.AllItemsAreNotNull(_icosahedron.Vertices);
        }

        [TestMethod]
        public void Every_Face_Should_Have_Three_Edges_And_Three_Vertices()
        {
            var numbersOfEdges = _icosahedron.Faces.Select(face => face.Edges.Count).ToList();
            Assert.IsTrue(numbersOfEdges.TrueForAll(count => count == 3));
            
            var numbersOfVertices = _icosahedron.Faces.Select(face => face.Vertices.Count).ToList();
            Assert.IsTrue(numbersOfVertices.TrueForAll(count => count == 3));
        }

        [TestMethod]
        public void Every_Vertex_Should_Have_Five_Edges_And_Fives_Faces()
        {
            var numbersOfEdges = _icosahedron.Vertices.Select(vertex => vertex.Edges.Count).ToList();
            Assert.IsTrue(numbersOfEdges.TrueForAll(count => count == 5));

            var numbersOfFaces = _icosahedron.Vertices.Select(vertex => vertex.Faces.Count).ToList();
            Assert.IsTrue(numbersOfFaces.TrueForAll(count => count == 5));
        }

        [TestMethod]
        public void Every_Edge_Should_Have_Two_Faces_And_Two_Vertices()
        {
            var numbersOfFaces = _icosahedron.Edges.Select(edge => edge.Faces.Count).ToList();
            Assert.IsTrue(numbersOfFaces.TrueForAll(count => count == 2));

            var numbersOfVertices = _icosahedron.Edges.Select(edge => edge.Vertices.Count).ToList();
            Assert.IsTrue(numbersOfVertices.TrueForAll(count => count == 2));
        }

        [TestMethod]
        public void Should_Have_A_North_Pole()
        {
            var expectedPosition = new Vector3(0, 0, 1);
            var actualPositions = _icosahedron.Vertices.Select(vertex => vertex.Position).ToList();

            CollectionAssert.Contains(actualPositions, expectedPosition);
        }

        [TestMethod]
        public void Every_Edge_Should_Have_A_Unique_Index()
        {
            var indices = _icosahedron.Edges.Select(edge => edge.Index).ToList();
            CollectionAssert.DoesNotContain(indices, -1); // Default value is -1.
            CollectionAssert.AllItemsAreUnique(indices);
        }


        [TestMethod]
        public void Every_Vertex_Should_Have_A_Unique_Index()
        {
            var indices = _icosahedron.Vertices.Select(face => face.Index).ToList();
            CollectionAssert.DoesNotContain(indices, -1); // Default value is -1.
            CollectionAssert.AllItemsAreUnique(indices);
        }


        [TestMethod]
        public void Every_Face_Should_Have_A_Unique_Index()
        {
            var indices = _icosahedron.Faces.Select(face => face.Index).ToList();
            CollectionAssert.DoesNotContain(indices, -1); // Default value is -1.
            CollectionAssert.AllItemsAreUnique(indices);
        }

        [TestMethod]
        public void Midlatitude_Face_On_The_Prime_Meridian_Should_Have_The_Correct_Vertices()
        {
            var face = _icosahedron.Faces[14];
            var expectedVertexIndices = new List<int> { 1, 6, 10 };
            var actualVertexIndices = face.Vertices.Select(vertex => vertex.Index).ToList();
            CollectionAssert.AreEquivalent(expectedVertexIndices, actualVertexIndices);
        }

        [TestMethod]
        public void Midlatitude_Face_On_The_Prime_Meridian_Should_Have_The_Correct_Edges()
        {
            var face = _icosahedron.Faces[14];
            var expectedEdgeIndices = new List<int> { 10, 19, 24 };
            var actualEdgeIndices = face.Edges.Select(vertex => vertex.Index).ToList();
            CollectionAssert.AreEquivalent(expectedEdgeIndices, actualEdgeIndices);
        }

        [TestMethod]
        public void Southern_Vertex_At_180E_Should_Have_The_Correct_Faces()
        {
            var vertex = _icosahedron.Vertices[8];
            var expectedFaceIndices = new List<int> { 8, 9, 10, 16, 17 };
            var actualFaceIndices = vertex.Faces.Select(face => face.Index).ToList();
            CollectionAssert.AreEquivalent(expectedFaceIndices, actualFaceIndices);
        }

        [TestMethod]
        public void Southern_Vertex_At_180E_Should_Have_The_Correct_Edges()
        {
            var vertex = _icosahedron.Vertices[8];
            var expectedEdgeIndices = new List<int> { 14, 15, 21, 22, 27 };
            var actualEdgeIndices = vertex.Edges.Select(edge => edge.Index).ToList();
            CollectionAssert.AreEquivalent(expectedEdgeIndices, actualEdgeIndices);
        }

        [TestMethod]
        public void Fourth_Edge_Around_On_Upper_Latitude_Should_Have_The_Correct_Faces()
        {
            var edge = _icosahedron.Edges[8];
            var expectedFaceIndices = new List<int> { 3, 11 };
            var actualFaceIndices = edge.Faces.Select(face => face.Index).ToList();
            CollectionAssert.AreEquivalent(expectedFaceIndices, actualFaceIndices);
        }

        [TestMethod]
        public void Fourth_Edge_Around_On_Upper_Latitude_Should_Have_The_Correct_Vertices()
        {
            var edge = _icosahedron.Edges[8];
            var expectedVertexIndices = new List<int> { 4, 5 };
            var actualVertexIndices = edge.Vertices.Select(vertex => vertex.Index).ToList();
            CollectionAssert.AreEquivalent(expectedVertexIndices, actualVertexIndices);
        }
    }
}