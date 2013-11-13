using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class BoundaryRenderer
    {
        private MeshHelper _helper;

        public BoundaryRenderer(List<Cell> cells, MeshHelper helper, string boundaryMaterialName)
        {
            _helper = helper;
            InitializeBoundaries(cells, boundaryMaterialName);
        }

        private void InitializeBoundaries(List<Cell> cells, string boundaryMaterialName)
        {
            var boundaryRoutes = FindBoundaryRoutes(cells);

            var boundaryObject = new GameObject("Boundaries");
            var boundaryMesh = boundaryObject.AddComponent<MeshFilter>();
            boundaryMesh.mesh.vertices = _helper.Positions;
            boundaryMesh.mesh.subMeshCount = boundaryRoutes.Count;

            for (int i = 0; i < boundaryRoutes.Count; i++)
            {
                var boundary = boundaryRoutes[i];
                boundaryMesh.mesh.SetIndices(boundary, MeshTopology.LineStrip, i);
            }

            var boundaryRenderer = boundaryObject.AddComponent<MeshRenderer>();
            var boundaryMaterial = (Material)Resources.Load(boundaryMaterialName, typeof(Material));
            boundaryRenderer.materials = Enumerable.Repeat(boundaryMaterial, boundaryRoutes.Count).ToArray();
        }

        private List<int[]> FindBoundaryRoutes(List<Cell> cells)
        {
            var routes = new List<int[]>();

            var atmosphericFaces = cells.Select<Cell, Face>(FoamUtils.TopFaceOf);
            var atmosphericEdges = atmosphericFaces.SelectMany(face => face.Edges).Distinct().ToList();

            var routeFinder = new BoundaryRouteFinder(atmosphericEdges);

            while (!routeFinder.AllEdgesVisited())
            {
                var route = routeFinder.GetRoute();
                var routeIndices = route.Select(vertex => _helper.VertexIndices[vertex]);

                routes.Add(routeIndices.ToArray());
            }

            return routes;
        }
    }
}
