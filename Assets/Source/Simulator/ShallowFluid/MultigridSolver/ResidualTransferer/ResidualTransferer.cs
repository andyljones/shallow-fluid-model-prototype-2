namespace Simulator.ShallowFluid.MultigridSolver.ResidualTransferer
{
    public class ResidualTransferer<T> : IResidualTransferer<T>
    {
        public IGeometry<T> Geometry { private get; set; }

        public void Transfer(ScalarField<T> fineField, ref ScalarField<T> coarseField)
        {
            var coarseNodes = Geometry.Graph.Keys;

            foreach (var node in coarseNodes)
            {
                coarseField[node] = fineField[node];
            }
        }
    }
}
