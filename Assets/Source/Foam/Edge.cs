using System.Collections.Generic;

namespace Foam
{
    public class Edge
    {
        public List<Cell> Cells = new List<Cell>(); 

        public List<Face> Faces = new List<Face>();

        public List<Vertex> Vertices = new List<Vertex>();
    }

}
