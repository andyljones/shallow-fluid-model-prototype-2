using System.Collections.Generic;
using UnityEngine;

namespace Foam
{
    public class Vertex
    {
        public List<Cell> Cells = new List<Cell>();

        public List<Face> Faces = new List<Face>();

        public List<Edge> Edges = new List<Edge>();

        public Vector3 Position = new Vector3(0, 0, 0);
    }
}
