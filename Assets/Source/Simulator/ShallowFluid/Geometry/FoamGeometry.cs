﻿using System.Collections.Generic;
using System.Linq;
using Foam;
using Simulator.ShallowFluid.MultigridSolver;

namespace Simulator.ShallowFluid
{
    public class FoamGeometry : IGeometry<Cell>
    {
        public Graph<Cell> Graph { get; private set; }

        /// <summary>
        /// Field representing the position of each cell.
        /// </summary>
        public VectorField<Cell> Positions
        {
            get { return _positions ?? (_positions = CalculatePositions(Graph)); }
        }
        private VectorField<Cell> _positions; 

        /// <summary>
        /// Field representing the areas of each cell.
        /// </summary>
        public ScalarField<Cell> Areas
        {
            get { return _areas ?? (_areas = CalculateAreas(Graph)); }
        }
        private ScalarField<Cell> _areas; 

        /// <summary>
        /// Dictionary of fields; each float gives the width of the face between the two cells used to index it.
        /// </summary>
        public ScalarFieldMap<Cell> Widths 
        {
            get { return _widths ?? (_widths = CalculateWidths(Graph)); }
        }
        private ScalarFieldMap<Cell> _widths;

        /// <summary>
        /// Dictionary of internode distances; each float gives the distance between the two cells used to index it.
        /// </summary>
        public ScalarFieldMap<Cell> InternodeDistances
        {
            get { return _distances ?? (_distances = CalculateDistances(Graph)); }
        }
        private ScalarFieldMap<Cell> _distances;

        public VectorFieldMap<Cell> RelativePositions
        {
            get { return _relativePositions ?? (_relativePositions = CalculateRelativePositions(Graph)); }
        }
        private VectorFieldMap<Cell> _relativePositions;

        /// <summary>
        /// Takes a graph of cells in adjacency dictionary format
        /// </summary>
        /// <param name="graph"></param>
        public FoamGeometry(Graph<Cell> graph)
        {
            Graph = graph;
        }

        private VectorField<Cell> CalculatePositions(Graph<Cell> graph)
        {
            var cells = graph.Keys;
            var positions = cells.ToDictionary(cell => cell, cell => cell.Center());

            return new VectorField<Cell>(positions);
        }

        private ScalarField<Cell> CalculateAreas(Graph<Cell> graph)
        {
            var cells = graph.Keys;
            var areas = cells.ToDictionary(cell => cell, cell => cell.HorizontalArea());

            return new ScalarField<Cell>(areas);
        }

        private ScalarFieldMap<Cell> CalculateWidths(Graph<Cell> graph)
        {
            var widths = new ScalarFieldMap<Cell>();

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

        private VectorFieldMap<Cell> CalculateRelativePositions(Graph<Cell> graph)
        {
            var calculator = new RelativePositionCalculator<Cell>(graph, Positions);
            return calculator.RelativePositions;
        }

        private ScalarFieldMap<Cell> CalculateDistances(Graph<Cell> graph)
        {
            var distances = new ScalarFieldMap<Cell>();

            foreach (var cellAndNeighbours in graph)
            {
                var cell = cellAndNeighbours.Key;
                var neighbours = cellAndNeighbours.Value;
                var distanceToNeighbours = new ScalarField<Cell>();

                foreach (var neighbour in neighbours)
                {
                    var distance = RelativePositions[cell][neighbour].magnitude;
                    distanceToNeighbours.Add(neighbour, distance);
                }

                distances.Add(cell, distanceToNeighbours);
            }

            return distances;
        }
    }
}
