using System.Collections.Generic;

namespace ClimateSim.Grids
{
    public class Edge
    {
        public int Index;

        public List<IcosahedralFace> Faces = new List<IcosahedralFace>();

        public List<Vertex> Vertices = new List<Vertex>();

        public Edge(int index)
        {
            Index = index;
        }
    }

}
