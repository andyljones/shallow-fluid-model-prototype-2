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

        public MeshHelper(List<Face> faces, float heightMultiplier)
        {
            VertexIndices = SetVertexIndices(faces);
            FaceIndices = SetFaceIndices(faces, VertexIndices.Count);
            Positions = InitializeVectors(faces, heightMultiplier);
            Triangles = InitializeTriangles(faces);
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

        private Vector3[] InitializeVectors(List<Face> faces, float heightMultiplier)
        {
            var vertices = faces.SelectMany(cell => cell.Vertices).Distinct().ToList();

            var positions = new Vector3[vertices.Count + faces.Count];

            for (int i = 0; i < vertices.Count; i++)
            {
                positions[i] = vertices[i].Position * heightMultiplier;
            }

            for (int i = 0; i < faces.Count; i++)
            {
                positions[i + vertices.Count] = FoamUtils.CenterOf(faces[i]) * heightMultiplier;
            }

            return positions;
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