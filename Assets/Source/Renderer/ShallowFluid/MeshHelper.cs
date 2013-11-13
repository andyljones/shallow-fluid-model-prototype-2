using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class MeshHelper
    {
        public Vector3[] Positions;
        public int[] Triangles;

        public Dictionary<Vertex, int> VertexIndices;
        public Dictionary<Face, int> FaceIndices;

        private readonly Vector3[] _initialPositions;
        private readonly float _heightMultiplier;
        private float[] _initialHeights;

        public MeshHelper(List<Face> faces, float heightMultiplier)
        {
            _heightMultiplier = heightMultiplier;
            VertexIndices = SetVertexIndices(faces);
            FaceIndices = SetFaceIndices(faces, VertexIndices.Count);
            Positions = InitializePositions(VertexIndices, FaceIndices, heightMultiplier);
            _initialPositions = Positions;
            _initialHeights = GetHeights(VertexIndices, FaceIndices);
            Triangles = InitializeTriangles(faces);
        }

        private float[] GetHeights(Dictionary<Vertex, int> vertexIndices, Dictionary<Face, int> faceIndices)
        {
            var heights = new float[vertexIndices.Count + faceIndices.Count];

            foreach (var vertexAndIndex in vertexIndices)
            {
                var vertex = vertexAndIndex.Key;
                var index = vertexAndIndex.Value;
                heights[index] = vertex.Cells.Average(cell => cell.Height);
            }

            foreach (var faceAndIndex in faceIndices)
            {
                var face = faceAndIndex.Key;
                var index = faceAndIndex.Value;
                heights[index] = face.Cells.Average(cell => cell.Height);
            }

            return heights;
        }

        private Dictionary<Vertex, int> SetVertexIndices(List<Face> faces)
        {
            var vertices = faces.SelectMany(cell => cell.Vertices).Distinct().ToList();
            var vertexIndices = new Dictionary<Vertex, int>(vertices.Count);

            for (int i = 0; i < vertices.Count; i++)
            {
                vertexIndices.Add(vertices[i], i);
            }

            return vertexIndices;
        }

        private Dictionary<Face, int> SetFaceIndices(List<Face> faces, int offset)
        {
            var faceIndices = new Dictionary<Face, int>(faces.Count);

            for (int i = 0; i < faces.Count; i++)
            {
                faceIndices.Add(faces[i], i + offset);
            }

            return faceIndices;
        }

        private Vector3[] InitializePositions(Dictionary<Vertex, int> vertexIndices, Dictionary<Face, int> faceIndices, float heightMultiplier)
        {
            var positions = new Vector3[vertexIndices.Count + faceIndices.Count];

            foreach (var vertexAndIndex in vertexIndices)
            {
                var vertex = vertexAndIndex.Key;
                var index = vertexAndIndex.Value;
                positions[index] = vertex.Position * heightMultiplier;
            }

            foreach (var faceAndIndex in faceIndices)
            {
                var face = faceAndIndex.Key;
                var index = faceAndIndex.Value;
                positions[index] = FoamUtils.CenterOf(face)*heightMultiplier;
            }

            return positions;
        }

        public void UpdatePositions(float multiplier)
        {
            foreach (var vertexAndIndex in VertexIndices)
            {
                var vertex = vertexAndIndex.Key;
                var index = vertexAndIndex.Value;
                var originalHeight = _initialHeights[index];
                var originalPosition = _initialPositions[index];
                var currentHeight = vertex.Cells.Average(cell => cell.Height); //TODO: this is probably slow
                Positions[index] = (1 + (currentHeight - originalHeight) * multiplier) * originalPosition;
            }

            foreach (var faceAndIndex in FaceIndices)
            {
                var face = faceAndIndex.Key;
                var index = faceAndIndex.Value;
                var originalHeight = _initialHeights[index];
                var originalPosition = _initialPositions[index];
                var currentHeight = face.Cells.Average(cell => cell.Height); //TODO: as is this
                Positions[index] = (1 + (currentHeight - originalHeight) * multiplier) * originalPosition;
            }
        }

        private int[] InitializeTriangles(List<Face> faces)
        {
            var triangleBuffer = new List<int>();

            foreach (var face in faces)
            {
                triangleBuffer.AddRange(TrianglesInFace(face));
            }

            return triangleBuffer.ToArray();
        }

        private IEnumerable<int> TrianglesInFace(Face face)
        {
            var faceIndex = FaceIndices[face];
            var faceCenter = Positions[faceIndex];

            var baseline = Vector3.Cross(faceCenter, face.Vertices.First().Position);
            var clockwiseComparer = new CompareVectorsClockwise(faceCenter, baseline);
            var vertexIndices = face.Vertices.Select(vertex => VertexIndices[vertex]);
            var clockwiseSortedIndices = vertexIndices.OrderBy(index => Positions[index], clockwiseComparer).ToList();

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