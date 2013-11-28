using System.Collections.Generic;
using System.Linq;
using Foam;
using NUnit.Framework;
using Tests.Fakes;
using Tests.GridTests;

namespace Tests
{
    // TODO: Update tests to reflect new FoamCopier
    [TestFixture]
    public class DeepCopyTests
    {
        private List<Face> _oldFaces;
        private List<Face> _newFaces;
            
        [SetUp]
        public void Create_And_Copy_A_List_Of_Faces()
        {
            _oldFaces = new FakeGrid().Faces;

            _newFaces = new FoamCopier(_oldFaces).FaceDictionary.Values.ToList();
        }

        [Test]
        public void New_Faces_Should_Be_Different_Objects_From_Old_Faces()
        {
            var allFaces = _oldFaces.Union(_newFaces);
         
            var expectedNumberOfUniqueFaceObjects = 4;
            var actualNumberOfUniqueFaceObjects = allFaces.Count();

            Assert.AreEqual(expectedNumberOfUniqueFaceObjects, actualNumberOfUniqueFaceObjects);
        }

        [Test]
        public void New_Edges_Should_Be_Different_Objects_From_Old_Edges()
        {
            var oldEdges = _oldFaces.SelectMany(face => face.Edges);
            var newEdges = _newFaces.SelectMany(face => face.Edges);

            var allEdges = oldEdges.Union(newEdges);

            var expectedNumberOfUniqueEdgeObjects = 10;
            var actualNumberOfUniqueEdgeObjects = allEdges.Count();

            Assert.AreEqual(expectedNumberOfUniqueEdgeObjects, actualNumberOfUniqueEdgeObjects);
        }

        [Test]
        public void New_Vertices_Should_Be_Different_Objects_From_Old_Edges()
        {
            var oldVertices = _oldFaces.SelectMany(face => face.Vertices);
            var newVertices = _newFaces.SelectMany(face => face.Vertices);

            var allVertices = oldVertices.Union(newVertices);

            var expectedNumberOfUniqueVertexObjects = 8;
            var actualNumberOfUniqueVertexObjects = allVertices.Count();

            Assert.AreEqual(expectedNumberOfUniqueVertexObjects, actualNumberOfUniqueVertexObjects);
        }

        [Test]
        public void Face_References_From_New_Vertices_Should_Be_New_Faces()
        {
            var newVertices = _newFaces.SelectMany(face => face.Vertices);
            var referencedFaces = newVertices.SelectMany(vertex => vertex.Faces);
            
            var allFaces = _oldFaces.Union(referencedFaces);

            var expectedNumberOfUniqueFaces = 4;
            var actualNumberOfUniqueFaces = allFaces.Count();
            Assert.AreEqual(expectedNumberOfUniqueFaces, actualNumberOfUniqueFaces);
        }

        [Test]
        public void Face_References_From_New_Edges_Should_Be_New_Faces()
        {
            var newFaces = _newFaces.SelectMany(face => face.Edges);
            var referencedFaces = newFaces.SelectMany(vertex => vertex.Faces);

            var allFaces = _oldFaces.Union(referencedFaces);

            var expectedNumberOfUniqueFaces = 4;
            var actualNumberOfUniqueFaces = allFaces.Count();
            Assert.AreEqual(expectedNumberOfUniqueFaces, actualNumberOfUniqueFaces);
        }

        [Test]
        public void Every_New_Edge_Should_Have_Two_Vertices()
        {
            var newEdges = _newFaces.SelectMany(face => face.Edges).Distinct();
            var actualVertexCounts = newEdges.Select(edge => edge.Vertices.Count).ToList();

            var expectedVertexCounts = new[] {2, 2, 2, 2, 2};

            CollectionAssert.AreEquivalent(expectedVertexCounts, actualVertexCounts);
        }

        [Test]
        public void New_Vertices_Should_Have_The_Correct_Number_Of_Edges()
        {
            var newVertices = _newFaces.SelectMany(face => face.Vertices).Distinct();
            var actualEdgeCounts = newVertices.Select(vertex => vertex.Edges.Count).ToList();

            var expectedEdgeCounts = new[] {2, 2, 3, 3};

            CollectionAssert.AreEquivalent(expectedEdgeCounts, actualEdgeCounts);
        }
    }
}
