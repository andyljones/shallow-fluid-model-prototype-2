using System.Collections.Generic;
using System.Linq;
using Foam;

namespace Simulator.ShallowFluidSimulator
{
    public class DifferenceOperators
    {
        public Dictionary<Cell, int> IndicesOfCells;

        public int[][] IndicesOfNeighbours;
        public float[] Areas;
        public float[][] Widths;
        public float[][] DistancesBetweenCenters;

        public DifferenceOperators(List<Cell> cells)
        {
            IndicesOfCells = AssignIndicesTo(cells);
            IndicesOfNeighbours = GetIndicesOfNeighboursOf(cells, IndicesOfCells);
            Areas = CalculateAreasOf(cells, IndicesOfCells);
            Widths = CalculateWidthsOfFacesOf(cells, IndicesOfCells, IndicesOfNeighbours);
            DistancesBetweenCenters = CalculateDistancesBetweenCenters(cells, IndicesOfCells, IndicesOfNeighbours);
        }

        //public FloatField Jacobian(FloatField A, FloatField B)
        //{
            

        //}

        //public FloatField FluxDivergence(FloatField A, FloatField B)
        //{
            
        //}

        //public FloatField Laplacian(FloatField A)
        //{
            
        //}

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