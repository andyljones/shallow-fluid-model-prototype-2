using System.Collections.Generic;
using UnityEngine;

namespace ClimateSim.Grids.IcosahedralGrid
{
    public class IcosahedralGridGenerator
    {
        public IcosahedralFace[] Faces { get; private set; }
        public Edge[] Edges { get; private set; }
        public Vertex[] Vertices { get; private set; }

        //public IcosahedralGridGenerator(IIcosahedralGridOptions options)
        //{
        //    var targetAngularResolution = options.Resolution/options.Radius;

        //    var currentAngularResolution = 1/Mathf.Sin(2*Mathf.PI/5);
        //    var currentIcosahedron = new Icosahedron();

        //    while (currentAngularResolution > targetAngularResolution)
        //    {
        //        currentIcosahedron = Subdivide(currentIcosahedron);
        //        currentAngularResolution = CalculateAngularResolution(currentIcosahedron);
        //    }
        //}

        //private Icosahedron Subdivide(Icosahedron currentIcosahedron)
        //{
        //    var faces = new List<IcosahedralFace>();
        //    var edges = new List<Edge>();
        //    var vertices = new List<Vector3>();

        //    foreach (var face in currentIcosahedron.Faces)
        //    {
        //        var subdividedFace = new FaceSubdivision(face);
        //    }
        //}

        //private float CalculateAngularResolution(Icosahedron currentIcosahedron)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
