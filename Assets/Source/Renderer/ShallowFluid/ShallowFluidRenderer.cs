﻿using System.Collections.Generic;
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

        private Dictionary<Cell, int> _arrowIndex = new Dictionary<Cell, int>();
        private Dictionary<Cell, Vector3> _cellCenter = new Dictionary<Cell, Vector3>(); 
        private Dictionary<Cell, Vector3> _localEast = new Dictionary<Cell, Vector3>();
        private Dictionary<Cell, Vector3> _localNorth = new Dictionary<Cell, Vector3>();
        private Mesh _arrowMesh;

        public ShallowFluidRenderer(ISimulator simulator, IShallowFluidRendererOptions options)
        {
            _simulator = simulator;
            _cells = simulator.Cells;
            _options = options;
            var helper = new MeshHelper(simulator.Cells, _options);

            InitializeLayers(helper, _options);
            InitializeBoundaries(helper, _options);
            InitializeArrows();
        }

        public void UpdateRender()
        {
            _simulator.StepSimulation();

            var localVertices = new Vector3[2*_cells.Count];

            foreach (var cell in _cells)
            {
                var cellCenter = _cellCenter[cell];
                var arrowVector = cell.Velocity.normalized; //TODO Remove this.

                var arrowIndex = _arrowIndex[cell];
                localVertices[arrowIndex - 1] = cellCenter;
                localVertices[arrowIndex] = cellCenter + arrowVector * _options.Radius * _options.Resolution / 40000;
            }

            _arrowMesh.vertices = localVertices;
        }

        private void InitializeArrows()
        {
            var vectors = new Vector3[2*_cells.Count];
            var lines = new int[2*_cells.Count];

            for (int i =0; i < _cells.Count; i++)
            {
                var cell = _cells[i];

                var cellCenter = FoamUtils.CenterOf(cell) * _options.DetailMultiplier;
                var localEast = Vector3.Cross(cellCenter, new Vector3(0, 0, 1)).normalized;
                var localNorth = Vector3.Cross(localEast, cellCenter).normalized;

                _cellCenter.Add(cell, cellCenter);
                _localEast.Add(cell, localEast);
                _localNorth.Add(cell, localNorth);
                _arrowIndex.Add(cell, 2*i + 1);

                var arrowVector = cell.Velocity.x*localEast + cell.Velocity.y*localNorth;

                vectors[2*i] = cellCenter;
                vectors[2*i + 1] = cellCenter + arrowVector * _options.Radius * _options.Resolution / 40000;

                lines[2*i] = 2*i;
                lines[2*i + 1] = 2*i + 1;
            }

            var arrowObject = new GameObject("Arrows");
            var arrowMesh = arrowObject.AddComponent<MeshFilter>();
            arrowMesh.mesh.subMeshCount = _cells.Count;
            arrowMesh.mesh.vertices = vectors;
            arrowMesh.mesh.SetIndices(lines, MeshTopology.Lines, 0);
            arrowMesh.mesh.normals = vectors.Select(vector => vector.normalized).ToArray();
            arrowMesh.mesh.uv = new Vector2[vectors.Length];

            _arrowMesh = arrowMesh.mesh;

            var arrowRenderer = arrowObject.AddComponent<MeshRenderer>();
            var arrowMaterial = (Material) Resources.Load(_options.ArrowMaterial, typeof (Material));
            arrowRenderer.material = arrowMaterial;
        }

        private void InitializeBoundaries(MeshHelper helper, IShallowFluidRendererOptions options)
        {
            var boundaryObject = new GameObject("Boundaries");
            var boundaryMesh = boundaryObject.AddComponent<MeshFilter>();
            boundaryMesh.mesh.vertices = helper.Vectors;
            boundaryMesh.mesh.subMeshCount = helper.Boundaries.Count;

            for (int i = 0; i < helper.Boundaries.Count; i++)
            {
                var boundary = helper.Boundaries[i];
                boundaryMesh.mesh.SetIndices(boundary, MeshTopology.LineStrip, i);
            }

            var boundaryRenderer = boundaryObject.AddComponent<MeshRenderer>();
            var boundaryMaterial = (Material) Resources.Load(options.BoundaryMaterial, typeof (Material));
            boundaryRenderer.materials = Enumerable.Repeat(boundaryMaterial, helper.Boundaries.Count).ToArray();
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