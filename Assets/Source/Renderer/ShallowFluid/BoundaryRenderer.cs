using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class BoundaryRenderer
    {
        private MeshHelper _helper;

        public BoundaryRenderer(List<Face> faces, MeshHelper helper, string boundaryMaterialName, GameObject layerObject)
        {
            _helper = helper;
            InitializeBoundaries(faces, boundaryMaterialName, layerObject);
        }

        private void InitializeBoundaries(List<Face> faces, string boundaryMaterialName, GameObject layerObject)
        {
            var boundaryRoutes = FindBoundaryRoutes(faces);

            var layerMesh = layerObject.GetComponent<MeshFilter>();
            var layerRenderer = layerObject.GetComponent<MeshRenderer>();

            var oldSubmeshCount = layerMesh.mesh.subMeshCount;
            layerMesh.mesh.subMeshCount = oldSubmeshCount + boundaryRoutes.Count;

            for (int i = 0; i < boundaryRoutes.Count; i++)
            {
                var boundary = boundaryRoutes[i];
                layerMesh.mesh.SetIndices(boundary, MeshTopology.LineStrip, i + oldSubmeshCount);
            }

            var boundaryMaterial = (Material)Resources.Load(boundaryMaterialName, typeof(Material));
            var oldMaterials = layerRenderer.materials;
            var newMaterials = oldMaterials.Concat(Enumerable.Repeat(boundaryMaterial, boundaryRoutes.Count)).ToArray();
            layerRenderer.materials = newMaterials;
        }

        private List<int[]> FindBoundaryRoutes(List<Face> faces)
        {
            var routes = new List<int[]>();

            var edges = faces.SelectMany(face => face.Edges).Distinct().ToList();

            var routeFinder = new BoundaryRouteFinder(edges);

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
