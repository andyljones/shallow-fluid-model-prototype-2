using System;
using System.Collections.Generic;

namespace ClimateSim.Grids
{
    //TODO: Add strip index
    public class IcosahedralFace
    {
        public int BlockIndex = -1;

        public int IndexInBlock = -1;

        public List<Vertex> Vertices = new List<Vertex>();

        public List<Edge> Edges = new List<Edge>();
    }
}
