using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class LayerRenderer
    {
        public Mesh LayerMesh;
        public GameObject LayerObject { get; set; }
        public MeshHelper Helper { get; private set; }

        private BoundaryRenderer _boundaryRenderer;

        public LayerRenderer(List<Face> faces, float heightMultiplier, string layerMaterialName, string boundaryMaterialName = null)
        {
            Helper = new MeshHelper(faces, heightMultiplier);
            LayerObject = InitializeLayer(Helper, layerMaterialName);
            LayerMesh = LayerObject.GetComponent<MeshFilter>().mesh;

            if (boundaryMaterialName != null)
            {
                _boundaryRenderer = new BoundaryRenderer(faces, Helper, boundaryMaterialName, LayerObject);
            }
        }

        private GameObject InitializeLayer(MeshHelper helper, string materialName)
        {
            var layerObject = new GameObject("Layer Object");

            var layerMesh = layerObject.AddComponent<MeshFilter>();
            layerMesh.mesh.subMeshCount = 1;
            layerMesh.mesh.vertices = helper.Positions;
            layerMesh.mesh.SetIndices(helper.Triangles, MeshTopology.Triangles, 0);
            layerMesh.mesh.normals = helper.Positions.Select(vector => vector.normalized).ToArray();
            layerMesh.mesh.uv = new Vector2[helper.Positions.Length];

            var layerRenderer = layerObject.AddComponent<MeshRenderer>();
            layerRenderer.materials = new [] {(Material)Resources.Load(materialName, typeof(Material))};

            return layerObject;
        }

        public void UpdateLayer(float multiplier)
        {
            Helper.UpdatePositions(multiplier);
            LayerMesh.vertices = Helper.Positions;
        }
    }
}

