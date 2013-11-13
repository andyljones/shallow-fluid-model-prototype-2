using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class MeshHelper
    {
        public Vector3[] Vectors;
        public List<int[]> LayerTriangles = new List<int[]>();
        public List<int[]> Boundaries = new List<int[]>();

        public Dictionary<Vertex, int> VertexIndices = new Dictionary<Vertex, int>();
        public Dictionary<Face, int> FaceIndices = new Dictionary<Face, int>();

        private float _detailMultiplier;

        public MeshHelper(List<Cell> cells, IShallowFluidRendererOptions options)
        {
            _detailMultiplier = options.DetailMultiplier;

            InitializeVectors(cells);
            InitializeTriangles(cells);
        }

        private void InitializeVectors(List<Cell> cells)
        {
            var vertices = cells.SelectMany(cell => cell.Vertices).Distinct().ToList();
            var surfaceFaces = cells.Select<Cell, Face>(FoamUtils.BottomFaceOf).ToList();
            var atmosphereFaces = cells.Select<Cell, Face>(FoamUtils.TopFaceOf).ToList();
            var faces = surfaceFaces.Concat(atmosphereFaces).ToList();

            Vectors = new Vector3[vertices.Count + faces.Count];

            for (int i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                var position = vertex.Position;
                Vectors[i] = position;
                VertexIndices.Add(vertex, i);
            }

            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];
                var position = CenterOfFace(face);
                Vectors[i + vertices.Count] = position;
                FaceIndices.Add(face, i + vertices.Count);
            }

            var vectorsToBeMultiplied = new List<int>();

            foreach (var face in atmosphereFaces)
            {
                vectorsToBeMultiplied.Add(FaceIndices[face]);
                vectorsToBeMultiplied.AddRange(face.Vertices.Select(vertex => VertexIndices[vertex]));
            }

            foreach (var vectorIndex in vectorsToBeMultiplied.Distinct())
            {
                Vectors[vectorIndex] *= _detailMultiplier;
            }

        }

        private Vector3 CenterOfFace(Face face)
        {
            var sumOfVertexPositions = face.Vertices.Aggregate(new Vector3(), (position, vertex) => position + vertex.Position);
            var centerOfFace = sumOfVertexPositions / face.Vertices.Count;

            return centerOfFace;
        }

        private void InitializeTriangles(List<Cell> cells)
        {
            LayerTriangles = new List<int[]>();

            var atmosphereTriangleBuffer = new List<int>();
            var surfaceTriangleBuffer = new List<int>();

            foreach (var cell in cells)
            {
                var atmosphereFace = FoamUtils.TopFaceOf(cell);
                var surfaceFace = FoamUtils.BottomFaceOf(cell);

                atmosphereTriangleBuffer.AddRange(TrianglesInFace(atmosphereFace));
                surfaceTriangleBuffer.AddRange(TrianglesInFace(surfaceFace));
            }

            LayerTriangles.Add(surfaceTriangleBuffer.ToArray());
            LayerTriangles.Add(atmosphereTriangleBuffer.ToArray());
        }

        private IEnumerable<int> TrianglesInFace(Face face)
        {
            var faceIndex = FaceIndices[face];
            var facePosition = Vectors[faceIndex];

            var baseline = Vector3.Cross(facePosition, face.Vertices.First().Position);
            var clockwiseComparer = new CompareVectorsClockwise(facePosition, baseline); //TODO: Uhh this'll still be degenerate at the poles
            var vertexIndices = face.Vertices.Select(vertex => VertexIndices[vertex]);
            var clockwiseSortedIndices = vertexIndices.OrderBy(index => Vectors[index], clockwiseComparer).ToList();

            var triangles = new List<int>();

            for (int i = 0; i < clockwiseSortedIndices.Count; i++)
            {
                triangles.Add(faceIndex);
                triangles.Add(clockwiseSortedIndices[i]);
                triangles.Add(clockwiseSortedIndices[(i + 1) % clockwiseSortedIndices.Count]);
            }

            return triangles;
        }

    }
}