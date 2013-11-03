using System.Collections.Generic;
using System.Linq;
using ClimateSim.Grids;
using UnityEngine;

namespace ClimateSim.Grids.IcosahedralGrid
{
    public class IcosahedralGridGenerator
    {
        public List<IcosahedralFace> Faces { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Vertices { get; private set; }

        /// <summary>
        /// This class generates an icosphere by subdividing the faces of an icosahedron until the desired angular 
        /// resolution is reached. 
        /// </summary>
        /// <param name="options"></param>
        public IcosahedralGridGenerator(IIcosahedralGridOptions options)
        {
            var targetAngularResolution = options.Resolution / options.Radius;

            // Create our basic icosahedron
            CreateIcosahedron();
            var currentAngularResolution = 1 / Mathf.Sin(2 * Mathf.PI / 5);

            while (currentAngularResolution > targetAngularResolution)
            {
                // Break all the edges in two and create midpoints between each new pair.
                var edgeSubdivision = new EdgeSubdivision(Edges, Vertices);
                Edges = edgeSubdivision.Edges;
                Vertices = edgeSubdivision.Vertices;

                // Break all the faces into four, generating some new edges in the process as well.
                var faceSubdivision = new FaceSubdivision(Faces, Edges, Vertices);
                Faces = faceSubdivision.Faces;
                Edges = faceSubdivision.Edges;
                Vertices = faceSubdivision.Vertices;

                currentAngularResolution /= 2;
            }
        }

        private void CreateIcosahedron()
        {
            var icosahedron = new Icosahedron();

            Faces = icosahedron.Faces;
            Edges = icosahedron.Edges;
            Vertices = icosahedron.Vertices;
        }
    }
}
