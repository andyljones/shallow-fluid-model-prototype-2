using System.Collections.Generic;
using Foam;
using Grids;
using UnityEngine;

namespace Tests.FoamTests
{
    public class FakeGrid : IGrid
    {
        public List<Face> Faces { get; private set; }

        public FakeGrid()
        {
            // Vertex positions in X-Y plane:
            //    1--3 
            //   /| /
            //  / |/
            // 0--2
            var vertices = new List<Vertex>
            {
                new Vertex {Position = new Vector3(0, 0, 1)},
                new Vertex {Position = new Vector3(1, 1, 1)},
                new Vertex {Position = new Vector3(1, 0, 1)},
                new Vertex {Position = new Vector3(2, 1, 1)}
            };

            // EdgePositions:
            //    --3--
            //   /|  /
            //  0 2 4
            // /  |/  
            //--1--
            var edges = new List<Edge>
            {
                new Edge {Vertices = new List<Vertex> {vertices[0], vertices[1]}},
                new Edge {Vertices = new List<Vertex> {vertices[0], vertices[2]}},
                new Edge {Vertices = new List<Vertex> {vertices[1], vertices[2]}},
                new Edge {Vertices = new List<Vertex> {vertices[1], vertices[3]}},
                new Edge {Vertices = new List<Vertex> {vertices[2], vertices[3]}}
            };

            // Face positions:
            //    -----
            //   /|  /
            //  /0|1/
            // /  |/  
            //-----
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