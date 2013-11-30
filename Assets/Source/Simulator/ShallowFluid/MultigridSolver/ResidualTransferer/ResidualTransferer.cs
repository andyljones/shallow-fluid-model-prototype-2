namespace Simulator.ShallowFluid.MultigridSolver.ResidualTransferer
{
    public class ResidualTransferer<T> : IResidualTransferer<T>
    {
        public Graph<T> CoarseGraph;

        public void Transfer(ScalarField<T> fineField, ref ScalarField<T> coarseField)
        {
            var coarseNodes = CoarseGraph.Keys;

            foreach (var node in coarseNodes)
            {
                coarseField[node] = fineField[node];
            }
        }
    }
}
