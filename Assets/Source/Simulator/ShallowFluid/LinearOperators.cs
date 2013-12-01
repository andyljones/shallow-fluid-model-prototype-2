namespace Simulator.ShallowFluid
{
    public static class LinearOperators
    {
        //TODO: Test.
        public static ScalarField<T> Add<T>(this ScalarField<T> lhs, ScalarField<T> rhs)
        {
            var result = new ScalarField<T>();

            foreach (var node in lhs.Keys)
            {
                result.Add(node, lhs[node] + rhs[node]);
            }

            return result;
        }
        //TODO: Test.
        public static ScalarField<T> Subtract<T>(this ScalarField<T> lhs, ScalarField<T> rhs)
        {
            var result = new ScalarField<T>();

            foreach (var node in lhs.Keys)
            {
                result.Add(node, lhs[node] - rhs[node]);
            }

            return result;
        }

        //TODO: Test.
        public static ScalarField<T> LaplacianWithRespectTo<T>(this ScalarField<T> field, IGeometry<T> geometry)
        {
            var result = new ScalarField<T>();

            foreach (var nodeAndNeighbours in geometry.Graph)
            {
                var node = nodeAndNeighbours.Key;
                var neighbours = nodeAndNeighbours.Value;

                var laplacian = 0f;

                foreach (var neighbour in neighbours)
                {
                    var widthOverDistance = geometry.Widths[node][neighbour]/
                                            geometry.InternodeDistances[node][neighbour];

                    var differenceInField = field[neighbour] - field[node];

                    laplacian += widthOverDistance*differenceInField/geometry.Areas[node];
                }

                result[node] = laplacian;
            }

            return result;
        }
    }
}
