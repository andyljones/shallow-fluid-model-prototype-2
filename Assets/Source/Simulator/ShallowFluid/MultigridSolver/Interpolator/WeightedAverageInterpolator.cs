using System.Collections.Generic;
using System.Linq;

namespace Simulator.ShallowFluid.MultigridSolver.Interpolator
{
    public class WeightedAverageInterpolator<T> : IInterpolator<T>
    {
        private readonly ScalarFieldMap<T> _weights;

        public WeightedAverageInterpolator(Dictionary<T, VectorField<T>> relativePositionsOfTargetNodesToSourceNodes)
        {
            _weights = CalculateWeights(relativePositionsOfTargetNodesToSourceNodes);
        }

        // Calculates the weights for the interpolation. Weights each neighbour according to 1/distance, then 
        // normalizes so they sum to 1.
        private ScalarFieldMap<T> CalculateWeights(Dictionary<T, VectorField<T>> relativePositions)
        {
            var weights = new ScalarFieldMap<T>();

            foreach (var nodeAndNeighbourPositions in relativePositions)
            {
                var node = nodeAndNeighbourPositions.Key;
                var neighbourPositions = nodeAndNeighbourPositions.Value;

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
