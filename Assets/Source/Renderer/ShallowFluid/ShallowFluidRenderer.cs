using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Foam;
using Simulator;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class ShallowFluidRenderer : IRenderer
    {
        public ShallowFluidRenderer(ISimulator simulator, IShallowFluidRendererOptions options)
        {
            var helper = new MeshHelper(simulator.Cells, options);

            RenderLayers(helper, options);
            RenderBoundaries(helper, options);
        }

        //TODO: Implement a Chinese Postman solution.
        private void RenderBoundaries(MeshHelper helper, IShallowFluidRendererOptions options)
        {
            var boundaryHolder = new GameObject("Boundary Holder");

            var boundaryMaterial = (Material) Resources.Load(options.BoundaryMaterial, typeof (Material));
            var boundaryWidth = 0.003f * options.Radius * options.DetailMultiplier;

            foreach (var boundary in helper.Boundaries)
            {
                var boundaryObject = new GameObject("Boundary Object");
                boundaryObject.transform.parent = boundaryHolder.transform;

                var lineRenderer = boundaryObject.AddComponent<LineRenderer>();
                lineRenderer.SetVertexCount(boundary.Length);
                lineRenderer.SetWidth(boundaryWidth, boundaryWidth);
                lineRenderer.material = boundaryMaterial;

                for (int i = 0; i < boundary.Length; i++)
                {
                    var vector = helper.Vectors[boundary[i]] * 1.005f;
                    lineRenderer.SetPosition(i, vector);
                }
            }
        }

        private void RenderLayers(MeshHelper helper, IShallowFluidRendererOptions options)
        {
            var layerHolder = new GameObject("Layer Holder");

            for (int i = 0; i < helper.LayerTriangles.Count; i++)
            {
                var layerObject = new GameObject("Layer Object");
                layerObject.transform.parent = layerHolder.transform;
                
                var layerMesh = layerObject.AddComponent<MeshFilter>();
                layerMesh.mesh.vertices = helper.Vectors;
                layerMesh.mesh.triangles = helper.LayerTriangles[i];
                layerMesh.mesh.RecalculateNormals();

                var layerRenderer = layerObject.AddComponent<MeshRenderer>();
                layerRenderer.material = (Material) Resources.Load(options.LayerMaterials[i], typeof(Material));
            }
        }
    }
}