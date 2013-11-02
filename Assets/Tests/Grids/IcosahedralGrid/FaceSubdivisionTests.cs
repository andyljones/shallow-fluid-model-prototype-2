using System.Collections.Generic;
using System.Linq;
using ClimateSim.Grids;
using ClimateSim.Grids.IcosahedralGrid;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;

namespace ClimateSim.Tests.Grids.IcosahedralGrid
{
    [TestClass]
    public class FaceSubdivisionTests
    {
        private FaceSubdivision _subdivision;
        private IcosahedralFace _face;

        [TestInitialize]
        public void Create_North_Pointing_Face_Subdivision()
        {
            var northVertex = new Vertex(1) {Position = new Vector3(0, 1, 1)};
            var southEastVertex = new Vertex(2) {Position = new Vector3(1, 1, -1)};
            var southWestVertex = new Vertex(3) {Position = new Vector3(-1, 1, -1)};

            var eastEdge = new Edge(10) {Vertices = new List<Vertex> {northVertex, southEastVertex}};
            var westEdge = new Edge(11) {Vertices = new List<Vertex> {northVertex, southWestVertex}};
            var southEdge = new Edge(12) {Vertices = new List<Vertex> {southWestVertex, southEastVertex}};
            var extraneousEdge = new Edge(25);

            _face = new IcosahedralFace
            {
                Vertices = new List<Vertex> {southWestVertex, northVertex, southEastVertex},
                Edges = new List<Edge> {eastEdge, westEdge, southEdge}
            };

            var extraneousFace = new IcosahedralFace {BlockIndex = 7, IndexInBlock = 14};

            northVertex.Edges = new List<Edge> {eastEdge, westEdge, extraneousEdge};
            northVertex.Faces = new List<IcosahedralFace> {_face, extraneousFace};

            _subdivision = new FaceSubdivision(_face);
        }

        [TestMethod]
        public void Northernmost_Vertex_Should_Be_Stripped_Of_Old_Faces_Edges()
        {
            var expectedEdgeIndices = new List<int> {25};
            var actualEdgeIndices = _subdivision.Vertices[0].Edges.Select(edge => edge.Index).ToList();

            CollectionAssert.AreEquivalent(expectedEdgeIndices, actualEdgeIndices);
        }

        [TestMethod]
        public void Northernmost_Vertex_Should_Be_Stripped_Of_Old_Face()
        {
            var expectedFaceBlockIndices = new List<int> {7};
            var actualFaceBlockIndices = _subdivision.Vertices[0].Faces.Select(face => face.BlockIndex).ToList();

            CollectionAssert.AreEquivalent(expectedFaceBlockIndices, actualFaceBlockIndices);

            var expectedFaceIndicesWithinBlock = new List<int> {14};
            var actualFaceIndicesWithinBlock = _subdivision.Vertices[0].Faces.Select(face => face.IndexInBlock).ToList();

            CollectionAssert.AreEquivalent(expectedFaceIndicesWithinBlock, actualFaceIndicesWithinBlock);
        }

        [TestMethod]
        public void Northernmost_Vertex_Should_Have_Its_Index_Doubled()
        {
            var expectedIndex = 2;
            var actualIndex = _subdivision.Vertices[0].Index;

            Assert.AreEqual(expectedIndex, actualIndex);
        }

        [TestMethod]
        public void Mid_Point_Vertices_Should_Have_The_Correct_Positions()
        {
            var expectedVertexPositions = new List<Vector3>
            {
                new Vector3(0, 1, 1), // North vertex
                new Vector3(1, 1, -1), // Southeast vertex
                new Vector3(-1, 1, -1), // Southwest vertex
                new Vector3(0.5f, 1.0f, 0.0f), // North-to-southeast vertex
                new Vector3(0.0f, 1.0f, -1.0f), // Southeast-to-southwest vertex
                new Vector3(-0.5f, 1.0f, 0.0f) // Southwest-to-north vertex
            };

            var actualVertexPositions = _subdivision.Vertices.Select(vertex => vertex.Position).ToList();

            CollectionAssert.AreEquivalent(expectedVertexPositions, actualVertexPositions);
        }
    }
}