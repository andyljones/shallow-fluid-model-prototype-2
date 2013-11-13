using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class LayerRenderer
    {
        public GameObject LayerObject;
        public MeshHelper Helper { get; private set; }

        public LayerRenderer(List<Face> faces, float heightMultiplier, string layerMaterialName)
        {
            Helper = new MeshHelper(faces, heightMultiplier);
            LayerObject = InitializeLayer(Helper, layerMaterialName);
        }

        private GameObject InitializeLayer(MeshHelper helper, string materialName)
        {
            var layerObject = new GameObject("Layer Object");

            var layerMesh = layerObject.AddComponent<MeshFilter>();
            layerMesh.mesh.vertices = helper.Positions;
            layerMesh.mesh.triangles = helper.Triangles;
            layerMesh.mesh.normals = helper.Positions.Select(vector => vector.normalized).ToArray();

            var layerRenderer = layerObject.AddComponent<MeshRenderer>();
            layerRenderer.material = (Material)Resources.Load(materialName, typeof(Material));

            return layerObject;
        }
    }
}

