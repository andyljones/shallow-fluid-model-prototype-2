using System.Collections.Generic;
using System.Linq;

namespace Simulator.ShallowFluid.MultigridSolver.ResidualTransferer
{
    public class SolutionTransferer<T> : ISolutionTransferer<T>
    {
        private readonly IEnumerable<T> _coarseNodes; 
        
        public SolutionTransferer(Graph<T> interpolationGraph)
        {
            _coarseNodes = interpolationGraph.Values.SelectMany(coarseNodeList => coarseNodeList).Distinct();
        }
        
        public void Transfer(ScalarField<T> fineField, ref ScalarField<T> coarseField)
        {
            foreach (var node in _coarseNodes)
            {
                coarseField[node] = fineField[node];
            }
        }
    }
}
