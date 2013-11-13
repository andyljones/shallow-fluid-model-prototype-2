using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Simulator.ShallowFluidSimulator
{
    public class CellPreprocessor : IPreprocessor
    {
        public Dictionary<Cell, int> CellIndexDict { get; private set; }
        public int[] CellIndices { get; private set; }

        public int[][] IndicesOfNeighbours { get; private set; }
        public Vector3[] CellCenters { get; private set; }
        public float[] Areas { get; private set; }
        public float[][] Widths { get; private set; }
        public float[][] DistancesBetweenCenters { get; private set; }
        public Vector3[][] NormalsToFaces { get; private set; }

        public CellPreprocessor(List<Cell> cells)
        {
            CellIndexDict = AssignIndicesTo(cells);
            CellIndices = CellIndexDict.Values.ToArray();

            IndicesOfNeighbours = GetIndicesOfNeighboursOf(cells, CellIndexDict);
            CellCenters = CalculateCellCenters(cells, CellIndexDict);
            Areas = CalculateAreasOf(cells, CellIndexDict);
            Widths = CalculateWidthsOfFacesOf(cells, CellIndexDict, IndicesOfNeighbours);
            DistancesBetweenCenters = CalculateDistancesBetweenCenters(cells, CellIndexDict, IndicesOfNeighbours);
            NormalsToFaces = CalculateNormalsToFaces(cells, CellIndexDict, IndicesOfNeighbours);
        }

        //TODO: Test
        private Vector3[] CalculateCellCenters(List<Cell> cells, Dictionary<Cell, int> cellIndexDict)
        {
            var cellCenters = new Vector3[cells.Count];

            foreach (var cell in cells)
            {
                var index = cellIndexDict[cell];
                cellCenters[index] = FoamUtils.CenterOf(cell);
            }

            return cellCenters;
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

        //TODO: This'll only be correct when the vector between neighbouring cells is perpendicular to the face that divides them.
        private Vector3[][] CalculateNormalsToFaces(List<Cell> cells, Dictionary<Cell, int> indicesOfCells, int[][] allNeighbourIndices)
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
