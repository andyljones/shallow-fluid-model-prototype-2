using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClimateSim.Grids.IcosahedralGrid
{
    public class IcosahedralGridGenerator
    {
        public List<IcosahedralFace> Faces { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Vertices { get; private set; }

        private float _targetAngularResolution;
        private float _currentAngularResolution;


        public IcosahedralGridGenerator(IIcosahedralGridOptions options)
        {
            _targetAngularResolution = options.Resolution / options.Radius;

            CreateIcosahedron();

            while (_currentAngularResolution > _targetAngularResolution)
            {
                SubdivideEdges();
                SubdivideFaces();
                _currentAngularResolution /= 2;
            }
        }

        private void CreateIcosahedron()
        {
            var icosahedron = new Icosahedron();
            _currentAngularResolution = 1/Mathf.Sin(2*Mathf.PI/5);

            Faces = icosahedron.Faces;
            Edges = icosahedron.Edges;
            Vertices = icosahedron.Vertices;
        }

        private void SubdivideEdges()
        {
            var newEdges = new List<Edge>();

            foreach (var edge in Edges)
            {
                newEdges.AddRange(SubdivideEdge(edge));
            }

            Edges = newEdges;
        }

        private IEnumerable<Edge> SubdivideEdge(Edge edge)
        {
            var endpoint0 = edge.Vertices[0];
            var endpoint1 = edge.Vertices[1];

            var midpoint = new Vertex(12+edge.Index) {Position = (endpoint0.Position + endpoint1.Position).normalized};

            var newEdge0 = new Edge(2*edge.Index+0) { Vertices = new List<Vertex> { endpoint0, midpoint } };
            var newEdge1 = new Edge(2*edge.Index+1) { Vertices = new List<Vertex> { endpoint1, midpoint } };

            endpoint0.Edges.Remove(edge);
            endpoint0.Edges.Add(newEdge0);

            endpoint1.Edges.Remove(edge);
            endpoint1.Edges.Add(newEdge1);

            midpoint.Edges.Add(newEdge0);
            midpoint.Edges.Add(newEdge1);
            Vertices.Add(midpoint);

            return new List<Edge> {newEdge0, newEdge1};
        }

        private void SubdivideFaces()
        {
            var newFaces = new List<IcosahedralFace>();

            foreach (var face in Faces)
            {
                newFaces.AddRange(SubdivideFace(face));
            }
        }

        private IEnumerable<IcosahedralFace> SubdivideFace(IcosahedralFace face)
        {
            
        }
    }
}
