using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Foam;
using Surfaces;

namespace Atmosphere.MonolayerAtmosphere
{
    public class MonolayerAtmosphere : IAtmosphere
    {
        public List<Cell> Cells { get; private set; }

        private Dictionary<Vertex, Vertex> _lowerToUpperVertexDictionary;
        private Dictionary<Edge, Edge> _lowerToUpperEdgeDictionary;
        private Dictionary<Face, Face> _lowerToUpperFaceDictionary;

        public MonolayerAtmosphere(ISurface surface, IMonolayerAtmosphereOptions options)
        {
            var surfaceCopier = new FoamCopier(surface.Faces);

            var lowerAtmosphereFaces = surfaceCopier.FaceDictionary.Values.ToList();
            var atmosphereCopier = new FoamCopier(lowerAtmosphereFaces);

            _lowerToUpperVertexDictionary = atmosphereCopier.VertexDictionary;
            _lowerToUpperEdgeDictionary = atmosphereCopier.EdgeDictionary;
            _lowerToUpperFaceDictionary = atmosphereCopier.FaceDictionary;

            CreateUpperAtmosphere(options.Height);
        }

        private void CreateUpperAtmosphere(float height)
        {
            Cells = new List<Cell>();

            foreach (var facePair in _lowerToUpperFaceDictionary)
            {
                var newEdges = CreateVerticalEdges(facePair); //TODO: write tests for this
                var newCell = CreateCell(facePair, newEdges);
                Cells.Add(newCell);
            }

            SetHeightOfTopFaces(height);
        }

        private List<Edge> CreateVerticalEdges(KeyValuePair<Face, Face> facePair)
        {
            var edges = new List<Edge>();

            var lowerFace = facePair.Key;

            foreach (var lowerVertex in lowerFace.Vertices)
            {
                var upperVertex = _lowerToUpperVertexDictionary[lowerVertex];
                var newEdge = new Edge {Vertices = new List<Vertex> {lowerVertex, upperVertex}};
                
                lowerVertex.Edges.Add(newEdge);
                upperVertex.Edges.Add(newEdge);

                edges.Add(newEdge);
            }

            return edges;
        }

        private Cell CreateCell(KeyValuePair<Face, Face> facePair, List<Edge> verticalEdges)
        {
            var newCell = new Cell();

            //TODO: Add cell to facess
            var bottomFace = facePair.Key;
            var topFace = facePair.Value;

            var faces = new List<Face> {bottomFace, topFace};
            bottomFace.Cells.Add(newCell);
            topFace.Cells.Add(newCell);

            var bottomEdges = bottomFace.Edges;
            var topEdges = topFace.Edges;
            var edges = bottomEdges.Concat(topEdges).Concat(verticalEdges).ToList();

            var bottomVertices = bottomFace.Vertices;
            var topVertices = topFace.Vertices;
            var vertices = bottomVertices.Concat(topVertices).ToList();

            return new Cell {Faces = faces, Edges = edges, Vertices = vertices};
        }

        private void SetHeightOfTopFaces(float height)
        {
            var vertices = _lowerToUpperVertexDictionary.Values.ToList();

            foreach (var vertex in vertices)
            {
                var direction = vertex.Position.normalized;
                var radius = vertex.Position.magnitude;

                var newPosition = (radius + height) * direction;

                vertex.Position = newPosition;
            }
        }
    }
}
