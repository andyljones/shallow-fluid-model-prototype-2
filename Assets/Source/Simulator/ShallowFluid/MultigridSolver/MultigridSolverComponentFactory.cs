using Simulator.ShallowFluid.MultigridSolver.Interpolator;
using Simulator.ShallowFluid.MultigridSolver.Relaxer;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public class MultigridSolverComponentFactory<T, TRelaxer, TInterpolator, TTransferer>
        where TRelaxer : IRelaxationCalculator<T>, new()
        where TInterpolator : IInterpolator<T>, new()
        where TTransferer : IInterpolator<T>, new()
    {
        public MultigridSolverComponentFactory(IGeometry<T> finestGeometry, IGraphCoarsener<T> coarsener)
        {
            
        }
    }
}
