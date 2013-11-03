using System.Collections.Generic;
using UnityEngine;

namespace ClimateSim.Grids
{
    public class Vertex
    {
        public List<Face> Faces = new List<Face>();

        public List<Edge> Edges = new List<Edge>();

        public Vector3 Position = new Vector3(0, 0, 0);
    }
}
