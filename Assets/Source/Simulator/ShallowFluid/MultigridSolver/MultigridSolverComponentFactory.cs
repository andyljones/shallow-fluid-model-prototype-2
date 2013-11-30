using System;
using System.Collections.Generic;
using System.Linq;
using Simulator.ShallowFluid.MultigridSolver.Interpolator;
using Simulator.ShallowFluid.MultigridSolver.Relaxer;
using Simulator.ShallowFluid.MultigridSolver.ResidualTransferer;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public class MultigridSolverComponentFactory<T>
    {
        private readonly List<IGeometry<T>> _coarsenedGeometries;

        public MultigridSolverComponentFactory(List<IGeometry<T>> coarsenedGeometries)
        {
            _coarsenedGeometries = coarsenedGeometries;
        }
    }
}