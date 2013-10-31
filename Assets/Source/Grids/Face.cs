using System.Collections.Generic;

namespace ClimateSim.Grids
{
    //TODO: Add strip index
    public class Face
    {
        public int Index = -1;

        public List<Vertex> Vertices = new List<Vertex>();

        public List<Edge> Edges = new List<Edge>();
    }
}
