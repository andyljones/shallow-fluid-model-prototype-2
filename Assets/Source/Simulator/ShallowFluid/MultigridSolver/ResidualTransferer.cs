﻿namespace Simulator.ShallowFluid.MultigridSolver
{
    public class ResidualTransferer<T>
    {
        private readonly Graph<T> _coarseGraph;

        public ResidualTransferer(Graph<T> coarseGraph)
        {
            _coarseGraph = coarseGraph;
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
