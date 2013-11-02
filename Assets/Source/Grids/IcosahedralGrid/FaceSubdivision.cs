using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ClimateSim.Grids.IcosahedralGrid
{
    public class FaceSubdivision
    {
        public List<Vertex> Vertices { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<IcosahedralFace> Faces { get; private set; }  

        public FaceSubdivision(List<IcosahedralFace> oldFaces, List<Edge> edges, List<Vertex> vertices)
        {
            Faces = new List<IcosahedralFace>();
            Edges = edges;
            Vertices = vertices;

            SubdivideFaces(oldFaces);
        }

        private void SubdivideFaces(List<IcosahedralFace> oldFaces)
        {
            foreach (var face in oldFaces)
            {
                SubdivideFace(face);
            }
        }

        //     0
        //    / \    <-- Subface A
        //   5---1
        //  / \ / \  <-- Subfaces C, D, B
        // 4---3---2

        // 2---3---4
        //  \ / \ /     <-- Subfaces B, D, C
        //   1---5
        //    \ /       <-- Subface A
        //     0

        private void SubdivideFace(IcosahedralFace face)
        {
            var zSortedVertices = face.Vertices.OrderBy(vertex => vertex.Position.z);
            var baselineVertex = NorthPointing(face) ? zSortedVertices.Last() : zSortedVertices.First();

            var sortedVertices = SortBoundaryVertices(face, baselineVertex);

            var subfaceA = CreateSubface(sortedVertices[5], sortedVertices[0], sortedVertices[1]);
            var subfaceB = CreateSubface(sortedVertices[1], sortedVertices[2], sortedVertices[3]);
            var subfaceC = CreateSubface(sortedVertices[3], sortedVertices[4], sortedVertices[5]);

            var subfaceD = CreateCentralSubface(sortedVertices[1], sortedVertices[3], sortedVertices[5]);

            RemoveReferencesToFace(face);

            Faces.AddRange(new List<IcosahedralFace> { subfaceA, subfaceB, subfaceC, subfaceD });
        }

        private Vertex[] SortBoundaryVertices(IcosahedralFace face, Vertex baseline)
        {
            var center = CenterOfFace(face);

            var clockwiseFromBaseline = new CompareVectorsClockwise(center, new Vector3(0, 0, 1));
            var clockwiseSortedVertices = face.Vertices.OrderBy(vertex => vertex.Position, clockwiseFromBaseline).ToList();
            var indexOfBaseline = clockwiseSortedVertices.IndexOf(baseline);
            var sortedVertices = new Vertex[clockwiseSortedVertices.Count];

            for (int i = 0; i < sortedVertices.Length; i++)
            {
                sortedVertices[i] = clockwiseSortedVertices[MathMod(indexOfBaseline + i, clockwiseSortedVertices.Count)];
            }

            return sortedVertices;
        }

        private void RemoveReferencesToFace(IcosahedralFace oldFace)
        {
            foreach (var vertex in oldFace.Vertices)
            {
                vertex.Faces.Remove(oldFace);
            }

            foreach (var edge in oldFace.Edges)
            {
                edge.Faces.Remove(oldFace);
            }
        }

        private IcosahedralFace CreateCentralSubface(Vertex u, Vertex v, Vertex w)
        {
            var vertices = new List<Vertex> { u, v, w };

            var newFace = new IcosahedralFace
            {
                Vertices = vertices,
                Edges =  FindEdgesBetween(vertices)
            };

            AddFaceToEdgesAndVertices(newFace);

            return newFace;
        }

        private IcosahedralFace CreateSubface(Vertex u, Vertex v, Vertex w)
        {
            var vertices = new List<Vertex> { u, v, w };

            var edges = FindEdgesBetween(vertices);
            var newEdge = CreateEdge(u, w);

            edges.Add(newEdge);

            var newFace = new IcosahedralFace
            {
                Vertices = vertices,
                Edges = edges
            };

            AddFaceToEdgesAndVertices(newFace);

            return newFace;
        }

        private Edge CreateEdge(Vertex u, Vertex v)
        {
            var newEdge = new Edge { Vertices = new List<Vertex> { u, v } };
            u.Edges.Add(newEdge);
            v.Edges.Add(newEdge);

            Edges.Add(newEdge);

            return newEdge;
        }

        private void AddFaceToEdgesAndVertices(IcosahedralFace face)
        {
            foreach (var edge in face.Edges)
            {
                edge.Faces.Add(face);
            }

            foreach (var vertex in face.Vertices)
            {
                vertex.Faces.Add(face);
            }
        }

        private List<Edge> FindEdgesBetween(List<Vertex> vertices)
        {
            var allNeighbouringEdges = vertices.SelectMany(vertex => vertex.Edges).ToList();
            var distinctNeighbouringEdges = allNeighbouringEdges.Distinct();
            var connectingEdges = distinctNeighbouringEdges.Where(e => allNeighbouringEdges.Count(f => (e == f)) > 1).ToList();

            return connectingEdges;
        }

        private bool NorthPointing(IcosahedralFace face)
        {
            var zValues = face.Vertices.Select(vertex => vertex.Position.z);
            var midpoint = (zValues.Max() + zValues.Min()) / 2;
            var average = zValues.Average();

            return average < midpoint;
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
