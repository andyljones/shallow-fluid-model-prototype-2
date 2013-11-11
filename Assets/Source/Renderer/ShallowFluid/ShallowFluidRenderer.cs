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
        private List<Cell> _cells;
        private IShallowFluidRendererOptions _options;

        private Dictionary<Cell, LineRenderer> _arrows = new Dictionary<Cell, LineRenderer>();
        private Dictionary<Cell, Vector3> _cellCenter = new Dictionary<Cell, Vector3>(); 
        private Dictionary<Cell, Vector3> _localEast = new Dictionary<Cell, Vector3>();
        private Dictionary<Cell, Vector3> _localNorth = new Dictionary<Cell, Vector3>(); 

        public ShallowFluidRenderer(ISimulator simulator, IShallowFluidRendererOptions options)
        {
            _cells = simulator.Cells;
            _options = options;
            var helper = new MeshHelper(simulator.Cells, _options);

            InitializeLayers(helper, _options);
            InitializeBoundaries(helper, _options);
            InitializeArrows();
        }

        public void UpdateRender()
        {
            foreach (var cell in _cells)
            {
                var cellCenter = _cellCenter[cell];
                var localEast = _localEast[cell];
                var localNorth = _localNorth[cell];
                var arrow = _arrows[cell];

                var arrowVector = cell.Velocity.x * localEast + cell.Velocity.y * localNorth;

                arrow.SetPosition(0, cellCenter);
                arrow.SetPosition(1, cellCenter + 100 * arrowVector);
            }
        }

        private void InitializeArrows()
        {
            var arrowHolder = new GameObject("Arrow Holder");

            var arrowMaterial = (Material)Resources.Load(_options.ArrowMaterial, typeof(Material));
            var arrowWidth = 0.003f * _options.Radius;

            foreach (var cell in _cells)
            {
                var cellCenter = FoamUtils.CenterOf(cell) * _options.DetailMultiplier;
                var localEast = Vector3.Cross(cellCenter, new Vector3(0, 0, 1)).normalized;
                var localNorth = Vector3.Cross(localEast, cellCenter).normalized;

                _cellCenter.Add(cell, cellCenter);
                _localEast.Add(cell, localEast);
                _localNorth.Add(cell, localNorth);

                var arrowObject = new GameObject("Arrow Object");
                arrowObject.transform.parent = arrowHolder.transform;

                var lineRenderer = arrowObject.AddComponent<LineRenderer>();
                lineRenderer.SetVertexCount(2);
                lineRenderer.SetWidth(arrowWidth, 0);
                lineRenderer.material = arrowMaterial;

                var arrowVector = cell.Velocity.x*localEast + cell.Velocity.y*localNorth;

                lineRenderer.SetPosition(0, cellCenter);
                lineRenderer.SetPosition(1, cellCenter + 100 * arrowVector);

                _arrows.Add(cell, lineRenderer);
            }
        }

        //TODO: Implement a Chinese Postman solution.
        private void InitializeBoundaries(MeshHelper helper, IShallowFluidRendererOptions options)
        {
            var boundaryHolder = new GameObject("Boundary Holder");

            var boundaryMaterial = (Material) Resources.Load(options.BoundaryMaterial, typeof (Material));
            var boundaryWidth = 0.003f * options.Radius;

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
                layerMesh.mesh.RecalculateNormals();

                var layerRenderer = layerObject.AddComponent<MeshRenderer>();
                layerRenderer.material = (Material) Resources.Load(options.LayerMaterials[i], typeof(Material));
            }
        }
    }
}