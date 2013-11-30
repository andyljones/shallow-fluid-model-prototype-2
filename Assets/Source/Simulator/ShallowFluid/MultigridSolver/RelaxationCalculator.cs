using System;
using System.Linq;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public class RelaxationCalculator<T> : IRelaxationCalculator<T>
    {
        private readonly Graph<T> _graph;
        private readonly ScalarField<T> _areas;
        private readonly ScalarField<T> _normalizationCoefficients;
        private readonly ScalarFieldMap<T> _termCoefficients;

        public RelaxationCalculator(Graph<T> graph, IGeometry<T> geometry)
        {
            _graph = graph;
            _areas = geometry.Areas;
            _termCoefficients = TermCoefficients(geometry);
            _normalizationCoefficients = NormalizationCoefficients(_termCoefficients);
        }

        private ScalarFieldMap<T> TermCoefficients(IGeometry<T> geometry)
        {
            var nodes = _graph.Keys;
            var termCoefficients = new ScalarFieldMap<T>();

            foreach (var node in nodes)
            {
                var neighbours = _graph[node];
                var widths = geometry.Widths[node];
                var distances = geometry.InternodeDistances[node];
                var termCoefficientsOfNode = neighbours.ToDictionary(neighbour => neighbour,
                                                                     neighbour => widths[neighbour]/distances[neighbour]);
                termCoefficients.Add(node, new ScalarField<T>(termCoefficientsOfNode));
            }

            return termCoefficients;
        }

        private ScalarField<T> NormalizationCoefficients(ScalarFieldMap<T> termCoefficients)
        {
            var nodes = _graph.Keys;
            var normalizationCoefficients = new ScalarField<T>();

            foreach (var node in nodes)
            {
                var normalizationCoefficient = termCoefficients[node].Values.Sum();
                normalizationCoefficients.Add(node, normalizationCoefficient);
            }

            return new ScalarField<T>(normalizationCoefficients);
        }

        public void Relax(ref ScalarField<T> field, ScalarField<T> laplacianOfField)
        {
            var nodes = _graph.Keys;

            foreach (var node in nodes)
            {
                field[node] = RelaxValueAtNode(node, field, laplacianOfField);
            }
        }

        public float RelaxValueAtNode(T node, ScalarField<T> field, ScalarField<T> laplacianOfField)
        {
            var sum = 0f;

            var neighbours = _graph[node];

            foreach (var neighbour in neighbours)
            {
                sum += _termCoefficients[node][neighbour]*field[neighbour];
            }

            sum -= _areas[node]*laplacianOfField[node];
            sum /= _normalizationCoefficients[node];

            return sum;
        }
    }
}
