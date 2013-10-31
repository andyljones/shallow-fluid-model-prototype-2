using System.Collections.Generic;
using UnityEngine;

namespace ClimateSim.Grids
{
    public class Vertex
    {
        public int Index = -1;

        public List<IcosahedralFace> Faces = new List<IcosahedralFace>();

        public List<Edge> Edges = new List<Edge>();

        public Vector3 Position = new Vector3(0, 0, 0);
    }
}
