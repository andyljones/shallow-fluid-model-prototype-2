using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClimateSim.Grids.IcosahedralGrid
{
    public class FaceSubdivision
    {
        public List<IcosahedralFace> Faces { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Vertices { get; private set; }

        private Vector3 _globalNorth = new Vector3(0, 0, 1);


        public FaceSubdivision(IcosahedralFace face)
        {
            if (NorthPointing(face))
            {
                SubdivideNorthPointingFace(face);
            }
            else
            {
                SubdivideSouthPointingFace(face);
            }
        }

        private void SubdivideNorthPointingFace(IcosahedralFace face)
        {
            var center = CalculateCenterOfFace(face);
            var east = Vector3.Cross(center, new Vector3(0, 0, 1));

            var compareClockwiseAroundCenterFromEast = new CompareVectorsClockwise(center, east);
            var sortedVertices = face.Vertices.OrderBy(vertex => vertex.Position, compareClockwiseAroundCenterFromEast).ToList();

            //     A
            //    / \
            //  AC   AB
            //  /     \
            //  C--BC--B
            var A = UpdateVertex(sortedVertices[2], face);
            var B = UpdateVertex(sortedVertices[0], face);
            var C = UpdateVertex(sortedVertices[1], face);

            var AB = VertexBetween(A, B);
            var BC = VertexBetween(B, C);
            var CA = VertexBetween(C, A);

            Vertices = new List<Vertex> {A, B, C, AB, BC, CA};
        }

        private Vertex UpdateVertex(Vertex vertex, IcosahedralFace face)
        {
            var index = 2 * vertex.Index;
            var position = vertex.Position;
            var edges = vertex.Edges.Except(face.Edges).ToList();
            var faces = vertex.Faces.Except(new List<IcosahedralFace> { face }).ToList();

            return new Vertex(index) {Position = position, Faces = faces, Edges = edges};
        }

        private Vertex VertexBetween(Vertex u, Vertex v)
        {
            var index = 2*u.Index + 1; //TODO: This is not unique. 
            var midpoint = (u.Position + v.Position)/2;
            return new Vertex(index) {Position = midpoint};
        }

        private void SubdivideSouthPointingFace(IcosahedralFace face)
        {
            throw new NotImplementedException();
        }
        
        private bool NorthPointing(IcosahedralFace face)
        {
            var zValues = face.Vertices.Select(vertex => vertex.Position.z).ToList();
            var midZ = (zValues.Max() + zValues.Min())/2;

            return zValues.Average() < midZ;
        }

        private Vector3 CalculateCenterOfFace(IcosahedralFace face)
        {
            var vertexPositions = face.Vertices.Select(vertex => vertex.Position).ToList();
            var averageVertexPosition = vertexPositions.Aggregate((u, v) => u + v)/vertexPositions.Count();

            return averageVertexPosition;
        }
    }
}