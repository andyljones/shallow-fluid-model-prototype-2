using System.Collections.Generic;
using Atmosphere;
using Foam;
using Surfaces;
using UnityEngine;

namespace Tests.Fakes
{
    public class FakeAtmosphere : IAtmosphere
    {
        public List<Cell> Cells { get; private set; }

        public FakeAtmosphere()
        {
            // Vertex positions in X-Z plane:
            //
            // LOWER VERTICES  UPPER VERTICES
            //        0               4
            //       /|              /|
            //      / |             / |
            //     1--2            5--6
            //      \ |             \ |
            //       \|              \|
            //        3               7
            var vertices = new List<Vertex>
            {   // Lower vertices
                new Vertex {Position = new Vector3( 0, 0,  10)},
                new Vertex {Position = new Vector3(-10, 0,  0)},
                new Vertex {Position = new Vector3( 0, 10,  0)},
                new Vertex {Position = new Vector3( 0, 0, -10)},
                // Upper vertices
                new Vertex {Position = new Vector3( 0, 0,  13)},
                new Vertex {Position = new Vector3(-13, 0,  0)},
                new Vertex {Position = new Vector3( 0, 13,  0)},
                new Vertex {Position = new Vector3( 0, 0, -13)}
            };

            // Edge positions in the X-Z plane:
            //
            // LOWER EDGES  UPPER EDGES   VERTICAL EDGES
            //     / |          / |      connect pairs of 
            //    0  1         5  6     lower/upper vertices
            //   /   |        /   |
            //  ---2--       ---7--
            //   \   |        \   |
            //    3  4         8  9
            //     \ |          \ |
            //                    
            var edges = new List<Edge>
            {
                // Lower edges
                new Edge {Vertices = new List<Vertex> {vertices[0], vertices[1]}},
                new Edge {Vertices = new List<Vertex> {vertices[0], vertices[2]}},
                new Edge {Vertices = new List<Vertex> {vertices[1], vertices[2]}},
                new Edge {Vertices = new List<Vertex> {vertices[1], vertices[3]}},
                new Edge {Vertices = new List<Vertex> {vertices[2], vertices[3]}},
                // Upper edges
                new Edge {Vertices = new List<Vertex> {vertices[4], vertices[5]}},
                new Edge {Vertices = new List<Vertex> {vertices[4], vertices[6]}},
                new Edge {Vertices = new List<Vertex> {vertices[5], vertices[6]}},
                new Edge {Vertices = new List<Vertex> {vertices[5], vertices[7]}},
                new Edge {Vertices = new List<Vertex> {vertices[6], vertices[7]}},
                // Vertical edges
                new Edge {Vertices = new List<Vertex> {vertices[0], vertices[4]}},
                new Edge {Vertices = new List<Vertex> {vertices[1], vertices[5]}},
                new Edge {Vertices = new List<Vertex> {vertices[2], vertices[6]}},
                new Edge {Vertices = new List<Vertex> {vertices[3], vertices[7]}}
            };

            // Face positions in the X-Z plane:
            //
            // LOWER FACES  UPPER FACES
            //      / |          / |
            //     /  |         /  |
            //    / 0 |        / 2 |
            //   ------       ------
            //    \ 1 |        \ 3 |
            //     \  |         \  |
            //      \ |          \ |
            //                     
            var faces = new List<Face>
            {
                // Lower faces
                new Face
                {
                    Edges = new List<Edge> {edges[0], edges[1], edges[2]},
                    Vertices = new List<Vertex> {vertices[0], vertices[1], vertices[2]}
                },
                new Face
                {
                    Edges = new List<Edge> {edges[2], edges[3], edges[4]},
                    Vertices = new List<Vertex> {vertices[1], vertices[2], vertices[3]}
                },
                // Upper faces
                new Face
                {
                    Edges = new List<Edge> {edges[5], edges[6], edges[7]},
                    Vertices = new List<Vertex> {vertices[4], vertices[5], vertices[6]}
                },
                new Face
                {
                    Edges = new List<Edge> {edges[7], edges[8], edges[9]},
                    Vertices = new List<Vertex> {vertices[5], vertices[6], vertices[7]}
                },
                // Vertical faces
                new Face
                {
                    Edges = new List<Edge> {edges[0], edges[5], edges[10], edges[11]},
                    Vertices = new List<Vertex> {vertices[0], vertices[1], vertices[4], vertices[5]}
                },
                new Face
                {
                    Edges = new List<Edge> {edges[1], edges[6], edges[10], edges[12]},
                    Vertices = new List<Vertex> {vertices[0], vertices[2], vertices[4], vertices[6]}
                },
                new Face
                {
                    Edges = new List<Edge> {edges[2], edges[7], edges[11], edges[12]},
                    Vertices = new List<Vertex> {vertices[1], vertices[2], vertices[5], vertices[6]}
                },
                new Face
                {
                    Edges = new List<Edge> {edges[3], edges[8], edges[11], edges[13]},
                    Vertices = new List<Vertex> {vertices[1], vertices[3], vertices[5], vertices[7]}
                },
                new Face
                {
                    Edges = new List<Edge> {edges[4], edges[9], edges[12], edges[13]},
                    Vertices = new List<Vertex> {vertices[2], vertices[3], vertices[6], vertices[7]}
                }
            };

            vertices[0].Edges.AddRange(new[] { edges[0], edges[1], edges[10] });
            vertices[1].Edges.AddRange(new[] { edges[2], edges[3], edges[11] }); // Wrong
            vertices[2].Edges.AddRange(new[] { edges[2], edges[4], edges[12] }); // Wrong.
            vertices[3].Edges.AddRange(new[] { edges[3], edges[4], edges[13] });
            vertices[4].Edges.AddRange(new[] { edges[5], edges[6], edges[10] });
            vertices[5].Edges.AddRange(new[] { edges[7], edges[8], edges[11] });
            vertices[6].Edges.AddRange(new[] { edges[7], edges[9], edges[12] });
            vertices[7].Edges.AddRange(new[] { edges[8], edges[9], edges[13] });

            vertices[0].Faces.AddRange(new[] { faces[0], faces[4], faces[5] });
            vertices[1].Faces.AddRange(new[] { faces[0], faces[1], faces[4], faces[6], faces[7] }); // Wrong?
            vertices[2].Faces.AddRange(new[] { faces[0], faces[1] }); // Wrong.
            vertices[3].Faces.AddRange(new[] { faces[1] });

            edges[0].Faces.AddRange(new[] { faces[0] });
            edges[1].Faces.AddRange(new[] { faces[0] });
            edges[2].Faces.AddRange(new[] { faces[0], faces[1] });
            edges[3].Faces.AddRange(new[] { faces[1] });
            edges[4].Faces.AddRange(new[] { faces[1] });
        }
    }
}
