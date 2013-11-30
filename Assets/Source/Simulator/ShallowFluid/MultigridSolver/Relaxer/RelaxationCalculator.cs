using System.Linq;
using UnityEditor;

namespace Simulator.ShallowFluid.MultigridSolver.Relaxer
{
    public class RelaxationCalculator<T> : IRelaxationCalculator<T>
    {
        private readonly IGeometry<T> _geometry;

        private readonly ScalarFieldMap<T> _termCoefficients;
        private readonly ScalarField<T> _normalizationCoefficients;

        public RelaxationCalculator(IGeometry<T> geometry)
        {
            _geometry = geometry;
            _termCoefficients = TermCoefficients(geometry.Graph, geometry);
            _normalizationCoefficients = NormalizationCoefficients(geometry.Graph, _termCoefficients);
        }

        private ScalarFieldMap<T> TermCoefficients(Graph<T> graph, IGeometry<T> geometry)
        {
            var nodes = graph.Keys;
            var termCoefficients = new ScalarFieldMap<T>();

            foreach (var node in nodes)
            {
                var neighbours = graph[node];
                var widths = geometry.Widths[node];
                var distances = geometry.InternodeDistances[node];
                var termCoefficientsOfNode = neighbours.ToDictionary(neighbour => neighbour,
                                                                     neighbour => widths[neighbour]/distances[neighbour]);
                termCoefficients.Add(node, new ScalarField<T>(termCoefficientsOfNode));
            }

            return termCoefficients;
        }

        private ScalarField<T> NormalizationCoefficients(Graph<T> graph, ScalarFieldMap<T> termCoefficients)
        {
            var nodes = graph.Keys;
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
            var nodes = _geometry.Graph.Keys;

            foreach (var node in nodes)
            {
                field[node] = RelaxValueAtNode(node, field, laplacianOfField);
            }
        }

        public float RelaxValueAtNode(T node, ScalarField<T> oldField, ScalarField<T> laplacianOfField)
        {
            var newFieldAtNode = 0f;

            var neighbours = _geometry.Graph[node];

            if (neighbours.Count == 0)
            {
                newFieldAtNode = oldField[node];
            }
            else
            {
                foreach (var neighbour in neighbours)
                {
                    newFieldAtNode += _termCoefficients[node][neighbour] * oldField[neighbour];
                }

                newFieldAtNode -= _geometry.Areas[node] * laplacianOfField[node];
                newFieldAtNode /= _normalizationCoefficients[node];    
            }
            
            return newFieldAtNode;
        }
    }
}
