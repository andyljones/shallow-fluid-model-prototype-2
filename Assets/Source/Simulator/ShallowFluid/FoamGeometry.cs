using System.Collections.Generic;
using System.Linq;
using Foam;

namespace Simulator.ShallowFluid
{
    public class FoamGeometry : IGeometry<Cell>
    {
        /// <summary>
        /// Field representing the areas of each node.
        /// </summary>
        public ScalarField<Cell> Areas
        {
            get { return _areas ?? (_areas = CalculateAreas(_graph)); }
        }
        private ScalarField<Cell> _areas; 

        /// <summary>
        /// Dictionary of fields; each float gives the width of the face between the two nodes used to index it.
        /// </summary>
        public Dictionary<Cell, ScalarField<Cell>> Widths 
        {
            get { return _widths ?? (_widths = CalculateWidths(_graph)); }
        }
        private Dictionary<Cell, ScalarField<Cell>> _widths;

        /// <summary>
        /// Dictionary of internode distances; each float gives the distance between the two nodes used to index it.
        /// </summary>
        public Dictionary<Cell, ScalarField<Cell>> InternodeDistances
        {
            get { return _distances ?? (_distances = CalculateWidths(_graph)); }
        }
        private Dictionary<Cell, ScalarField<Cell>> _distances; 


        private readonly Graph<Cell> _graph;  

        /// <summary>
        /// Takes a graph of cells in adjacency dictionary format
        /// </summary>
        /// <param name="graph"></param>
        public FoamGeometry(Graph<Cell> graph)
        {
            _graph = graph;
        }

        private ScalarField<Cell> CalculateAreas(Graph<Cell> graph)
        {
            var areas = graph.ToDictionary(cellAndNeighbours => cellAndNeighbours.Key, 
                                           cellAndNeighbours => cellAndNeighbours.Key.HorizontalArea());

            return new ScalarField<Cell>(areas);
        }

        private Dictionary<Cell, ScalarField<Cell>> CalculateWidths(Graph<Cell> graph)
        {
            var widths = new Dictionary<Cell, ScalarField<Cell>>();

            foreach (var cellAndNeighbours in graph)
            {
                var cell = cellAndNeighbours.Key;
                var neighbours = cellAndNeighbours.Value;
                var widthsOfFacesBetweenCellAndNeighbours = new ScalarField<Cell>();

                foreach (var neighbour in neighbours)
                {
                    var face = cell.Faces.Intersect(neighbour.Faces).Single();
                    widthsOfFacesBetweenCellAndNeighbours.Add(neighbour, face.VerticalWidth());
                }

                widths.Add(cell, widthsOfFacesBetweenCellAndNeighbours);
            }

            return widths;
        }


        //TODO: Test.
        private Dictionary<Cell, ScalarField<Cell>> CalculateDistances(Graph<Cell> graph)
        {
            var distances = new Dictionary<Cell, ScalarField<Cell>>();

            foreach (var cellAndNeighbours in graph)
            {
                var cell = cellAndNeighbours.Key;
                var neighbours = cellAndNeighbours.Value;
                var distanceToNeighbours = new ScalarField<Cell>();

                foreach (var neighbour in neighbours)
                {
                    var distance = cell.DistanceTo(neighbour);
                    distanceToNeighbours.Add(neighbour, distance);
                }

                distances.Add(cell, distanceToNeighbours);
            }

            return distances;
        }
    }
}
