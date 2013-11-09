using System.Collections.Generic;

namespace Foam
{
    public class Cell
    {
        public int Index;

        public List<Face> Faces = new List<Face>();

        public List<Edge> Edges = new List<Edge>();

        public List<Vertex> Vertices = new List<Vertex>();
    }
}
