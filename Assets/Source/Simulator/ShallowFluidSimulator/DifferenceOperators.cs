using System;
using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Simulator.ShallowFluidSimulator
{
    public static class DifferenceOperators
    {

        public static float Jacobian(float[] fieldA, float[] fieldB, int cell, int[] neighbours, float[] areas)
        {
            float sum = 0;

            for (int i = 0; i < neighbours.Length; i++)
            {
                var previousNeighbour = neighbours[MathMod(i - 1, neighbours.Length)];
                var currentNeighbour = neighbours[i];
                var nextNeighbour = neighbours[MathMod(i + 1, neighbours.Length)];

                sum += (fieldA[cell] + fieldA[currentNeighbour])*(fieldB[nextNeighbour] - fieldB[previousNeighbour]);
            }

            return sum/(6*areas[cell]);
        }

        public static float FluxDivergence(float[] fieldA, float[] fieldB, int cell, int[] neighbours, float[] areas, float[][] distancesBetweenCenters, float[][] faceWidths)
        {
            float sum = 0;

            for (int i = 0; i < neighbours.Length; i++)
            {
                var currentNeighbour = neighbours[i];
                var faceWidth = faceWidths[cell][i];
                var distanceBetween = distancesBetweenCenters[cell][i];

                sum += faceWidth / distanceBetween * (fieldA[cell] + fieldA[currentNeighbour]) * (fieldB[currentNeighbour] - fieldB[cell]);
            }

            return sum / (2 * areas[cell]);
        }

        public static float Laplacian(float[] fieldA, int cell, int[] neighbours, float[] areas, float[][] distancesBetweenCenters, float[][] faceWidths)
        {
            float sum = 0;

            for (int i = 0; i < neighbours.Length; i++)
            {
                var currentNeighbour = neighbours[i];
                var faceWidth = faceWidths[cell][i];
                var distanceBetween = distancesBetweenCenters[cell][i];

                sum += faceWidth / distanceBetween * (fieldA[currentNeighbour] - fieldA[cell]);
            }

            return sum / areas[cell];
        }

        public static int MathMod(int x, int m)
        {
            return ((x%m) + m)%m;
        }
    }

    
}