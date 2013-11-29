using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tests.SimulatorTests.ShallowFluidTests;

namespace Simulator.ShallowFluid
{
    public class LinearInterpolator<T> : IInterpolator<T>
    {
        private Graph<T> _graph; 
        private RelativePositionCalculator<T> _relativePositionCalculator; 

        public LinearInterpolator(Graph<T> graph, RelativePositionCalculator<T> positionCalculator)
        {
            _graph = graph;
            _relativePositionCalculator = positionCalculator;
        }


        public ScalarField<T> Interpolate(ScalarField<T> field)
        {
            throw new NotImplementedException();
        }

        public float LinearlyInterpolate(T target, List<T> sources, ScalarField<T> field)
        {
            throw new NotImplementedException();
        }

        public float AverageInterpolate(T target, List<T> sources, ScalarField<T> field)
        {
            throw new NotImplementedException();
        }

        public float ConstantInterpolate(T target, List<T> sources, ScalarField<T> field)
        {
            throw new NotImplementedException();
        }
    }
}
