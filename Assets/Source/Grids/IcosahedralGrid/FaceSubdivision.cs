using System;
using System.Collections.Generic;
using System.Linq;

namespace ClimateSim.Grids.IcosahedralGrid
{
    public class FaceSubdivision
    {
        public List<IcosahedralFace> Faces { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Vertices { get; private set; }

        public FaceSubdivision(IcosahedralFace face)
        {
            if (NorthPointing(face))
            {
                SubdivideNorthPointingFace(face);
            }
            else
            {
                SubdivideSouthPointingFace(face);
            }
        }

        private void SubdivideSouthPointingFace(IcosahedralFace face)
        {
            throw new NotImplementedException();
        }

        private void SubdivideNorthPointingFace(IcosahedralFace face)
        {
            throw new NotImplementedException();
        }

        public bool NorthPointing(IcosahedralFace face)
        {
            var zValues = face.Vertices.Select(vertex => vertex.Position.z).ToList();
            var midZ = (zValues.Max() + zValues.Min())/2;

            return zValues.Average() < midZ;
        }
    }
}