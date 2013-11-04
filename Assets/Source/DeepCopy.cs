    using System.Collections.Generic;
    using System.Linq;
    using ClimateSim.Grids;

namespace ClimateSim
{
    public static class DeepCopyMethods
    {
        public static List<Face> DeepCopy(this List<Face> oldFaces)
        {
            var oldVertices = oldFaces.SelectMany(face => face.Vertices).Distinct();
            var oldEdges = oldFaces.SelectMany(face => face.Edges).Distinct();

            var vertexDictionary = oldVertices.ToDictionary(oldVertex => oldVertex, oldVertex => new Vertex { Position = oldVertex.Position });
            var edgeDictionary = oldEdges.ToDictionary(oldEdge => oldEdge, oldEdge => new Edge());
            var faceDictionary = oldFaces.ToDictionary(oldFace => oldFace, oldFace => new Face());

            LinkVerticesAndEdges(vertexDictionary, edgeDictionary);
            LinkEdgesAndFaces(edgeDictionary, faceDictionary);
            LinkFacesAndVertices(faceDictionary, vertexDictionary);

            return faceDictionary.Values.ToList();
        }

        private static void LinkVerticesAndEdges(Dictionary<Vertex, Vertex> vertexDictionary, Dictionary<Edge, Edge> edgeDictionary)
        {
            foreach (var edgePair in edgeDictionary)
            {
                var oldEdge = edgePair.Key;
                var newEdge = edgePair.Value;

                foreach (var oldVertex in oldEdge.Vertices)
                {
                    var newVertex = vertexDictionary[oldVertex];

                    newEdge.Vertices.Add(newVertex);
                    newVertex.Edges.Add(newEdge);
                }
            }
        }

        private static void LinkEdgesAndFaces(Dictionary<Edge, Edge> edgeDictionary, Dictionary<Face, Face> faceDictionary)
        {
            foreach (var facePair in faceDictionary)
            {
                var oldFace = facePair.Key;
                var newFace = facePair.Value;

                foreach (var oldEdge in oldFace.Edges)
                {
                    var newEdge = edgeDictionary[oldEdge];

                    newFace.Edges.Add(newEdge);
                    newEdge.Faces.Add(newFace);
                }
            }
        }

        private static void LinkFacesAndVertices(Dictionary<Face, Face> faceDictionary, Dictionary<Vertex, Vertex> vertexDictionary)
        {
            foreach (var vertexPair in vertexDictionary)
            {
                var oldVertex = vertexPair.Key;
                var newVertex = vertexPair.Value;

                foreach (var oldFace in oldVertex.Faces)
                {
                    var newFace = faceDictionary[oldFace];

                    newVertex.Faces.Add(newFace);
                    newFace.Vertices.Add(newVertex);
                }
            }
        }
    }
}
