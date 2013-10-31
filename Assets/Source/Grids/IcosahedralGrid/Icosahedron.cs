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
        public Vertex[] Vertices;

        public Edge[] Edges;

        public IcosahedralFace[] Faces;

        public Icosahedron()
        {
            Vertices = GenerateVertexPositions();
            Edges = GenerateEdgesFromVertices();
            Faces = GenerateFacesFromEdges();

            LinkFacesToVertices();

            LinkVerticesToEdges();
            LinkEdgesAndVerticesToFaces();
        }

        //Generates the positions of the 20 vertices
        private Vertex[] GenerateVertexPositions()
        {
            var vertices = new Vertex[12];

            vertices[0] = new Vertex {Position = GenerateVertexPosition(0, 0)};

            for (int j = 0; j < 5; j++)
            {
                vertices[j + 1] = new Vertex {Position = GenerateVertexPosition(1, 2*j)};
                vertices[j + 6] = new Vertex {Position = GenerateVertexPosition(2, 2*j+1)};
            }

            vertices[11] = new Vertex {Position = GenerateVertexPosition(2, 0)};

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Index = i;
            }

            return vertices;
        }

        //There are four distinct latitudes in a icosahedron (indexed 0-3, north to south), and ten distinct meridians 
        //(indexed 0-9) from a prime meridian of the positive y-axis (the y-axis because it's the easiest way to make
        // the right-handed coordinate system I'm used to match the left-handed one Unity uses)
        private Vector3 GenerateVertexPosition(int i, int j)
        {
            var x = Mathf.Sin(i*Mathf.PI/3)*Mathf.Sin(j * 2*Mathf.PI/10);
            var y = Mathf.Sin(i*Mathf.PI/3)*Mathf.Cos(j * 2*Mathf.PI/10);
            var z = Mathf.Cos(i*Mathf.PI/3);

            return new Vector3(x, y, z);
        }

        // Generates the edges in terms of the vertices.
        private Edge[] GenerateEdgesFromVertices()
        {
            var edges = new Edge[30];

            //Edges from north pole
            edges[0] = new Edge { Vertices = new List<Vertex> { Vertices[0], Vertices[1] } };
            edges[1] = new Edge { Vertices = new List<Vertex> { Vertices[0], Vertices[2] } };
            edges[2] = new Edge { Vertices = new List<Vertex> { Vertices[0], Vertices[3] } };
            edges[3] = new Edge { Vertices = new List<Vertex> { Vertices[0], Vertices[4] } };
            edges[4] = new Edge { Vertices = new List<Vertex> { Vertices[0], Vertices[5] } };

            // Upper latitude
            edges[5] = new Edge { Vertices = new List<Vertex> { Vertices[1], Vertices[2] } };
            edges[6] = new Edge { Vertices = new List<Vertex> { Vertices[2], Vertices[3] } };
            edges[7] = new Edge { Vertices = new List<Vertex> { Vertices[3], Vertices[4] } };
            edges[8] = new Edge { Vertices = new List<Vertex> { Vertices[4], Vertices[5] } };
            edges[9] = new Edge { Vertices = new List<Vertex> { Vertices[5], Vertices[1] } };

            // Middle edges, going down-up-down-up from the prime meridian
            edges[10] = new Edge { Vertices = new List<Vertex> { Vertices[1], Vertices[6] } };
            edges[11] = new Edge { Vertices = new List<Vertex> { Vertices[6], Vertices[2] } };
            edges[12] = new Edge { Vertices = new List<Vertex> { Vertices[2], Vertices[7] } };
            edges[13] = new Edge { Vertices = new List<Vertex> { Vertices[7], Vertices[3] } };
            edges[14] = new Edge { Vertices = new List<Vertex> { Vertices[3], Vertices[8] } };
            edges[15] = new Edge { Vertices = new List<Vertex> { Vertices[8], Vertices[4] } };
            edges[16] = new Edge { Vertices = new List<Vertex> { Vertices[4], Vertices[9] } };
            edges[17] = new Edge { Vertices = new List<Vertex> { Vertices[9], Vertices[5] } };
            edges[18] = new Edge { Vertices = new List<Vertex> { Vertices[5], Vertices[10] } };
            edges[19] = new Edge { Vertices = new List<Vertex> { Vertices[10], Vertices[1] } };

            //Lower latitude
            edges[20] = new Edge { Vertices = new List<Vertex> { Vertices[6], Vertices[7] } };
            edges[21] = new Edge { Vertices = new List<Vertex> { Vertices[7], Vertices[8] } };
            edges[22] = new Edge { Vertices = new List<Vertex> { Vertices[8], Vertices[9] } };
            edges[23] = new Edge { Vertices = new List<Vertex> { Vertices[9], Vertices[10] } };
            edges[24] = new Edge { Vertices = new List<Vertex> { Vertices[10], Vertices[6] } };

            //Edges to south pole
            edges[25] = new Edge { Vertices = new List<Vertex> { Vertices[6], Vertices[11] } };
            edges[26] = new Edge { Vertices = new List<Vertex> { Vertices[7], Vertices[11] } };
            edges[27] = new Edge { Vertices = new List<Vertex> { Vertices[8], Vertices[11] } };
            edges[28] = new Edge { Vertices = new List<Vertex> { Vertices[9], Vertices[11] } };
            edges[29] = new Edge { Vertices = new List<Vertex> { Vertices[10], Vertices[11] } };

            for (int i = 0; i < edges.Length; i++)
            {
                edges[i].Index = i;
            }

            return edges;
        }

        // Generates the faces in terms of the edges. 
        private IcosahedralFace[] GenerateFacesFromEdges()
        {
            var faces = new IcosahedralFace[20];

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
