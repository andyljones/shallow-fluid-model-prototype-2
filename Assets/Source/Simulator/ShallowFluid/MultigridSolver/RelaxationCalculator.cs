using System;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public class RelaxationCalculator<T> : IRelaxationCalculator<T>
    {
        public RelaxationCalculator(Graph<T> graph, IGeometry<T> geometry)
        {
            
        }

        public void Relax(out ScalarField<T> U, ScalarField<T> f)
        {
            throw new NotImplementedException();
        }
    }
}
