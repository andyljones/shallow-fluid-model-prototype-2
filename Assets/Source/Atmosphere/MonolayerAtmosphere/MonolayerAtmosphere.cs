using System.Collections.Generic;
using System.Linq;
using Foam;
using Surfaces;

namespace Atmosphere.MonolayerAtmosphere
{
    //TODO: Refactor.
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
            SetHeightOfTopVertices(height);

            foreach (var vertexPair in _lowerToUpperVertexDictionary)
            {
                CreateEdge(vertexPair);
            }

            foreach (var edgePair in _lowerToUpperEdgeDictionary)
            {
                CreateFace(edgePair);
            }

            Cells = new List<Cell>();

            foreach (var facePair in _lowerToUpperFaceDictionary)
            {
                CreateCell(facePair);
            }
        }

        private void CreateFace(KeyValuePair<Edge, Edge> edgePair)
        {
            var newFace = new Face();

            var lowerEdge = edgePair.Key;
            var upperEdge = edgePair.Value;

            var vertices = lowerEdge.Vertices.Concat(upperEdge.Vertices);

            foreach (var vertex in vertices)
            {
                vertex.Faces.Add(newFace);
                newFace.Vertices.Add(vertex);
            }

            var edgesNeighbouringLowerEdge = lowerEdge.Vertices.SelectMany(vertex => vertex.Edges);
            var edgesNeighbouringUpperEdge = upperEdge.Vertices.SelectMany(vertex => vertex.Edges);
            var edges = edgesNeighbouringLowerEdge.Intersect(edgesNeighbouringUpperEdge).ToList();

            edges.Add(lowerEdge);
            edges.Add(upperEdge);

            foreach (var edge in edges)
            {
                edge.Faces.Add(newFace);
                newFace.Edges.Add(edge);
            }
        }

        private void CreateEdge(KeyValuePair<Vertex, Vertex> vertexPair)
        {
            var lowerVertex = vertexPair.Key;
            var upperVertex = vertexPair.Value;

            var newEdge = new Edge {Vertices = new List<Vertex> {lowerVertex, upperVertex}};
            lowerVertex.Edges.Add(newEdge);
            upperVertex.Edges.Add(newEdge);
        }

        private void CreateCell(KeyValuePair<Face, Face> facePair)
        {
            var newCell = new Cell();
            
            var bottomFace = facePair.Key;
            var topFace = facePair.Value;
            
            AddVerticesToCell(bottomFace, topFace, newCell);
            AddEdgesToCell(bottomFace, topFace, newCell);
            AddFacesToCell(bottomFace, topFace, newCell);

            Cells.Add(newCell);
        }

        private void AddFacesToCell(Face bottomFace, Face topFace, Cell newCell)
        {
            var faces = GetFacesBetween(bottomFace, topFace);
            faces.Add(bottomFace);
            faces.Add(topFace);
            foreach (var face in faces)
            {
                face.Cells.Add(newCell);
                newCell.Faces.Add(face);
            }
        }

        private List<Face> GetFacesBetween(Face bottomFace, Face topFace)
        {
            var facesNeighbouringBottomFace = bottomFace.Edges.SelectMany(edge => edge.Faces);
            var facesNeighbouringTopFace = topFace.Edges.SelectMany(edge => edge.Faces);

            return facesNeighbouringBottomFace.Intersect(facesNeighbouringTopFace).ToList();
        }

        private void AddEdgesToCell(Face bottomFace, Face topFace, Cell newCell)
        {
            var bottomEdges = bottomFace.Edges;
            var topEdges = topFace.Edges;
            var verticalEdges = GetEdgesBetween(bottomFace, topFace);
            var edges = bottomEdges.Concat(topEdges).Concat(verticalEdges);
            foreach (var edge in edges)
            {
                edge.Cells.Add(newCell);
                newCell.Edges.Add(edge);
            }
        }

        private IEnumerable<Edge> GetEdgesBetween(Face bottomFace, Face topFace)
        {
            var edgesNeighbouringBottomFace = bottomFace.Vertices.SelectMany(vertex => vertex.Edges);
            var edgesNeighbouringTopFace = topFace.Vertices.SelectMany(vertex => vertex.Edges);

            return edgesNeighbouringBottomFace.Intersect(edgesNeighbouringTopFace);
        }

        private void AddVerticesToCell(Face bottomFace, Face topFace, Cell newCell)
        {
            var bottomVertices = bottomFace.Vertices;
            var topVertices = topFace.Vertices;
            var vertices = bottomVertices.Concat(topVertices).ToList();
            foreach (var vertex in vertices)
            {
                vertex.Cells.Add(newCell);
                newCell.Vertices.Add(vertex);
            }
        }

        private void SetHeightOfTopVertices(float height)
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
