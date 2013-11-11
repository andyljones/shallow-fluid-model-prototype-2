using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class BoundaryRouteFinder
    {
        private HashSet<Edge> _validEdges;
        private HashSet<Edge> _unvisitedEdges;

        public BoundaryRouteFinder(List<Edge> validEdges)
        {
            _validEdges = new HashSet<Edge>(validEdges);
            _unvisitedEdges = new HashSet<Edge>(validEdges);
        }

        public List<Vertex> GetRoute()
        {
            var currentEdge = _unvisitedEdges.First();
            var currentVertex = currentEdge.Vertices.First();
            var path = new List<Vertex> { currentVertex };

            while (currentEdge != default(Edge))
            {
                currentVertex = currentEdge.OtherEndpointFrom(currentVertex);

                path.Add(currentVertex);
                _unvisitedEdges.Remove(currentEdge);

                var liveEdges = LiveEdgesOf(currentVertex);
                var notDeadends = liveEdges.Where(edge => !IsDeadend(edge.OtherEndpointFrom(currentVertex))).ToList();

                if (notDeadends.Any())
                {
                    currentEdge = notDeadends.First();
                }
                else
                {
                    currentEdge = liveEdges.FirstOrDefault();
                }
            }

            return path;
        }

        private bool IsDeadend(Vertex vertex)
        {
            return LiveEdgesOf(vertex).Count < 2;
        }

        private List<Edge> LiveEdgesOf(Vertex currentVertex)
        {
            return currentVertex.Edges.Where(edge => _validEdges.Contains(edge) && _unvisitedEdges.Contains(edge)).ToList();
        }

        public bool AllEdgesVisited()
        {
            return _unvisitedEdges.Count == 0;
        }
    }
}
