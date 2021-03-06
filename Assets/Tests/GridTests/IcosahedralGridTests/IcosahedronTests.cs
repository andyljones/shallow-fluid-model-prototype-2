﻿using System.Collections.Generic;
using System.Linq;
using Foam;
using Grids.IcosahedralGridGenerator;
using NUnit.Framework;
using UnityEngine;

namespace Tests.GridTests.IcosahedralGridTests
{
    [TestFixture]
    public class IcosahedronTests
    {
        private Icosahedron _icosahedron;

        [SetUp]
        public void Create_Icosahedron()
        {
            _icosahedron = new Icosahedron();
        }

        [Test]
        public void Should_Have_20_Initialized_Faces()
        {
            Assert.AreEqual(20, _icosahedron.Faces.Count);
            CollectionAssert.AllItemsAreNotNull(_icosahedron.Faces);
        }

        [Test]
        public void Should_Have_30_Initialized_Edges()
        {
            Assert.AreEqual(30, _icosahedron.Edges.Count);
            CollectionAssert.AllItemsAreNotNull(_icosahedron.Edges);
        }

        [Test]
        public void Should_Have_12_Initialized_Vertices()
        {
            Assert.AreEqual(12, _icosahedron.Vertices.Count);
            CollectionAssert.AllItemsAreNotNull(_icosahedron.Vertices);
        }

        [Test]
        public void Every_Face_Should_Have_Three_Edges_And_Three_Vertices()
        {
            var numbersOfEdges = _icosahedron.Faces.Select(face => face.Edges.Count).ToList();
            Assert.IsTrue(numbersOfEdges.TrueForAll(count => count == 3));
            
            var numbersOfVertices = _icosahedron.Faces.Select(face => face.Vertices.Count).ToList();
            Assert.IsTrue(numbersOfVertices.TrueForAll(count => count == 3));
        }

        [Test]
        public void Every_Vertex_Should_Have_Five_Edges_And_Fives_Faces()
        {
            var numbersOfEdges = _icosahedron.Vertices.Select(vertex => vertex.Edges.Count).ToList();
            Assert.IsTrue(numbersOfEdges.TrueForAll(count => count == 5));

            var numbersOfFaces = _icosahedron.Vertices.Select(vertex => vertex.Faces.Count).ToList();
            Assert.IsTrue(numbersOfFaces.TrueForAll(count => count == 5));
        }

        [Test]
        public void Every_Edge_Should_Have_Two_Faces_And_Two_Vertices()
        {
            var numbersOfFaces = _icosahedron.Edges.Select(edge => edge.Faces.Count).ToList();
            Assert.IsTrue(numbersOfFaces.TrueForAll(count => count == 2));

            var numbersOfVertices = _icosahedron.Edges.Select(edge => edge.Vertices.Count).ToList();
            Assert.IsTrue(numbersOfVertices.TrueForAll(count => count == 2));
        }

        [Test]
        public void Should_Have_A_North_Pole()
        {
            var expectedPosition = new Vector3(0, 0, 1);
            var actualPositions = _icosahedron.Vertices.Select(vertex => vertex.Position).ToList();

            CollectionAssert.Contains(actualPositions, expectedPosition);
        }

        [Test]
        public void Midlatitude_Face_On_The_Prime_Meridian_Should_Have_The_Correct_Vertices()
        {
            var face = _icosahedron.Faces[14];
            var expectedVertices = new List<Vertex> { _icosahedron.Vertices[1], _icosahedron.Vertices[6], _icosahedron.Vertices[10] };
            var actualVertices = face.Vertices;
            CollectionAssert.AreEquivalent(expectedVertices, actualVertices);
        }

        [Test]
        public void Midlatitude_Face_On_The_Prime_Meridian_Should_Have_The_Correct_Edges()
        {
            var face = _icosahedron.Faces[14];
            var expectedEdges = new List<Edge> { _icosahedron.Edges[10], _icosahedron.Edges[19], _icosahedron.Edges[24] };
            var actualEdges = face.Edges;
            CollectionAssert.AreEquivalent(expectedEdges, actualEdges);
        }

        //TODO: Update so it doesn't use indices.
        //[Test]
        //public void Southern_Vertex_At_180E_Should_Have_The_Correct_Faces()
        //{
        //    var vertices = _icosahedron.Vertices[8];
        //    var expectedBlockIndices = new List<int> { 2, 2, 2, 3, 3 };
        //    var actualBlockIndices = vertices.Faces.Select(face => face.BlockIndex).ToList();
        //    CollectionAssert.AreEquivalent(expectedBlockIndices, actualBlockIndices);

        //    var expectedIndicesWithinBlock2 = new List<int> {1, 2, 3};
        //    var actualIndicesWithinBlock2 = vertices.Faces.Where(face => face.BlockIndex == 2).Select(face => face.IndexInBlock).ToList();
        //    CollectionAssert.AreEquivalent(expectedIndicesWithinBlock2, actualIndicesWithinBlock2);

        //    var expectedIndicesWithinBlock3 = new List<int> { 2, 3 };
        //    var actualIndicesWithinBlock3 = vertices.Faces.Where(face => face.BlockIndex == 3).Select(face => face.IndexInBlock).ToList();
        //    CollectionAssert.AreEquivalent(expectedIndicesWithinBlock3, actualIndicesWithinBlock3);
        //}

        [Test]
        public void Southern_Vertex_At_180E_Should_Have_The_Correct_Edges()
        {
            var vertex = _icosahedron.Vertices[8];
            var expectedEdges = new List<Edge>
            {
                _icosahedron.Edges[14],
                _icosahedron.Edges[15],
                _icosahedron.Edges[21],
                _icosahedron.Edges[22],
                _icosahedron.Edges[27]
            };
            var actualEdges = vertex.Edges;
            CollectionAssert.AreEquivalent(expectedEdges, actualEdges);
        }

        //TODO: Update so it doesn't use indices.
        //[Test]
        //public void Fourth_Edge_Around_On_Upper_Latitude_Should_Have_The_Correct_Faces()
        //{
        //    var edge = _icosahedron.Edges[8];
        //    var expectedBlockIndices = new List<int> { 3, 3 };
        //    var actualBlockIndices = edge.Faces.Select(face => face.BlockIndex).ToList();
        //    CollectionAssert.AreEquivalent(expectedBlockIndices, actualBlockIndices);

        //    var expectedIndicesWithinBlock3 = new List<int> { 0, 1 };
        //    var actualIndicesWithinBlock3 = edge.Faces.Where(face => face.BlockIndex == 3).Select(face => face.IndexInBlock).ToList();
        //    CollectionAssert.AreEquivalent(expectedIndicesWithinBlock3, actualIndicesWithinBlock3);
        //}

        [Test]
        public void Fourth_Edge_Around_On_Upper_Latitude_Should_Have_The_Correct_Vertices()
        {
            var edge = _icosahedron.Edges[8];
            var expectedVertices = new List<Vertex> { _icosahedron.Vertices[4], _icosahedron.Vertices[5] };
            var actualVertices = edge.Vertices;
            CollectionAssert.AreEquivalent(expectedVertices, actualVertices);
        }

        [Test]
        public void Every_Vertex_Should_Be_Of_Unit_Distance_From_The_Origin()
        {
            var actualVertexDistances = _icosahedron.Vertices.Select(vertex => vertex.Position.magnitude);
            var expectedVertexDistance = 1;

            Assert.IsTrue(actualVertexDistances.All(distance => Mathf.Abs(distance - expectedVertexDistance) < 0.01f));
        }

        [Test]
        public void Every_Edge_Should_Have_Roughly_The_Same_Length()
        {
            var expectedEdgeLength = 1/Mathf.Sin(2*Mathf.PI/5);
            var actualEdgeLengths =
                _icosahedron.Edges.Select(edge => (edge.Vertices[0].Position - edge.Vertices[1].Position).magnitude).ToList();

            Assert.IsTrue(actualEdgeLengths.All(length => Mathf.Abs(length - expectedEdgeLength) < 0.01f));
        }
    }
}