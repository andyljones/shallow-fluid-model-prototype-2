using System;
using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Foam;
using UnityEngine;

namespace Simulator.ShallowFluidSimulator
{
    public class ShallowFluidSimulator : ISimulator
    {
        public List<Cell> Cells { get; private set; }

        private IShallowFluidSimulatorOptions _options;
        private DifferenceOperators _operators;
        private FloatField _coriolis;
        private FloatField _vorticity;
        private FloatField _divergence;
        private FloatField _velocityPotential;
        private FloatField _streamfunction;
        private FloatField _height;

        public ShallowFluidSimulator(IAtmosphere atmosphere, IShallowFluidSimulatorOptions options)
        {
            Cells = atmosphere.Cells;
            _options = options;
            _operators = new DifferenceOperators(Cells);
            
            InitializeConditions();
        }

        private void InitializeConditions()
        {
            var angularVelocity = 2*Mathf.PI/_options.DayLength;
            _coriolis = new FloatField(Cells.Select(cell => 2 * angularVelocity * FoamUtils.CenterOf(cell).normalized.z).ToArray());
            _vorticity = new FloatField(Cells.Count);
            _divergence = new FloatField(Cells.Count);
            _velocityPotential = new FloatField(Cells.Count);
            _streamfunction = new FloatField(Cells.Count);
            _height = new FloatField(Cells.Select(cell => _options.Height).ToArray());
        }

        public void StepSimulation()
        {
            var derivativeOfVorticity = _operators.Jacobian(_vorticity, _velocityPotential) -
                                        _operators.FluxDivergence(_vorticity, _streamfunction);

            var energy = 0.5f *
                         (_operators.FluxDivergence(_streamfunction, _streamfunction) -
                          _streamfunction*_operators.Laplacian(_streamfunction) +
                          _operators.FluxDivergence(_velocityPotential, _velocityPotential) -
                          _velocityPotential*_operators.Laplacian(_velocityPotential)) +
                          _operators.Jacobian(_streamfunction, _velocityPotential) +
                          9.81f * _height;

            var derivativeOfDivergence = _operators.FluxDivergence(_vorticity, _velocityPotential) +
                                         _operators.Jacobian(_vorticity, _streamfunction) -
                                         _operators.Laplacian(energy);

            var derivativeOfDepth = _operators.Jacobian(_height, _streamfunction) -
                                    _operators.FluxDivergence(_height, _streamfunction);

            _vorticity = _vorticity + _options.Timestep * derivativeOfVorticity;
            _divergence = _divergence + _options.Timestep * derivativeOfDivergence;
            _height = _height + _options.Timestep * derivativeOfDepth;

            _streamfunction = _operators.InvertElliptic(_streamfunction, _vorticity - _coriolis);
            _velocityPotential = _operators.InvertElliptic(_velocityPotential, _divergence);

            var streamfunctionGradient = _operators.Gradient(_streamfunction);
            var velocityPotentialGradient = _operators.Gradient(_velocityPotential);

            foreach (var cell in Cells)
            {
                var cellIndex = _operators.Preprocessor.CellIndexDict[cell];
                var velocity = Vector3.Cross(FoamUtils.CenterOf(cell).normalized, streamfunctionGradient[cellIndex]) +
                               velocityPotentialGradient[cellIndex];
                cell.Velocity = velocity;
            }
        }
    }
}
