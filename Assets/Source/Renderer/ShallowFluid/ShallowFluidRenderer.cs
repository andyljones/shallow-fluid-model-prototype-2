using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class ShallowFluidRenderer : IRenderer
    {
        public Vector3[] Vectors { get; private set; }
        public int[] AtmosphereTriangles { get; private set; }
        public int[] SurfaceTriangles { get; private set; }

        private Dictionary<Vertex, int> _vertexIndices = new Dictionary<Vertex, int>();
        private Dictionary<Face, int> _faceIndices = new Dictionary<Face, int>(); 

        public ShallowFluidRenderer(IAtmosphere atmosphere, IShallowFluidRendererOptions options)
        {
            var helper = new MeshHelper();

            helper.InitializeVectors(atmosphere.Cells);
            helper.InitializeTriangles(atmosphere.Cells);

            Vectors = helper.Vectors;
            AtmosphereTriangles = helper.AtmosphereTriangles;
            SurfaceTriangles = helper.SurfaceTriangles;
        }
    }
}