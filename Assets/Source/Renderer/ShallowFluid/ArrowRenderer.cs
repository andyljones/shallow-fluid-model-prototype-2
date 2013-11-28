using System.Collections.Generic;
using System.Linq;
using Foam;
using Renderer.Heightmap;
using UnityEngine;

namespace Renderer.ShallowFluid
{


    public class ArrowRenderer
    {
        private List<Cell> _cells;
        private Dictionary<Cell, Vector3> _cellCenter = new Dictionary<Cell, Vector3>(); 
        private Dictionary<Cell, Vector3> _localNorth = new Dictionary<Cell, Vector3>();
        private Dictionary<Cell, Vector3> _localEast = new Dictionary<Cell, Vector3>(); 

        private Dictionary<Cell, int> _arrowIndices = new Dictionary<Cell, int>();
        private Mesh _arrowMesh;

        private IHeightmap _heightmap;
        private float _arrowScaleFactor;


        public ArrowRenderer(List<Cell> cells, IHeightmap heightmap, float arrowScaleFactor, string arrowMaterialName)
        {
            _cells = cells;
            _heightmap = heightmap;
            _arrowScaleFactor = arrowScaleFactor;

            InitializeArrows(arrowMaterialName);
        }

        private void InitializeArrows(string arrowMaterialName)
        {
            var vectors = new Vector3[2 * _cells.Count];
            var arrows = new int[2 * _cells.Count];

            for (int i = 0; i < _cells.Count; i++)
            {
                var cell = _cells[i];

                var cellCenter = _heightmap.VisualPositionFromActualPosition(FoamUtils.Center(FoamUtils.TopFace(cell)));
                var localEast = Vector3.Cross(cellCenter, new Vector3(0, 0, 1)).normalized;
                var localNorth = Vector3.Cross(localEast, cellCenter).normalized;

                _cellCenter.Add(cell, cellCenter);
                _localEast.Add(cell, localEast);
                _localNorth.Add(cell, localNorth);

                _arrowIndices.Add(cell, 2 * i + 1);

                arrows[2 * i] = 2 * i;
                arrows[2 * i + 1] = 2 * i + 1;
            }

            var arrowObject = new GameObject("Arrows");
            var arrowMesh = arrowObject.AddComponent<MeshFilter>();
            arrowMesh.mesh.subMeshCount = _cells.Count;
            arrowMesh.mesh.vertices = vectors;
            arrowMesh.mesh.SetIndices(arrows, MeshTopology.Lines, 0);
            arrowMesh.mesh.normals = vectors.Select(vector => vector.normalized).ToArray();
            arrowMesh.mesh.uv = new Vector2[vectors.Length];

            _arrowMesh = arrowMesh.mesh;

            var arrowRenderer = arrowObject.AddComponent<MeshRenderer>();
            var arrowMaterial = (Material)Resources.Load(arrowMaterialName, typeof(Material));
            arrowRenderer.material = arrowMaterial;
        }

        public void UpdateArrows()
        {
            var localVertices = new Vector3[2 * _cells.Count];

            foreach (var cell in _cells)
            {
                var cellCenter = _cellCenter[cell];
                var arrowVector = cell.Velocity.x*_localEast[cell] + cell.Velocity.y*_localNorth[cell];

                var arrowIndex = _arrowIndices[cell];
                localVertices[arrowIndex - 1] = cellCenter;
                localVertices[arrowIndex] = cellCenter + arrowVector * _arrowScaleFactor;
            }
            _arrowMesh.vertices = localVertices;
        }
    }
}
