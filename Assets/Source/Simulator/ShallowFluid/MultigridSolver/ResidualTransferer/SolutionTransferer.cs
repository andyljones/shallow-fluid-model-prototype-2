namespace Simulator.ShallowFluid.MultigridSolver.ResidualTransferer
{
    public class SolutionTransferer<T> : ISolutionTransferer<T>
    {
        private readonly Graph<T> _coarseGraph; 
        
        public SolutionTransferer(IGeometry<T> geometry)
        {
            _coarseGraph = geometry.Graph;
        }
        
        public void Transfer(ScalarField<T> fineField, ref ScalarField<T> coarseField)
        {
            var coarseNodes = _coarseGraph.Keys;

            foreach (var node in coarseNodes)
            {
                coarseField[node] = fineField[node];
            }
        }
    }
}
