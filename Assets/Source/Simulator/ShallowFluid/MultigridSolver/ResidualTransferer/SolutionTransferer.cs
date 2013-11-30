namespace Simulator.ShallowFluid.MultigridSolver.ResidualTransferer
{
    public class SolutionTransfererFactory<T> : ISolutionTransfererFactory<T>
    {
        public ISolutionTransferer<T> GetSolutionTransferer(IGeometry<T> geometry)
        {
            return new SolutionTransferer<T>(geometry);
        }
    }

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
