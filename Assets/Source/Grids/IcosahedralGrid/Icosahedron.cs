using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClimateSim.Grids.IcosahedralGrid
{   
    //TODO: Add indices to each element so it's easier to test (and delink later on)
    /// <summary>
    /// Generates a unit-radius icosahedron as a list of faces, edges and vertices, all linked to eachother. 
    /// </summary>
    public class Icosahedron
    {
        public List<Vertex> Vertices;

        public List<Edge> Edges;

        public List<IcosahedralFace> Faces;

        public Icosahedron()
        {
            GenerateVertexPositions();
            Edges = GenerateEdgesFromVertices();
            Faces = GenerateFacesFromEdges();

            LinkFacesToVertices();

            LinkVerticesToEdges();
            LinkEdgesAndVerticesToFaces();
        }

        private void GenerateVertexPositions()
        {
            Vertices = Enumerable.Repeat(default(Vertex), 12).ToList();

            // North pole
            Vertices[0] = new Vertex(0) { Position = new Vector3(0, 0, 1)};

            // Northern latitude
            Vertices[1] = new Vertex(1) { Position = new Vector3(xCoordOf(0), yCoordOf(0), 1 / Mathf.Sqrt(5)) };
            Vertices[2] = new Vertex(2) { Position = new Vector3(xCoordOf(2), yCoordOf(2), 1 / Mathf.Sqrt(5)) };
            Vertices[3] = new Vertex(3) { Position = new Vector3(xCoordOf(4), yCoordOf(4), 1 / Mathf.Sqrt(5)) };
            Vertices[4] = new Vertex(4) { Position = new Vector3(xCoordOf(6), yCoordOf(6), 1 / Mathf.Sqrt(5)) };
            Vertices[5] = new Vertex(5) { Position = new Vector3(xCoordOf(8), yCoordOf(8), 1 / Mathf.Sqrt(5)) };

            // SouthernLatitude
            Vertices[6] = new Vertex(6) { Position = new Vector3(xCoordOf(1), yCoordOf(1), -1 / Mathf.Sqrt(5)) };
            Vertices[7] = new Vertex(7) { Position = new Vector3(xCoordOf(3), yCoordOf(3), -1 / Mathf.Sqrt(5)) };
            Vertices[8] = new Vertex(8) { Position = new Vector3(xCoordOf(5), yCoordOf(5), -1 / Mathf.Sqrt(5)) };
            Vertices[9] = new Vertex(9) { Position = new Vector3(xCoordOf(7), yCoordOf(7), -1 / Mathf.Sqrt(5)) };
            Vertices[10] = new Vertex(10) { Position = new Vector3(xCoordOf(9), yCoordOf(9), -1 / Mathf.Sqrt(5)) };

            // South pole
            Vertices[11] = new Vertex(11) {Position = new Vector3(0, 0, -1)};
        }

        private float xCoordOf(int i)
        {
            return 2/Mathf.Sqrt(5)*Mathf.Sin(Mathf.PI/5*i);
        }

        private float yCoordOf(int i)
        {
            return 2/Mathf.Sqrt(5)*Mathf.Cos(Mathf.PI/5*i);
        }

        // Generates the edges in terms of the vertices. This is ugly as sin but frankly doing it procedurally would be a lot uglier.
        private List<Edge> GenerateEdgesFromVertices()
        {
            var edges = Enumerable.Repeat(default(Edge), 30).ToList();

            //Edges from north pole
            edges[0] = new Edge(0) { Vertices = new List<Vertex> { Vertices[0], Vertices[1] } };
            edges[1] = new Edge(1) { Vertices = new List<Vertex> { Vertices[0], Vertices[2] } };
            edges[2] = new Edge(2) { Vertices = new List<Vertex> { Vertices[0], Vertices[3] } };
            edges[3] = new Edge(3) { Vertices = new List<Vertex> { Vertices[0], Vertices[4] } };
            edges[4] = new Edge(4) { Vertices = new List<Vertex> { Vertices[0], Vertices[5] } };

            // Upper latitude
            edges[5] = new Edge(5) { Vertices = new List<Vertex> { Vertices[1], Vertices[2] } };
            edges[6] = new Edge(6) { Vertices = new List<Vertex> { Vertices[2], Vertices[3] } };
            edges[7] = new Edge(7) { Vertices = new List<Vertex> { Vertices[3], Vertices[4] } };
            edges[8] = new Edge(8) { Vertices = new List<Vertex> { Vertices[4], Vertices[5] } };
            edges[9] = new Edge(9) { Vertices = new List<Vertex> { Vertices[5], Vertices[1] } };

            // Middle edges, going down-up-down-up from the prime meridian
            edges[10] = new Edge(10) { Vertices = new List<Vertex> { Vertices[1], Vertices[6] } };
            edges[11] = new Edge(11) { Vertices = new List<Vertex> { Vertices[6], Vertices[2] } };
            edges[12] = new Edge(12) { Vertices = new List<Vertex> { Vertices[2], Vertices[7] } };
            edges[13] = new Edge(13) { Vertices = new List<Vertex> { Vertices[7], Vertices[3] } };
            edges[14] = new Edge(14) { Vertices = new List<Vertex> { Vertices[3], Vertices[8] } };
            edges[15] = new Edge(15) { Vertices = new List<Vertex> { Vertices[8], Vertices[4] } };
            edges[16] = new Edge(16) { Vertices = new List<Vertex> { Vertices[4], Vertices[9] } };
            edges[17] = new Edge(17) { Vertices = new List<Vertex> { Vertices[9], Vertices[5] } };
            edges[18] = new Edge(18) { Vertices = new List<Vertex> { Vertices[5], Vertices[10] } };
            edges[19] = new Edge(19) { Vertices = new List<Vertex> { Vertices[10], Vertices[1] } };

            //Lower latitude
            edges[20] = new Edge(20) { Vertices = new List<Vertex> { Vertices[6], Vertices[7] } };
            edges[21] = new Edge(21) { Vertices = new List<Vertex> { Vertices[7], Vertices[8] } };
            edges[22] = new Edge(22) { Vertices = new List<Vertex> { Vertices[8], Vertices[9] } };
            edges[23] = new Edge(23) { Vertices = new List<Vertex> { Vertices[9], Vertices[10] } };
            edges[24] = new Edge(24) { Vertices = new List<Vertex> { Vertices[10], Vertices[6] } };

            //Edges to south pole
            edges[25] = new Edge(25) { Vertices = new List<Vertex> { Vertices[6], Vertices[11] } };
            edges[26] = new Edge(26) { Vertices = new List<Vertex> { Vertices[7], Vertices[11] } };
            edges[27] = new Edge(27) { Vertices = new List<Vertex> { Vertices[8], Vertices[11] } };
            edges[28] = new Edge(28) { Vertices = new List<Vertex> { Vertices[9], Vertices[11] } };
            edges[29] = new Edge(29) { Vertices = new List<Vertex> { Vertices[10], Vertices[11] } };

            return edges;
        }

        // Generates the faces in terms of the edges. 
        private List<IcosahedralFace> GenerateFacesFromEdges()
        {
            var faces = Enumerable.Repeat(default(IcosahedralFace), 20).ToList(); ;

            // Faces around north pole
            faces[0] = new IcosahedralFace { BlockIndex = 0, IndexInBlock = 0, Edges = new List<Edge> { Edges[0], Edges[1], Edges[5] }};
            faces[1] = new IcosahedralFace { BlockIndex = 1, IndexInBlock = 0, Edges = new List<Edge> { Edges[1], Edges[2], Edges[6] } };
            faces[2] = new IcosahedralFace { BlockIndex = 2, IndexInBlock = 0, Edges = new List<Edge> { Edges[2], Edges[3], Edges[7] } };
            faces[3] = new IcosahedralFace { BlockIndex = 3, IndexInBlock = 0, Edges = new List<Edge> { Edges[3], Edges[4], Edges[8] } };
            faces[4] = new IcosahedralFace { BlockIndex = 4, IndexInBlock = 0, Edges = new List<Edge> { Edges[4], Edges[0], Edges[9] } };

            // Middle faces
            faces[5] = new IcosahedralFace { BlockIndex = 0, IndexInBlock = 1, Edges = new List<Edge> { Edges[5], Edges[10], Edges[11] } };
            faces[6] = new IcosahedralFace { BlockIndex = 1, IndexInBlock = 2, Edges = new List<Edge> { Edges[20], Edges[11], Edges[12] } };
            faces[7] = new IcosahedralFace { BlockIndex = 1, IndexInBlock = 1, Edges = new List<Edge> { Edges[6], Edges[12], Edges[13] } };
            faces[8] = new IcosahedralFace { BlockIndex = 2, IndexInBlock = 2, Edges = new List<Edge> { Edges[21], Edges[13], Edges[14] } };
            faces[9] = new IcosahedralFace { BlockIndex = 2, IndexInBlock = 1, Edges = new List<Edge> { Edges[7], Edges[14], Edges[15] } };
            faces[10] = new IcosahedralFace { BlockIndex = 3, IndexInBlock = 2, Edges = new List<Edge> { Edges[22], Edges[15], Edges[16] } };
            faces[11] = new IcosahedralFace { BlockIndex = 3, IndexInBlock = 1, Edges = new List<Edge> { Edges[8], Edges[16], Edges[17] } };
            faces[12] = new IcosahedralFace { BlockIndex = 4, IndexInBlock = 2, Edges = new List<Edge> { Edges[23], Edges[17], Edges[18] } };
            faces[13] = new IcosahedralFace { BlockIndex = 4, IndexInBlock = 1, Edges = new List<Edge> { Edges[9], Edges[18], Edges[19] } };
            faces[14] = new IcosahedralFace { BlockIndex = 0, IndexInBlock = 2, Edges = new List<Edge> { Edges[24], Edges[19], Edges[10] } };

            // Lower faces
            faces[15] = new IcosahedralFace { BlockIndex = 1, IndexInBlock = 3, Edges = new List<Edge> { Edges[20], Edges[25], Edges[26] } };
            faces[16] = new IcosahedralFace { BlockIndex = 2, IndexInBlock = 3, Edges = new List<Edge> { Edges[21], Edges[26], Edges[27] } };
            faces[17] = new IcosahedralFace { BlockIndex = 3, IndexInBlock = 3, Edges = new List<Edge> { Edges[22], Edges[27], Edges[28] } };
            faces[18] = new IcosahedralFace { BlockIndex = 4, IndexInBlock = 3, Edges = new List<Edge> { Edges[23], Edges[28], Edges[29] } };
            faces[19] = new IcosahedralFace { BlockIndex = 0, IndexInBlock = 3, Edges = new List<Edge> { Edges[24], Edges[29], Edges[25] } };

            return faces;
        }

        // Add to each vertex the edges that are incident to it.
        private void LinkVerticesToEdges()
        {
            foreach (var edge in Edges)
            {
                AddEdgeToVertices(edge, edge.Vertices);
            }
        }

        // Add to each face the vertices adjacent to it.
        private void LinkFacesToVertices()
        {
            foreach (var face in Faces)
            {
                face.Vertices = face.Edges.SelectMany(edge => edge.Vertices).Distinct().ToList();
            }
        }

        // Add to each vertex and edge the faces it's adjacent to.
        private void LinkEdgesAndVerticesToFaces()
        {
            foreach (var face in Faces)
            {
                AddFaceToEdges(face, face.Edges);
                AddFaceToVertices(face, face.Vertices);
            }
        }

        private void AddFaceToEdges(IcosahedralFace face, List<Edge> edges)
        {
            foreach (var edge in edges)
            {
                edge.Faces.Add(face);
            }
        }

        private void AddFaceToVertices(IcosahedralFace face, List<Vertex> vertices)
        {
            foreach (var vertex in vertices)
            {
                vertex.Faces.Add(face);
            }
        }

        private void AddEdgeToVertices(Edge edge, List<Vertex> vertices)
        {
            foreach (var vertex in vertices)
            {
                vertex.Edges.Add(edge);
            }
        }

        private int MathMod(int x, int m)
        {
            return ((x%m) + m)%m;
        }
    }
}
