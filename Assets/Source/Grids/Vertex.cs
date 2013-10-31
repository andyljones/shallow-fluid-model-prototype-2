using System.Collections.Generic;
using UnityEngine;

namespace ClimateSim.Grids
{
    public class Vertex
    {
        public List<Face> Faces;

        public List<Edge> Edges;

        public Vector3 Position;
    }
}
