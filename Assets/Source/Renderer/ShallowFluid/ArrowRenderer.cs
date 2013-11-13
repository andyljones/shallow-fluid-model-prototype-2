using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{


    public class ArrowRenderer
    {
        private List<Cell> _cells;
        private Dictionary<Cell, Vector3> _cellCenter = new Dictionary<Cell, Vector3>(); 

        private Dictionary<Cell, int> _arrowIndices = new Dictionary<Cell, int>();
        private Mesh _arrowMesh;

        private float _lengthMultiplier;


        public ArrowRenderer(List<Cell> cells, float lengthMultiplier, string arrowMaterialName)
        {
            _cells = cells;
            _lengthMultiplier = lengthMultiplier;

            InitializeArrows(arrowMaterialName);
        }

        private void InitializeArrows(string arrowMaterialName)
        {
            var vectors = new Vector3[2 * _cells.Count];
            var lines = new int[2 * _cells.Count];

            for (int i = 0; i < _cells.Count; i++)
            {
                var cell = _cells[i];

                var cellCenter = FoamUtils.CenterOf(cell) *_lengthMultiplier;

                _cellCenter.Add(cell, cellCenter);
                _arrowIndices.Add(cell, 2 * i + 1);

                lines[2 * i] = 2 * i;
                lines[2 * i + 1] = 2 * i + 1;
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
            var arrowMaterial = (Material)Resources.Load(arrowMaterialName, typeof(Material));
            arrowRenderer.material = arrowMaterial;
        }

        public void UpdateArrows()
        {
            var localVertices = new Vector3[2 * _cells.Count];

            foreach (var cell in _cells)
            {
                var cellCenter = _cellCenter[cell];
                var arrowVector = cell.Velocity;

                var arrowIndex = _arrowIndices[cell];
                localVertices[arrowIndex - 1] = cellCenter;
                localVertices[arrowIndex] = cellCenter + arrowVector * 1000000;
            }

            _arrowMesh.vertices = localVertices;
        }
    }
}
