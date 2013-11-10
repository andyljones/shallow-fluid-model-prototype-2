using System.Collections.Generic;

namespace Foam
{
    public class Cell
    {
        public List<Face> Faces = new List<Face>();
        public List<Edge> Edges = new List<Edge>();
        public List<Vertex> Vertices = new List<Vertex>();

        public float Streamfunction;
        public float VelocityPotential;
        public float Depth;
        public float Area;
    }
}
