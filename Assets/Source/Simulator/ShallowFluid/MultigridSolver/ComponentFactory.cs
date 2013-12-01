using System;
using System.Collections.Generic;
using System.Linq;
using Simulator.ShallowFluid.MultigridSolver.Interpolator;
using Simulator.ShallowFluid.MultigridSolver.Relaxer;
using Simulator.ShallowFluid.MultigridSolver.ResidualTransferer;

namespace Simulator.ShallowFluid.MultigridSolver
{
    public class ComponentFactory<T> : IComponentFactory<T>
    {
        public List<IGeometry<T>> CoarsenedGeometries { get; private set; }

        public ComponentFactory(List<IGeometry<T>> coarsenedGeometries)
        {
            CoarsenedGeometries = coarsenedGeometries;
        }

    }
}