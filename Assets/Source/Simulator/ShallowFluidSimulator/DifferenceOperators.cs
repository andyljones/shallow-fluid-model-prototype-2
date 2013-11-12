using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Simulator.ShallowFluidSimulator
{
    public class DifferenceOperators
    {
        public Dictionary<Cell, int> CellIndexDict;
        public int[] CellIndices;

        public int[][] IndicesOfNeighbours;
        public float[] Areas;
        public float[][] Widths;
        public float[][] DistancesBetweenCenters;
        public Vector3[][] NormalsToFaces;

        public DifferenceOperators(List<Cell> cells)
        {
            CellIndexDict = AssignIndicesTo(cells);
            CellIndices = CellIndexDict.Values.ToArray();

            IndicesOfNeighbours = GetIndicesOfNeighboursOf(cells, CellIndexDict);
            Areas = CalculateAreasOf(cells, CellIndexDict);
            Widths = CalculateWidthsOfFacesOf(cells, CellIndexDict, IndicesOfNeighbours);
            DistancesBetweenCenters = CalculateDistancesBetweenCenters(cells, CellIndexDict, IndicesOfNeighbours);
            NormalsToFaces = CalculateNormalsToCenters(cells, CellIndexDict, IndicesOfNeighbours);
        }

        public FloatField Jacobian(FloatField A, FloatField B)
        {
            var jacobian = new FloatField(A.Values.Length);

            foreach (int cell in CellIndices)
            {
                int[] neighbourIndices = IndicesOfNeighbours[cell];
                float sum = 0;

                for (int j = 0; j < neighbourIndices.Length; j++)
                {
                    var previousNeighbour = neighbourIndices[MathMod(j - 1, neighbourIndices.Length)];
                    var currentNeighbour = neighbourIndices[j];
                    var nextNeighbour = neighbourIndices[MathMod(j + 1, neighbourIndices.Length)];
                    sum += (A[cell] + A[currentNeighbour])*(B[nextNeighbour] - B[previousNeighbour]);
                }

                var jacobianAtPoint = sum/(6*Areas[cell]);

                jacobian[cell] = jacobianAtPoint;
            }

            return jacobian;
        }

        public FloatField FluxDivergence(FloatField A, FloatField B)
        {
            var fluxDivergence = new FloatField(A.Values.Length);

            foreach (int cell in CellIndices)
            {
                int[] neighbourIndices = IndicesOfNeighbours[cell];
                float sum = 0;

                for (int j = 0; j < neighbourIndices.Length; j++)
                {
                    var currentNeighbour = neighbourIndices[j];
                    var widthToDistance = Widths[cell][j]/DistancesBetweenCenters[cell][j];
                    sum += widthToDistance * (A[cell] + A[currentNeighbour]) * (B[currentNeighbour] - B[cell]);
                }

                var fluxDivergenceAtPoint = sum / (2 * Areas[cell]);

                fluxDivergence[cell] = fluxDivergenceAtPoint;
            }

            return fluxDivergence;
        }

        public FloatField Laplacian(FloatField A)
        {
            var laplacian = new FloatField(A.Values.Length);

            foreach (int cell in CellIndices)
            {
                int[] neighbourIndices = IndicesOfNeighbours[cell];
                float sum = 0;

                for (int j = 0; j < neighbourIndices.Length; j++)
                {
                    var currentNeighbour = neighbourIndices[j];
                    var widthToDistance = Widths[cell][j] / DistancesBetweenCenters[cell][j];
                    sum += widthToDistance * (A[currentNeighbour] - A[cell]);
                }

                var laplacianAtPoint = sum / Areas[cell];

                laplacian[cell] = laplacianAtPoint;
            }

            return laplacian;
        }

        public FloatField InvertElliptic(FloatField U, FloatField f)
        {
            var newU = new FloatField(U.Values.Length);

            foreach (int cell in CellIndices)
            {
                int[] neighbourIndices = IndicesOfNeighbours[cell];
                float sum = 0;
                float sumOfWidthsToDistances = 0;
                float areaTimesF = Areas[cell]*f[cell];

                for (int j = 0; j < neighbourIndices.Length; j++)
                {
                    var currentNeighbour = neighbourIndices[j];
                    var widthToDistance = Widths[cell][j] / DistancesBetweenCenters[cell][j];
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

            foreach (var cell in CellIndices)
            {
                int[] neighbourIndices = IndicesOfNeighbours[cell];
                var valueAtCell = A[cell];
                var sum = new Vector3();

                for (int j = 0; j < neighbourIndices.Length; j++)
                {
                    var currentNeighbour = neighbourIndices[j];
                    var valueAtFace = (A[currentNeighbour] + valueAtCell) / 2;
                    var normalToFace = NormalsToFaces[cell][j];

                    sum += valueAtFace*normalToFace;
                }

                gradient[cell] = sum/Areas[cell];
            }

            return gradient;
        }

        private int MathMod(int x, int m)
        {
            return ((x%m) + m)%m;
        }

        private int[][] GetIndicesOfNeighboursOf(List<Cell> cells, Dictionary<Cell, int> cellIndices)
        {
            var allNeighbourIndices = new int[cells.Count][];

            foreach (var cell in cells)
            {
                var facesWithNeighbours = FoamUtils.FacesWithNeighbours(cell);

                var center = FoamUtils.CenterOf(cell);
                var baseline = cell.Vertices.First().Position;
                var clockwiseComparer = new CompareVectorsClockwise(center, baseline);
                var sortedFaces = facesWithNeighbours.OrderBy(face => FoamUtils.CenterOf(face), clockwiseComparer);

                var neighbouringCells = sortedFaces.Select(face => FoamUtils.NeighbourAcross(face, cell));
                var indicesOfNeighbouringCells = neighbouringCells.Select(neighbour => cellIndices[neighbour]);

                var index = cellIndices[cell];

                allNeighbourIndices[index] = indicesOfNeighbouringCells.ToArray();
            }

            return allNeighbourIndices;
        }

        private float[][] CalculateWidthsOfFacesOf(List<Cell> cells, Dictionary<Cell, int> indicesOfCells, int[][] allNeighbourIndices)
        {
            var widths = new float[cells.Count][];

            var cellAtIndex = indicesOfCells.ToDictionary(pair => pair.Value, pair => pair.Key);

            foreach (var cell in cells)
            {
                var indicesOfNeighbours = allNeighbourIndices[indicesOfCells[cell]];
                var neighbours = indicesOfNeighbours.Select(neighbourIndex => cellAtIndex[neighbourIndex]);
                var faces = neighbours.Select(neighbour => cell.Faces.Intersect(neighbour.Faces).Single());
                var faceWidths = faces.Select(face => FoamUtils.WidthOfVerticalFace(face));

                var index = indicesOfCells[cell];
                widths[index] = faceWidths.ToArray();
            }

            return widths;
        }

        private float[][] CalculateDistancesBetweenCenters(List<Cell> cells, Dictionary<Cell, int> indicesOfCells, int[][] allNeighbourIndices)
        {
            var allDistances = new float[cells.Count][];

            var cellAtIndex = indicesOfCells.ToDictionary(pair => pair.Value, pair => pair.Key);

            foreach (var cell in cells)
            {
                var cellCenter = FoamUtils.CenterOf(cell);

                var indicesOfNeighbours = allNeighbourIndices[indicesOfCells[cell]];
                var neighbours = indicesOfNeighbours.Select(neighbourIndex => cellAtIndex[neighbourIndex]);
                var centersOfNeighbours = neighbours.Select(neighbour => FoamUtils.CenterOf(neighbour));
                var distancesToNeighbours = centersOfNeighbours.Select((neighbourCenter => (neighbourCenter - cellCenter).magnitude));

                var index = indicesOfCells[cell];
                allDistances[index] = distancesToNeighbours.ToArray();
            }

            return allDistances;
        }

        //TODO: Test
        private Vector3[][] CalculateNormalsToCenters(List<Cell> cells, Dictionary<Cell, int> indicesOfCells, int[][] allNeighbourIndices)
        {
            var allNormals = new Vector3[cells.Count][];

            var cellAtIndex = indicesOfCells.ToDictionary(pair => pair.Value, pair => pair.Key);

            foreach (var cell in cells)
            {
                var cellCenter = FoamUtils.CenterOf(cell);

                var indicesOfNeighbours = allNeighbourIndices[indicesOfCells[cell]];
                var neighbours = indicesOfNeighbours.Select(neighbourIndex => cellAtIndex[neighbourIndex]);
                var centersOfNeighbours = neighbours.Select(neighbour => FoamUtils.CenterOf(neighbour));
                var normalsToNeighbours = centersOfNeighbours.Select((neighbourCenter => (neighbourCenter - cellCenter).normalized));

                var index = indicesOfCells[cell];
                allNormals[index] = normalsToNeighbours.ToArray();
            }

            return allNormals;
        }


        private float[] CalculateAreasOf(List<Cell> cells, Dictionary<Cell, int> indices)
        {
            var areas = new float[cells.Count];

            foreach (var cell in cells)
            {
                var area = FoamUtils.HorizontalAreaOf(cell);
                var index = indices[cell];

                areas[index] = area;
            }

            return areas;
        }

        private Dictionary<Cell, int> AssignIndicesTo(List<Cell> cells)
        {
            var indices = new Dictionary<Cell, int>();

            for (int i = 0; i < cells.Count; i++)
            {
                indices.Add(cells[i], i);
            }

            return indices;
        }
    }

    
}