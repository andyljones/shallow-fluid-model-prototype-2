using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Foam;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class ShallowFluidRenderer : IRenderer
    {
        public Vector3[] Vectors { get; private set; }
        public int[] Triangles { get; private set; }

        private Dictionary<Vertex, int> _vertexIndices = new Dictionary<Vertex, int>();
        private Dictionary<Face, int> _faceIndices = new Dictionary<Face, int>(); 

        public ShallowFluidRenderer(IAtmosphere atmosphere, IShallowFluidRendererOptions options)
        {
            InitializeVectors(atmosphere.Cells);
            InitializeTriangles(atmosphere.Cells);
        }

        private void InitializeVectors(List<Cell> cells)
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
            var centerOfFace = sumOfVertexPositions/face.Vertices.Count;

            return centerOfFace;
        }

        private void InitializeTriangles(List<Cell> cells)
        {
            var triangleBuffer = new List<int>();

            foreach (var cell in cells)
            {
                var faces = FacesToBeRendered(cell);

                foreach (var face in faces)
                {
                    triangleBuffer.AddRange(TrianglesInFace(face));
                }
            }

            Triangles = triangleBuffer.ToArray();
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
                triangles.Add(clockwiseSortedIndices[(i+1) % clockwiseSortedIndices.Count]);
            }

            return triangles;
        }

        private List<Face> FacesToBeRendered(Cell cell)
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