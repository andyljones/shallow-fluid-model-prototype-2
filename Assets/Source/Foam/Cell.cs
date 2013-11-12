using System.Collections.Generic;
using UnityEngine;

namespace Foam
{
    public class Cell
    {
        public List<Face> Faces = new List<Face>();
        public List<Edge> Edges = new List<Edge>();
        public List<Vertex> Vertices = new List<Vertex>();

        //TODO: Move all this to arrays held internally by the simulator. Renderer only cares about height & velocity maps, and they can be passed as a dictionary
        //That destroys the ISimulator interface though doesn't it?
        public float Height;
        public Vector3 Velocity = new Vector3();
    }
}
