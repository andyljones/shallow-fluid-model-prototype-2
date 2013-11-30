using System.Collections.Generic;
using Foam;
using Surfaces;
using UnityEngine;

namespace Tests.AtmosphereTests.MonolayerAtmosphereTests
{
    public class FakeSurface : ISurface
    {
        public List<Face> Faces { get; private set; }

        public FakeSurface()
        {
            // Vertex positions in X-Z plane:
            //     0
            //    /|
            //   / |
            //  1--2
            //   \ |
            //    \|
            //     3
            var vertices = new List<Vertex>
            {
                new Vertex {Position = new Vector3( 0, 0,  10)},
                new Vertex {Position = new Vector3(-10, 0,  0)},
                new Vertex {Position = new Vector3( 0, 10,  0)},
                new Vertex {Position = new Vector3( 0, 0, -10)}
            };

            // Edge positions in the X-Z plane:
            //
            //    / |
            //   0  1
            //  /   |
            // ---2--
            //  \   |
            //   3  4
            //    \ |
            //      
            var edges = new List<Edge>
            {
                new Edge {Vertices = new List<Vertex> {vertices[0], vertices[1]}},
                new Edge {Vertices = new List<Vertex> {vertices[0], vertices[2]}},
                new Edge {Vertices = new List<Vertex> {vertices[1], vertices[2]}},
                new Edge {Vertices = new List<Vertex> {vertices[1], vertices[3]}},
                new Edge {Vertices = new List<Vertex> {vertices[2], vertices[3]}}
            };

            // Face positions in the X-Z plane:
            //
            //    / |
            //   /  |
            //  / 0 |
            // ------
            //  \ 1 |
            //   \  |
            //    \ |
            //       
            Faces = new List<Face>
            {
                new Face
                {
                    Edges = new List<Edge> {edges[0], edges[1], edges[2]},
                    Vertices = new List<Vertex> {vertices[0], vertices[1], vertices[2]}
                },
                new Face
                {
                    Edges = new List<Edge> {edges[2], edges[3], edges[4]},
                    Vertices = new List<Vertex> {vertices[1], vertices[2], vertices[3]}
                }
            };

            vertices[0].Edges.AddRange(new[] { edges[0], edges[1] });
            vertices[1].Edges.AddRange(new[] { edges[2], edges[3] });
            vertices[2].Edges.AddRange(new[] { edges[2], edges[4] });
            vertices[3].Edges.AddRange(new[] { edges[3], edges[4] });

            vertices[0].Faces.AddRange(new[] { Faces[0] });
            vertices[1].Faces.AddRange(new[] { Faces[0], Faces[1] });
            vertices[2].Faces.AddRange(new[] { Faces[0], Faces[1] });
            vertices[3].Faces.AddRange(new[] { Faces[1] });

            edges[0].Faces.AddRange(new[] { Faces[0] });
            edges[1].Faces.AddRange(new[] { Faces[0] });
            edges[2].Faces.AddRange(new[] { Faces[0], Faces[1] });
            edges[3].Faces.AddRange(new[] { Faces[1] });
            edges[4].Faces.AddRange(new[] { Faces[1] });
        }
    }
}
