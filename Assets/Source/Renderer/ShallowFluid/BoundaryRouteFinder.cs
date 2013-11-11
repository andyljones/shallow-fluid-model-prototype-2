using System.Collections.Generic;
using System.Linq;
using Foam;

namespace Renderer.ShallowFluid
{
    public class BoundaryRouteFinder
    {
        private List<Edge> _validEdges;
        private Dictionary<Edge, bool> _visitedEdges;

        public BoundaryRouteFinder(List<Edge> validEdges)
        {
            _validEdges = validEdges;
            _visitedEdges = _validEdges.ToDictionary(edge => edge, edge => false);
        }

        public List<Vertex> GetRoute()
        {
            var possibleStarts = _validEdges.Where(edge => !_visitedEdges[edge]).ToList();
            var currentEdge = possibleStarts[possibleStarts.Count()/2];
            var currentVertex = currentEdge.Vertices.First();
            var path = new List<Vertex> {currentVertex};

            while (currentEdge != default(Edge))
            {
                currentVertex = currentEdge.OtherEndpointFrom(currentVertex);

                path.Add(currentVertex);
                _visitedEdges[currentEdge] = true;

                var liveEdges = LiveEdgesOf(currentVertex);
                var notDeadends = liveEdges.Where(edge => !IsDeadend(edge.OtherEndpointFrom(currentVertex)));

                currentEdge = notDeadends.FirstOrDefault();
            }

            return path;
        }

        private bool IsDeadend(Vertex vertex)
        {
            return LiveEdgesOf(vertex).Count < 2;
        }

        private List<Edge> LiveEdgesOf(Vertex currentVertex)
        {
            var validEdges = currentVertex.Edges.Intersect(_validEdges);
            var liveEdges = validEdges.Where(edge => !_visitedEdges[edge]);

            return liveEdges.ToList();
        }

        public bool AllEdgesVisited()
        {
            return !_visitedEdges.ContainsValue(false);
        }
    }
}
