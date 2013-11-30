using System.Linq;
using UnityEditor;

namespace Simulator.ShallowFluid.MultigridSolver.Relaxer
{
    public class RelaxationCalculator<T> : IRelaxationCalculator<T>
    {
        public IGeometry<T> Geometry { private get; set; }
        private ScalarFieldMap<T> _termCoefficients;
        private ScalarField<T> _normalizationCoefficients;

        private void InitializeCoefficients()
        {
            _termCoefficients = TermCoefficients(Geometry);
            _normalizationCoefficients = NormalizationCoefficients(_termCoefficients);
        }

        private ScalarFieldMap<T> TermCoefficients(IGeometry<T> geometry)
        {
            var nodes = Geometry.Graph.Keys;
            var termCoefficients = new ScalarFieldMap<T>();

            foreach (var node in nodes)
            {
                var neighbours = Geometry.Graph[node];
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
            var nodes = Geometry.Graph.Keys;
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
            if ((_termCoefficients == null) || (_normalizationCoefficients) == null)
            {
                InitializeCoefficients();
            }

            var nodes = Geometry.Graph.Keys;

            foreach (var node in nodes)
            {
                field[node] = RelaxValueAtNode(node, field, laplacianOfField);
            }
        }

        public float RelaxValueAtNode(T node, ScalarField<T> oldField, ScalarField<T> laplacianOfField)
        {
            var newFieldAtNode = 0f;

            var neighbours = Geometry.Graph[node];

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

                newFieldAtNode -= Geometry.Areas[node] * laplacianOfField[node];
                newFieldAtNode /= _normalizationCoefficients[node];    
            }
            
            return newFieldAtNode;
        }
    }
}
