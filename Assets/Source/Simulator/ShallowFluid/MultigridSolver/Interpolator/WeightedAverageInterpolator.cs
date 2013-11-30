using System.Linq;

namespace Simulator.ShallowFluid.MultigridSolver.Interpolator
{
    public class WeightedAverageInterpolator<T> : IInterpolator<T>
    {
        private readonly ScalarFieldMap<T> _weights;

        public WeightedAverageInterpolator(IGeometry<T> geometry)
        {
            _weights = CalculateWeights(geometry.Graph, geometry.RelativePositions);
        }

        // Calculates the weights for the interpolation. Weights each neighbour according to 1/distance, then 
        // normalizes so they sum to 1.
        private ScalarFieldMap<T> CalculateWeights(Graph<T> graph, VectorFieldMap<T> relativePositions)
        {
            var weights = new ScalarFieldMap<T>();

            foreach (var nodeAndNeighbours in graph)
            {
                var node = nodeAndNeighbours.Key;
                var neighbourPositions = relativePositions[node];

                var weightsOfNeighbours =
                    neighbourPositions.ToDictionary(neighbourAndPosition => neighbourAndPosition.Key,
                                                    neighbourAndPosition => 1/neighbourAndPosition.Value.magnitude); //TODO: Division through by zero here

                var sumOfWeights = weightsOfNeighbours.Values.Sum();

                var normalizedWeightsOfNeighbours =
                    weightsOfNeighbours.ToDictionary(neighbourAndDistance => neighbourAndDistance.Key,
                                                       neighbourAndDistance => neighbourAndDistance.Value/sumOfWeights);

                weights.Add(node, new ScalarField<T>(normalizedWeightsOfNeighbours));
            }

            return weights;
        }

        public void Interpolate(ScalarField<T> sourceField, ref ScalarField<T> targetField)
        {
            foreach (var nodeAndWeightings in _weights)
            {
                var node = nodeAndWeightings.Key;
                var interpolatedValue = InterpolateValueAtNode(node, sourceField);
                targetField[node] = interpolatedValue;
            }
        }

        private float InterpolateValueAtNode(T node, ScalarField<T> field)
        {
            var neighbourWeightings = _weights[node];

            var interpolatedValue = 0f;

            //TODO: See whether the LINQ expression here would be dead slow as you expect.
            foreach (var neighbourAndWeight in neighbourWeightings)
            {
                var neighbour = neighbourAndWeight.Key;
                var weight = neighbourAndWeight.Value;

                interpolatedValue += weight * field[neighbour];
            }

            return interpolatedValue;
        }
    }
}
