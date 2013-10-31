using System.Collections.Generic;
using UnityEngine;

namespace ClimateSim.Grids
{
    public class CompareVerticesClockwiseAround : IComparer<Vertex>
    {
        private readonly CompareVectorsClockwiseAround _vectorComparer;

        public CompareVerticesClockwiseAround(Vector3 vector)
        {
            _vectorComparer = new CompareVectorsClockwiseAround(vector);
        }

        public int Compare(Vertex x, Vertex y)
        {
            return _vectorComparer.Compare(x.Position, y.Position);
        }
    }
} 