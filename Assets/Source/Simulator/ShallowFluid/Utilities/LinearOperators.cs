using System.Linq;
using Foam;
using Simulator.ShallowFluid.Utilities;
using UnityEngine;

namespace Simulator.ShallowFluid
{
    public static class LinearOperators
    {
        //TODO: Test.
        public static ScalarField<T> Laplacian<T>(this ScalarField<T> field, IGeometry<T> geometry)
        {
            var results = new ScalarField<T>(geometry.Graph.Keys);

            foreach (var nodeAndNeighbours in geometry.Graph)
            {
                var node = nodeAndNeighbours.Key;
                var neighbours = nodeAndNeighbours.Value;

                var laplacianAtNode = 0f;

                foreach (var neighbour in neighbours)
                {
                    var widthOverDistance = geometry.Widths[node][neighbour]/
                                            geometry.InternodeDistances[node][neighbour];

                    var differenceInField = field[neighbour] - field[node];

                    laplacianAtNode += widthOverDistance*differenceInField;
                }

                results[node] = laplacianAtNode / geometry.Areas[node];
            }

            return results;
        }

        //TODO: Test.
        public static ScalarField<T> FluxDivergence<T>(this ScalarField<T> fieldA, 
                                                            ScalarField<T> fieldB, 
                                                            IGeometry<T> geometry)
        {
            var results = new ScalarField<T>(geometry.Graph.Keys);

            foreach (var nodeAndNeighbours in geometry.Graph)
            {
                var node = nodeAndNeighbours.Key;
                var neighbours = nodeAndNeighbours.Value;

                var fluxDivergenceAtNode = 0f;

                foreach (var neighbour in neighbours)
                {
                    var widthOverDistance = geometry.Widths[node][neighbour] /
                                            geometry.InternodeDistances[node][neighbour];

                    var sumOfFieldA = fieldA[node] + fieldA[neighbour];
                    var differenceInFieldB = fieldB[neighbour] - fieldB[node];

                    fluxDivergenceAtNode += widthOverDistance * sumOfFieldA * differenceInFieldB;
                }

                results[node] = fluxDivergenceAtNode / (2*geometry.Areas[node]);
            }

            return results;
        }

        //TODO: Test.
        public static ScalarField<Cell> Jacobian(this ScalarField<Cell> fieldA,
                                                      ScalarField<Cell> fieldB,
                                                      IGeometry<Cell> geometry)
        {
            var results = new ScalarField<Cell>(geometry.Graph.Keys);

            foreach (var cell in geometry.Graph.Keys)
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

                results[cell] = jacobianAtCell/(2 * geometry.Areas[cell]);
            }

            return results;
        }

        public static float AverageAboutEdge(Edge edge, ScalarField<Cell> field)
        {
            return edge.Cells.Average(cell => field[cell]);
        }

        //TODO: Test.
        public static VectorField<Cell> Gradient(this ScalarField<Cell> fieldA, IGeometry<Cell> geometry)
        {
            var gradients = new VectorField<Cell>(geometry.Graph.Keys);

            foreach (var cellAndNeighbours in geometry.Graph)
            {
                var cell = cellAndNeighbours.Key;
                var neighbours = cellAndNeighbours.Value;

                var gradientAtNode = new Vector3();

                foreach (var neighbour in neighbours)
                {
                    //TODO: Will only work when face is perpendicular to relative vector
                    var normalToFace = geometry.RelativePositions[cell][neighbour].normalized;
                    var valueAtFace = (fieldA[cell] + fieldA[neighbour])/2;

                    gradientAtNode += valueAtFace*normalToFace;
                }

                gradients[cell] = gradientAtNode;
            }

            return gradients;
        }
    }
}
