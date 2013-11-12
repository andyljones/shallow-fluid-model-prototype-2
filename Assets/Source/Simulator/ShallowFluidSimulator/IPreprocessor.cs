using System.Collections.Generic;
using Foam;
using UnityEngine;

namespace Simulator.ShallowFluidSimulator
{
    public interface IPreprocessor
    {
        Dictionary<Cell, int> CellIndexDict { get; }
        int[] CellIndices { get; }

        int[][] IndicesOfNeighbours { get; }
        float[] Areas { get; }
        float[][] Widths { get; }
        float[][] DistancesBetweenCenters { get; }
        Vector3[][] NormalsToFaces { get; }
    }
}