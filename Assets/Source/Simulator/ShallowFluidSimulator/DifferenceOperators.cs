using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Simulator.ShallowFluidSimulator
{
    public class DifferenceOperators
    {
        public CellPreprocessor Preprocessor;

        public DifferenceOperators(List<Cell> cells)
        {
            Preprocessor = new CellPreprocessor(cells);
        }

        public FloatField Jacobian(FloatField A, FloatField B)
        {
            var jacobian = new FloatField(A.Values.Length);

            foreach (int cell in Preprocessor.CellIndices)
            {
                int[] neighbourIndices = Preprocessor.IndicesOfNeighbours[cell];
                float sum = 0;

                for (int j = 0; j < neighbourIndices.Length; j++)
                {
                    var previousNeighbour = neighbourIndices[MathMod(j - 1, neighbourIndices.Length)];
                    var currentNeighbour = neighbourIndices[j];
                    var nextNeighbour = neighbourIndices[MathMod(j + 1, neighbourIndices.Length)];
                    sum += (A[cell] + A[currentNeighbour])*(B[nextNeighbour] - B[previousNeighbour]);
                }

                var jacobianAtPoint = sum/(6*Preprocessor.Areas[cell]);

                jacobian[cell] = jacobianAtPoint;
            }

            return jacobian;
        }

        public FloatField FluxDivergence(FloatField A, FloatField B)
        {
            var fluxDivergence = new FloatField(A.Values.Length);

            foreach (int cell in Preprocessor.CellIndices)
            {
                int[] neighbourIndices = Preprocessor.IndicesOfNeighbours[cell];
                float sum = 0;

                for (int j = 0; j < neighbourIndices.Length; j++)
                {
                    var currentNeighbour = neighbourIndices[j];
                    var widthToDistance = Preprocessor.Widths[cell][j]/Preprocessor.DistancesBetweenCenters[cell][j];
                    sum += widthToDistance * (A[cell] + A[currentNeighbour]) * (B[currentNeighbour] - B[cell]);
                }

                var fluxDivergenceAtPoint = sum / (2 * Preprocessor.Areas[cell]);

                fluxDivergence[cell] = fluxDivergenceAtPoint;
            }

            return fluxDivergence;
        }

        public FloatField Laplacian(FloatField A)
        {
            var laplacian = new FloatField(A.Values.Length);

            foreach (int cell in Preprocessor.CellIndices)
            {
                int[] neighbourIndices = Preprocessor.IndicesOfNeighbours[cell];
                float sum = 0;

                for (int j = 0; j < neighbourIndices.Length; j++)
                {
                    var currentNeighbour = neighbourIndices[j];
                    var widthToDistance = Preprocessor.Widths[cell][j] / Preprocessor.DistancesBetweenCenters[cell][j];
                    sum += widthToDistance * (A[currentNeighbour] - A[cell]);
                }

                var laplacianAtPoint = sum / Preprocessor.Areas[cell];

                laplacian[cell] = laplacianAtPoint;
            }

            return laplacian;
        }

        public FloatField InvertElliptic(FloatField U, FloatField f)
        {
            var newU = new FloatField(U.Values.Length);

            foreach (int cell in Preprocessor.CellIndices)
            {
                int[] neighbourIndices = Preprocessor.IndicesOfNeighbours[cell];
                float sum = 0;
                float sumOfWidthsToDistances = 0;
                float areaTimesF = Preprocessor.Areas[cell]*f[cell];

                for (int j = 0; j < neighbourIndices.Length; j++)
                {
                    var currentNeighbour = neighbourIndices[j];
                    var widthToDistance = Preprocessor.Widths[cell][j] / Preprocessor.DistancesBetweenCenters[cell][j];
                    sum += widthToDistance * U[currentNeighbour] - areaTimesF;
                    sumOfWidthsToDistances += widthToDistance;
                }

                var newUatPoint = sum/sumOfWidthsToDistances;

                newU[cell] = newUatPoint;
            }

            return newU;
        }

        public Vector3[] Gradient(FloatField A)
        {
            var gradient = new Vector3[A.Values.Length];

            foreach (var cell in Preprocessor.CellIndices)
            {
                int[] neighbourIndices = Preprocessor.IndicesOfNeighbours[cell];
                var valueAtCell = A[cell];
                var sum = new Vector3();

                for (int j = 0; j < neighbourIndices.Length; j++)
                {
                    var currentNeighbour = neighbourIndices[j];
                    var valueAtFace = (A[currentNeighbour] + valueAtCell) / 2;
                    var normalToFace = Preprocessor.NormalsToFaces[cell][j];

                    sum += valueAtFace*normalToFace;
                }

                gradient[cell] = sum/Preprocessor.Areas[cell];
            }

            return gradient;
        }

        private int MathMod(int x, int m)
        {
            return ((x%m) + m)%m;
        }
    }

    
}