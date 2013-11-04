
using System;
using System.Collections.Generic;

namespace ClimateSim.Grids
{
    public class Edge
    {
        public int Index = -1;

        public List<Face> Faces = new List<Face>();

        public List<Vertex> Vertices = new List<Vertex>();
    }

}
