using System.Linq;

namespace Simulator.ShallowFluid.MultigridSolver.Interpolator
{
    public class WeightedAverageInterpolator<T> : IInterpolator<T>
    {
        private readonly ScalarFieldMap<T> _weights;

        public WeightedAverageInterpolator(Graph<T> interpolationGraph, IGeometry<T> fineGeometry)
        {
            _weights = CalculateWeights(interpolationGraph, fineGeometry.RelativePositions);
        }

        // Calculates the weights for the interpolation. Weights each neighbour according to 1/distance, then 
        // normalizes so they sum to 1.
        //TODO: this is gonna fuck up if the coarse nodes aren't immediate neighbours of the fine nodes
        private ScalarFieldMap<T> CalculateWeights(Graph<T> interpolationGraph, VectorFieldMap<T> relativePositions)
        {
            var weights = new ScalarFieldMap<T>();

            foreach (var nodeAndNeighbours in interpolationGraph)
            {
                var node = nodeAndNeighbours.Key;
                var neighbourPositions = relativePositions[node];

                var weightsOfNeighbours =
                    neighbourPositions.ToDictionary(neighbourAndPosition => neighbourAndPosition.Key,
                                                    neighbourAndPosition => Weight(neighbourAndPosition.Value.magnitude));

                var sumOfWeights = weightsOfNeighbours.Values.Sum(); //TODO: Possible overflow.

                var normalizedWeightsOfNeighbours =
                    weightsOfNeighbours.ToDictionary(neighbourAndDistance => neighbourAndDistance.Key,
                                                       neighbourAndDistance => neighbourAndDistance.Value/sumOfWeights);

                weights.Add(node, new ScalarField<T>(normalizedWeightsOfNeighbours));
            }

            return weights;
        }

        private float Weight(float distance)
        {
            float weight;

            if (distance == 0f)
            {
                weight = float.MaxValue;
            }
            else
            {
                weight = 1/distance;
            }

            return weight;
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
