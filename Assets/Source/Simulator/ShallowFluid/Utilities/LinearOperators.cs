using System.Collections.Generic;
using System.Linq;
using Foam;
using Simulator.ShallowFluid.Utilities;
using UnityEngine;

namespace Simulator.ShallowFluid
{
    public class LinearOperators
    {
        private readonly IGeometry<Cell> _geometry;

        public LinearOperators(IGeometry<Cell> geometry)
        {
            _geometry = geometry;
        }

        //TODO: Test.
        public ScalarField<Cell> Laplacian(ScalarField<Cell> field)
        {
            var results = new ScalarField<Cell>(_geometry.Graph.Keys);

            foreach (var nodeAndNeighbours in _geometry.Graph)
            {
                var node = nodeAndNeighbours.Key;
                var neighbours = nodeAndNeighbours.Value;

                var laplacianAtNode = 0f;

                foreach (var neighbour in neighbours)
                {
                    var widthOverDistance = _geometry.Widths[node][neighbour]/
                                            _geometry.InternodeDistances[node][neighbour];

                    var differenceInField = field[neighbour] - field[node];

                    laplacianAtNode += widthOverDistance*differenceInField;
                }

                results[node] = laplacianAtNode / _geometry.Areas[node];
            }

            return results;
        }

        //TODO: Test.
        public ScalarField<Cell> FluxDivergence(ScalarField<Cell> fieldA, ScalarField<Cell> fieldB)
        {
            var results = new ScalarField<Cell>(_geometry.Graph.Keys);

            foreach (var nodeAndNeighbours in _geometry.Graph)
            {
                var node = nodeAndNeighbours.Key;
                var neighbours = nodeAndNeighbours.Value;

                var fluxDivergenceAtNode = 0f;

                foreach (var neighbour in neighbours)
                {
                    var widthOverDistance = _geometry.Widths[node][neighbour] /
                                            _geometry.InternodeDistances[node][neighbour];

                    var sumOfFieldA = fieldA[node] + fieldA[neighbour];
                    var differenceInFieldB = fieldB[neighbour] - fieldB[node];

                    fluxDivergenceAtNode += widthOverDistance * sumOfFieldA * differenceInFieldB;
                }

                results[node] = fluxDivergenceAtNode / (2*_geometry.Areas[node]);
            }

            return results;
        }

        //TODO: Test.
        public ScalarField<Cell> Jacobian(ScalarField<Cell> fieldA, ScalarField<Cell> fieldB)
        {
            var results = new ScalarField<Cell>(_geometry.Graph.Keys);

            foreach (var cell in _geometry.Graph.Keys)
            {
                var sortedEdges = new CircularList<Edge>(cell.VerticalEdges().SortedClockwise());
                var averagesOfB = sortedEdges.ToDictionary(edge => edge, edge => AverageAboutEdge(edge, fieldA));

                var jacobianAtCell = 0f;

                for (int i = 0; i < sortedEdges.Count; i++)
                {
                    var firstEdge = sortedEdges[i];
                    var secondEdge = sortedEdges[i + 1];
                    var faceBetweenEdges = firstEdge.Faces.Intersect(secondEdge.Faces).Single();
                    var neighbour = cell.NeighbourAcross(faceBetweenEdges);

                    jacobianAtCell += (fieldA[cell] + fieldA[neighbour]) * 
                                      (averagesOfB[secondEdge] - averagesOfB[firstEdge]);
                }

                results[cell] = jacobianAtCell/(2 * _geometry.Areas[cell]);
            }

            return results;
        }

        public static float AverageAboutEdge(Edge edge, ScalarField<Cell> field)
        {
            return edge.Cells.Average(cell => field[cell]);
        }

        //TODO: Test.
        public VectorField<Cell> Gradient(ScalarField<Cell> fieldA)
        {
            var gradients = new VectorField<Cell>(_geometry.Graph.Keys);

            foreach (var cellAndNeighbours in _geometry.Graph)
            {
                var cell = cellAndNeighbours.Key;
                var neighbours = cellAndNeighbours.Value;

                var gradientAtNode = new Vector3();

                foreach (var neighbour in neighbours)
                {
                    //TODO: Will only work when face is perpendicular to relative vector
                    var normalToFace = _geometry.RelativePositions[cell][neighbour].normalized;
                    var valueAtFace = (fieldA[cell] + fieldA[neighbour])/2;

                    gradientAtNode += valueAtFace*normalToFace;
                }

                gradients[cell] = gradientAtNode;
            }

            return gradients;
        }
    }
}
