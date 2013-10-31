using System;
using System.Collections.Generic;
using System.Linq;

namespace ClimateSim.Grids.IcosahedralGrid
{
    public class FaceSubdivision
    {
        public List<Face> Faces { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Vertices { get; private set; }

        public FaceSubdivision(Face face)
        {
            Vertices = GenerateVertices(face);
        }

        private List<Vertex> GenerateVertices(Face face)
        {
            var vertex1 = SubdivideEdge(face.Edges[0]);
            var vertex2 = SubdivideEdge(face.Edges[1]);
            var vertex3 = SubdivideEdge(face.Edges[2]);
        }

        private Vertex SubdivideEdge(Edge edge)
        {
            var midVertexPosition = (edge.Vertices[0].Position + edge.Vertices[1].Position)/2;

            return new Vertex {Position = midVertexPosition.normalized};
        }
    }
}