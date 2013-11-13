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
        private DifferenceOperators _ops;
        private CellPreprocessor _preprocessor;
        private FloatField _coriolis;

        private FloatField _eta;
        private FloatField _delta;
        
        private FloatField _chi;
        private FloatField _phi;
        private FloatField _h;

        private List<FloatField> latestDEtaDt = new List<FloatField>();
        private List<FloatField> latestDDeltaDt = new List<FloatField>();
        private List<FloatField> latestDhDt = new List<FloatField>();

        public ShallowFluidSimulator(IAtmosphere atmosphere, IShallowFluidSimulatorOptions options)
        {
            Cells = atmosphere.Cells;
            _options = options;
            _preprocessor = new CellPreprocessor(Cells);
            _ops = new DifferenceOperators(_preprocessor);
            
            InitializeConditions();
        }

        private void InitializeConditions()
        {
            var rand = new System.Random();

            var angularVelocity = 2*Mathf.PI/_options.DayLength;
            _coriolis = new FloatField(Cells.Select(cell => 2 * angularVelocity * FoamUtils.CenterOf(cell).normalized.z).ToArray());
            _chi = new FloatField(Cells.Count);
            _phi = new FloatField(Cells.Count);
            _h = new FloatField(Cells.Select(cell => _options.Height + rand.Next(-3, 3)).ToArray());
            _eta = _ops.Laplacian(_phi) + _coriolis;
            _delta = _ops.Laplacian(_chi);

            UpdateCellConditions();

            StepFields(Matsuno);
            StepFields(Matsuno);
        }

        public void StepSimulation()
        {
            StepFields(AdamsBashforth);
        }

        public void UpdateCellConditions()
        {
            var phiGradient = _ops.Gradient(_phi);
            var chiGradient = _ops.Gradient(_chi);

            foreach (var cell in Cells)
            {
                var cellIndex = _preprocessor.CellIndexDict[cell];
                var cellCenter = _preprocessor.CellCenters[cellIndex];
                var velocity = Vector3.Cross(cellCenter.normalized, phiGradient[cellIndex]) +
                               chiGradient[cellIndex];
                cell.Velocity = velocity;
                cell.Height = _h[cellIndex];
            }
        }

        private void StepFields(Solver solver)
        {
            var dEtaDt = _ops.Jacobian(_eta, _phi) - _ops.FluxDivergence(_eta, _chi);

            var energy = 0.5f *
                         (_ops.FluxDivergence(_phi, _phi) - _phi * _ops.Laplacian(_phi) +
                          _ops.FluxDivergence(_chi, _chi) - _chi * _ops.Laplacian(_chi)) -
                          _ops.Jacobian(_phi, _chi) + 0.00981f * _h;

            //Debug.Log(energy.Values.Max());

            var dDeltaDt = _ops.FluxDivergence(_eta, _phi) + _ops.Jacobian(_eta, _chi) - _ops.Laplacian(energy);

            var dHdT = _ops.Jacobian(_h, _phi) - _ops.FluxDivergence(_h, _chi);

            _eta = _eta + _options.Timestep * solver(latestDEtaDt, dEtaDt);
            _delta = _delta + _options.Timestep * solver(latestDDeltaDt, dDeltaDt);
            _h = _h + _options.Timestep * solver(latestDhDt, dHdT);

            _phi = _ops.InvertElliptic(_phi, _eta - _coriolis);
            _chi = _ops.InvertElliptic(_chi, _delta);
        }

        private delegate FloatField Solver(List<FloatField> oldFields, FloatField latestField);

        private FloatField AdamsBashforth(List<FloatField> oldFields, FloatField latestField)
        {
            oldFields.Add(latestField);
            var result = (1/12f)*(23*oldFields[2] - 16*oldFields[1] + 5*oldFields[0]);
            oldFields.Remove(oldFields[0]);

            return result;
        }

        private FloatField Matsuno(List<FloatField> oldFields, FloatField latestField)
        {
            oldFields.Add(latestField);

            return latestField;
        }
    }
}
