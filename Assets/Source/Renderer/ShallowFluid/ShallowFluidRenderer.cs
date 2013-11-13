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
        private ISimulator _simulator;
        private List<Cell> _cells;
        private IShallowFluidRendererOptions _options;
        private ArrowRenderer _arrowRenderer;
        private BoundaryRenderer _boundaryRenderer;

        public ShallowFluidRenderer(ISimulator simulator, IShallowFluidRendererOptions options)
        {
            _simulator = simulator;
            _cells = simulator.Cells;
            _options = options;
            var helper = new MeshHelper(simulator.Cells, _options);

            InitializeLayers(helper, _options);

            _boundaryRenderer = new BoundaryRenderer(_cells, helper, _options.BoundaryMaterial);
            _arrowRenderer = new ArrowRenderer(_cells, _options.DetailMultiplier, _options.ArrowMaterial);
        }

        public void UpdateRender()
        {
            _simulator.StepSimulation();
            _simulator.UpdateCellConditions();

            _arrowRenderer.UpdateArrows();
        }

        private void InitializeLayers(MeshHelper helper, IShallowFluidRendererOptions options)
        {
            var layerHolder = new GameObject("Layer Holder");

            for (int i = 0; i < helper.LayerTriangles.Count; i++)
            {
                var layerObject = new GameObject("Layer Object");
                layerObject.transform.parent = layerHolder.transform;
                
                var layerMesh = layerObject.AddComponent<MeshFilter>();
                layerMesh.mesh.vertices = helper.Vectors;
                layerMesh.mesh.triangles = helper.LayerTriangles[i];
                layerMesh.mesh.normals = helper.Vectors.Select(vector => vector.normalized).ToArray();

                var layerRenderer = layerObject.AddComponent<MeshRenderer>();
                layerRenderer.material = (Material) Resources.Load(options.LayerMaterials[i], typeof(Material));
            }
        }
    }
}