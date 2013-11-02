using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClimateSim.Grids.IcosahedralGrid
{
    public class IcosahedralGridGenerator
    {
        public List<IcosahedralFace> Faces { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Vertices { get; private set; }

        private float _targetAngularResolution;
        private float _currentAngularResolution;

        private readonly Vector3 _globalNorth = new Vector3(0, 0, 1);


        public IcosahedralGridGenerator(IIcosahedralGridOptions options)
        {
            _targetAngularResolution = options.Resolution / options.Radius;

            CreateIcosahedron();

            while (_currentAngularResolution > _targetAngularResolution)
            {
                SubdivideEdges();
                //BUG: Edge counts on each vertex are fine here.
                SubdivideFaces();
                LinkEdgesAndVerticesToFaces();
                _currentAngularResolution /= 2;
            }
        }

        private void LinkEdgesAndVerticesToFaces()
        {
            foreach (var face in Faces)
            {
                AddFaceToEdges(face);
                AddFaceToVertices(face);
            }
        }

        private void AddFaceToEdges(IcosahedralFace face)
        {
            foreach (var edge in face.Edges)
            {
                edge.Faces.Add(face);
            }
        }

        private void AddFaceToVertices(IcosahedralFace face)
        {
            foreach (var vertex in face.Vertices)
            {
                vertex.Faces.Add(face);
            }
        }

        private void CreateIcosahedron()
        {
            var icosahedron = new Icosahedron();
            _currentAngularResolution = 1/Mathf.Sin(2*Mathf.PI/5);

            Faces = icosahedron.Faces;
            Edges = icosahedron.Edges;
            Vertices = icosahedron.Vertices;
        }

        private void SubdivideEdges()
        {
            var newEdges = new List<Edge>();

            foreach (var edge in Edges)
            {
                newEdges.AddRange(SubdivideEdge(edge));
            }

            Edges = newEdges;
            System.Console.WriteLine(Faces.Count(NorthPointing));
        }

        private List<Edge> SubdivideEdge(Edge edge)
        {
            var endpoint0 = edge.Vertices[0];
            var endpoint1 = edge.Vertices[1];

            var face0 = edge.Faces[0];
            var face1 = edge.Faces[1];

            endpoint0.Edges.Remove(edge);
            endpoint1.Edges.Remove(edge);
            face0.Edges.Remove(edge);
            face1.Edges.Remove(edge);

            //var omega = Mathf.Cos(Vector3.Dot(endpoint0.Position, endpoint1.Position));
            var midpointPosition = (endpoint0.Position + endpoint1.Position)/2;
            var midpoint = new Vertex {Position = midpointPosition};

            var newEdge0 = new Edge { Vertices = new List<Vertex> { endpoint0, midpoint } };
            var newEdge1 = new Edge { Vertices = new List<Vertex> { endpoint1, midpoint } };

            midpoint.Edges.Add(newEdge0);
            midpoint.Edges.Add(newEdge1);

            Vertices.Add(midpoint);
            edge.Faces[0].Vertices.Add(midpoint);
            edge.Faces[1].Vertices.Add(midpoint);

            endpoint0.Edges.Add(newEdge0);
            face0.Edges.Add(newEdge0);
            face1.Edges.Add(newEdge0);

            endpoint1.Edges.Add(newEdge1);
            face0.Edges.Add(newEdge1);
            face1.Edges.Add(newEdge1);

            return new List<Edge> {newEdge0, newEdge1};
        }

        private void SubdivideFaces()
        {
            var newFaces = new List<IcosahedralFace>();

            foreach (var face in Faces)
            {
                newFaces.AddRange(SubdivideFace(face));
            }

            Faces = newFaces;
        }

        private IEnumerable<IcosahedralFace> SubdivideFace(IcosahedralFace face)
        {
            //TODO: These are breaking down too. Vertices of face have wild numbers of edges
            //face.Vertices = FindBoundaryVertices(face);
            //face.Edges = FindBoundaryEdges(face);
            List<IcosahedralFace> newFaces;

            if (NorthPointing(face))
            {
                newFaces = SubfacesOfNorthPointingFace(face);    
            }
            else
            {
                newFaces = SubfacesOfSouthPointingFace(face);
            }
            

            return newFaces;
        }

        private List<IcosahedralFace> SubfacesOfNorthPointingFace(IcosahedralFace face)
        {
            var center = CenterOfFace(face);

            //var largestZValue = face.Vertices.Max(vertex => vertex.Position.z);
            //var northmostVertex = face.Vertices.Single(vertex => vertex.Position.z == largestZValue);
            //var baseline = (northmostVertex.Position - center).normalized;

            var northmostVertex = face.Vertices.OrderByDescending(vertex => vertex.Position.z).First();

            var clockwiseFromNorth = new CompareVectorsClockwise(center, new Vector3(0, 0, 1));
            var clockwiseSortedVertices = face.Vertices.OrderBy(vertex => vertex.Position, clockwiseFromNorth).ToList();
            var indexOfNorthmost = clockwiseSortedVertices.IndexOf(northmostVertex);
            var sortedVertices = new Vertex[clockwiseSortedVertices.Count];
            for (int i = 0; i < sortedVertices.Length; i++)
            {
                sortedVertices[i] = clockwiseSortedVertices[MathMod(indexOfNorthmost + i, clockwiseSortedVertices.Count)];
            }

            //     0
            //    / \    <-- Northern subface
            //   5---1
            //  / \ / \  <-- Western, central, then eastern subface
            // 4---3---2


            //BUG: Intuition is the problem's with the sorting. E: Yup, issues here.
            // Problem that the edge alignment issue from the first iteration throws things off true north?
            //var maxZ = face.Vertices.Max(vertex => vertex.Position.z);
            //var northPoint = face.Vertices.Single(vertex => vertex.Position.z == maxZ);
            //System.Console.WriteLine(northPoint == sortedVertices[0]);

            //TODO: Add new edges and vertices to face WHEN THEY'RE CREATED. Don't fuck around with this allNeighbouringVertex stuff
            //TODO: Rather than trying to find local north, sort vertices by Z value and use the position of the largest as the baseline.

            int blockIndex = face.BlockIndex;
            int indexInBlock = face.IndexInBlock;

            var northernSubface = CreateSubface(sortedVertices[5], sortedVertices[0], sortedVertices[1], 2*blockIndex, 2*indexInBlock);
            var easternSubface = CreateSubface(sortedVertices[1], sortedVertices[2], sortedVertices[3], 2*blockIndex+1, 2*indexInBlock);
            var westernSubface = CreateSubface(sortedVertices[3], sortedVertices[4], sortedVertices[5], 2*blockIndex, 2*indexInBlock+2);
            var centralSubface = CreateCentralSubface(sortedVertices[1], sortedVertices[3], sortedVertices[5], 2*blockIndex, 2*indexInBlock+1);

            RemoveFaceFromVertices(face, face.Vertices);

            return new List<IcosahedralFace> {northernSubface, easternSubface, westernSubface, centralSubface};
        }

        private void RemoveFaceFromVertices(IcosahedralFace face, List<Vertex> vertices)
        {
            foreach (var vertex in vertices)
            {
                vertex.Faces = vertex.Faces.Except(new List<IcosahedralFace> {face}).ToList();
            }
        }

        private List<IcosahedralFace> SubfacesOfSouthPointingFace(IcosahedralFace face)
        {
            var center = CenterOfFace(face);
            var southmostVertex = face.Vertices.OrderBy(vertex => vertex.Position.z).First();


            var clockwiseFromNorth = new CompareVectorsClockwise(center, new Vector3(0, 0, 1));
            var clockwiseSortedVertices = face.Vertices.OrderBy(vertex => vertex.Position, clockwiseFromNorth).ToList();
            var indexOfSouthmost = clockwiseSortedVertices.IndexOf(southmostVertex);
            var sortedVertices = new Vertex[clockwiseSortedVertices.Count];
            for (int i = 0; i < sortedVertices.Length; i++)
            {
                sortedVertices[i] = clockwiseSortedVertices[MathMod(indexOfSouthmost + i, clockwiseSortedVertices.Count)];
            }

            //2---3---4
            // \ / \ /     <-- Western subface, central subface, eastern subface
            //  1---5
            //   \ /       <-- Southern subface
            //    0

            int blockIndex = face.BlockIndex;
            int indexInBlock = face.IndexInBlock;

            var southernSubface = CreateSubface(sortedVertices[5], sortedVertices[0], sortedVertices[1], 2 * blockIndex + 1, 2 * indexInBlock + 1);
            var westernSubface = CreateSubface(sortedVertices[1], sortedVertices[2], sortedVertices[3], 2 * blockIndex, 2 * indexInBlock + 1);
            var easternSubface = CreateSubface(sortedVertices[3], sortedVertices[4], sortedVertices[5], 2 * blockIndex + 1, 2 * indexInBlock - 1);

            var centralSubface = CreateCentralSubface(sortedVertices[1], sortedVertices[3], sortedVertices[5], 2 * blockIndex + 1, 2 * indexInBlock);

            RemoveFaceFromVertices(face, face.Vertices);

            return new List<IcosahedralFace> { southernSubface, westernSubface, easternSubface, centralSubface };
        }

        private IcosahedralFace CreateCentralSubface(Vertex u, Vertex v, Vertex w, int blockIndex, int indexInBlock)
        {
            var vertices = new List<Vertex> { u, v, w };

            var edges = FindEdgesBetween(vertices);

            var newFace = new IcosahedralFace
            {
                BlockIndex = blockIndex,
                IndexInBlock = indexInBlock,
                Vertices = vertices,
                Edges = edges
            };

            return newFace;
        }

        private IcosahedralFace CreateSubface(Vertex u, Vertex v, Vertex w, int blockIndex, int indexInBlock)
        {
            var vertices = new List<Vertex> {u, v, w};

            var edges = FindEdgesBetween(vertices);
            var newEdge = new Edge { Vertices = new List<Vertex> { u, w } };
            u.Edges.Add(newEdge);
            w.Edges.Add(newEdge);
            
            edges.Add(newEdge);
            Edges.Add(newEdge);

            var newFace = new IcosahedralFace
            {
                BlockIndex = blockIndex,
                IndexInBlock = indexInBlock,
                Vertices = vertices,
                Edges = edges
            };

            return newFace;
        }

        private List<Edge> FindEdgesBetween(List<Vertex> vertices)
        {
            var allNeighbouringEdges = vertices.SelectMany(vertex => vertex.Edges).ToList();
            var distinctNeighbouringEdges = allNeighbouringEdges.Distinct();
            var connectingEdges = distinctNeighbouringEdges.Where(e => allNeighbouringEdges.Count(f => (e == f)) > 1).ToList();

            return connectingEdges;
        }

        //BUG: Does not work for shit. Need to fix edge length problem first.
        private bool NorthPointing(IcosahedralFace face)
        {
            var zValues = face.Vertices.Select(vertex => vertex.Position.z);
            var midpoint = (zValues.Max() + zValues.Min())/2;
            var average = zValues.Average();

            return average < midpoint;
        }

        private List<Vertex> FindBoundaryVertices(IcosahedralFace face)
        {
            var allNeighbouringEdges = face.Vertices.SelectMany(vertex => vertex.Edges);
            var allNeighbouringVertices = allNeighbouringEdges.SelectMany(edge => edge.Vertices);
            var distinctNeighbouringVertices = allNeighbouringVertices.Distinct();
            var boundaryVertices = distinctNeighbouringVertices.Where(u => allNeighbouringVertices.Count(v => (u == v)) > 1).ToList();

            return boundaryVertices;
        }

        private List<Edge> FindBoundaryEdges(IcosahedralFace face)
        {
            var allNeighbouringEdges = face.Vertices.SelectMany(vertex => vertex.Edges).ToList();
            var distinctNeighbouringEdges = allNeighbouringEdges.Distinct();
            var boundaryVertices = distinctNeighbouringEdges.Where(e => allNeighbouringEdges.Count(f => (e == f)) > 1).ToList();

            return boundaryVertices;
        }

        private Vector3 CenterOfFace(IcosahedralFace face)
        {
            var vertexPositions = face.Vertices.Select(vertex => vertex.Position).ToList();
            var averageVertexPosition = vertexPositions.Aggregate((u, v) => u + v) / vertexPositions.Count();

            return averageVertexPosition;
        }

        private int MathMod(int x, int m)
        {
            return ((x % m) + m) % m;
        }
    }
}
