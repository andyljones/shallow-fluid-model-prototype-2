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
        public List<IInterpolator<T>> Interpolators
        {
            get
            {
                return _interpolators ?? (_interpolators = CreateInterpolators(_coarsenedGeometries));
            }
        }
        private List<IInterpolator<T>> _interpolators;

        public List<IRelaxationCalculator<T>> Relaxers
        {
            get
            {
                return _relaxers ?? (_relaxers = CreateRelaxers(_coarsenedGeometries));
            }
        }
        private List<IRelaxationCalculator<T>> _relaxers;

        public List<ISolutionTransferer<T>> Transferers
        {
            get
            {
                return _transferers ?? (_transferers = CreateTransferers(_coarsenedGeometries));
            }
        }
        private List<ISolutionTransferer<T>> _transferers;

        private readonly List<IGeometry<T>> _coarsenedGeometries;

        public MultigridSolverComponentFactory(List<IGeometry<T>> coarsenedGeometries)
        {
            _coarsenedGeometries = coarsenedGeometries;
        }

        private List<IInterpolator<T>> CreateInterpolators(List<IGeometry<T>> coarsenedGeometries)
        {
            return coarsenedGeometries.Select(geometry => new WeightedAverageInterpolator<T>(geometry) as IInterpolator<T>).ToList();
        }

        private List<IRelaxationCalculator<T>> CreateRelaxers(List<IGeometry<T>> coarsenedGeometries)
        {
            return coarsenedGeometries.Select(geometry => new RelaxationCalculator<T>(geometry) as IRelaxationCalculator<T>).ToList();
        }

        private List<ISolutionTransferer<T>> CreateTransferers(List<IGeometry<T>> coarsenedGeometries)
        {
            return coarsenedGeometries.Select(geometry => new SolutionTransferer<T>(geometry) as ISolutionTransferer<T>).ToList();

        }

    }
}