using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class MeshHelper
    {
        public Vector3[] Vectors { get; private set; }
        public int[] AtmosphereTriangles { get; private set; }
        public int[] SurfaceTriangles { get; private set; }

        private Dictionary<Vertex, int> _vertexIndices = new Dictionary<Vertex, int>();
        private Dictionary<Face, int> _faceIndices = new Dictionary<Face, int>();

        public MeshHelper()
        {
            
        }

        public void InitializeVectors(List<Cell> cells)
        {
            var vertices = cells.SelectMany(cell => cell.Vertices).Distinct().ToList();
            var faces = cells.SelectMany(cell => cell.Faces).Distinct().ToList();

            Vectors = new Vector3[vertices.Count + faces.Count];

            for (int i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                var position = vertex.Position;
                Vectors[i] = position;
                _vertexIndices.Add(vertex, i);
            }

            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];
                var position = CenterOfFace(face);
                Vectors[i + vertices.Count] = position;
                _faceIndices.Add(face, i + vertices.Count);
            }

        }

        private Vector3 CenterOfFace(Face face)
        {
            var sumOfVertexPositions = face.Vertices.Aggregate(new Vector3(), (position, vertex) => position + vertex.Position);
            var centerOfFace = sumOfVertexPositions / face.Vertices.Count;

            return centerOfFace;
        }

        public void InitializeTriangles(List<Cell> cells)
        {
            var atmosphereTriangleBuffer = new List<int>();
            var surfaceTriangleBuffer = new List<int>();

            foreach (var cell in cells)
            {
                var atmosphereFaces = AtmosphereFacesToBeRendered(cell);
                var surfaceFaces = SurfaceFacesToBeRendered(cell);

                foreach (var face in atmosphereFaces)
                {
                    atmosphereTriangleBuffer.AddRange(TrianglesInFace(face));
                }

                foreach (var face in surfaceFaces)
                {
                    surfaceTriangleBuffer.AddRange(TrianglesInFace(face));
                }
            }

            AtmosphereTriangles = atmosphereTriangleBuffer.ToArray();
            SurfaceTriangles = surfaceTriangleBuffer.ToArray();
        }

        private List<Face> SurfaceFacesToBeRendered(Cell cell)
        {
            var verticesOrderedByHeight = cell.Vertices.OrderBy(vertex => vertex.Position.magnitude).ToList();

            var facesNeighbouringLowestVertex = verticesOrderedByHeight[0].Faces;
            var facesNeighbouringSecondLowestVertex = verticesOrderedByHeight[1].Faces;
            var facesNeighbouringThirdLowestVertex = verticesOrderedByHeight[2].Faces;

            var lowestFace =
                facesNeighbouringLowestVertex
                .Intersect(facesNeighbouringSecondLowestVertex)
                .Intersect(facesNeighbouringThirdLowestVertex);

            return lowestFace.ToList();
        }

        private IEnumerable<int> TrianglesInFace(Face face)
        {
            var faceIndex = _faceIndices[face];
            var facePosition = Vectors[faceIndex];

            var clockwiseComparer = new CompareVectorsClockwise(facePosition, new Vector3(0, 0, 1));
            var vertexIndices = face.Vertices.Select(vertex => _vertexIndices[vertex]);
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

        private List<Face> AtmosphereFacesToBeRendered(Cell cell)
        {
            var verticesOrderedByHeight = cell.Vertices.OrderByDescending(vertex => vertex.Position.magnitude).ToList();

            var facesNeighbouringHighestVertex = verticesOrderedByHeight[0].Faces;
            var facesNeighbouringSecondHighestVertex = verticesOrderedByHeight[1].Faces;
            var facesNeighbouringThirdHighestVertex = verticesOrderedByHeight[2].Faces;

            var highestFace =
                facesNeighbouringHighestVertex
                .Intersect(facesNeighbouringSecondHighestVertex)
                .Intersect(facesNeighbouringThirdHighestVertex);

            return highestFace.ToList();
        }

    }
}