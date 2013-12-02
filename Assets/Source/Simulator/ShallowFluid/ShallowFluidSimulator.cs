using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Foam;
using Simulator.ShallowFluid.MultigridSolver;
using UnityEngine;

namespace Simulator.ShallowFluid
{
    public class ShallowFluidSimulator : ISimulator
    {
        public List<Cell> Cells { get; private set; }

        private readonly FoamGeometry _geometry;
        private readonly Solver _solver;
        private bool _initialized = false;

        private ScalarField<Cell> _eta;
        private ScalarField<Cell> _delta;
        private ScalarField<Cell> _h;

        private ScalarField<Cell> _psi;
        private ScalarField<Cell> _chi;

        private List<ScalarField<Cell>> _dEtas;
        private List<ScalarField<Cell>> _dDeltas;
        private List<ScalarField<Cell>> _dHs;

        private float _g = 9.81f;
        private float _f = 2*Mathf.PI/(24*3600);
        private float _t = 300f;

        public ShallowFluidSimulator(IAtmosphere atmosphere)
        {
            Cells = atmosphere.Cells;
            var graph = AdjacencyGraphOf(Cells);
            _geometry = new FoamGeometry(graph);
            _solver = new Solver(graph);
        }

        private Graph<Cell> AdjacencyGraphOf(List<Cell> cells)
        {
            var graph = new Graph<Cell>();

            foreach (var cell in cells)
            {
                graph.Add(cell, cell.Neighbours());
            }

            return graph;
        }

        public void UpdateModel()
        {
            if (!_initialized)
            {
                InitializeFields();
                _initialized = true;
            }
            else
            {
                StepSimulation(AdamsBashforthScheme);   
            }
        }

        public void StepSimulation(UpdateScheme scheme)
        {
            var psiK = (_psi.FluxDivergence(_psi, _geometry).Subtract(_psi.Laplacian(_geometry))).MultiplyBy(0.5f);
            var chiK = (_chi.FluxDivergence(_chi, _geometry).Subtract(_chi.Laplacian(_geometry))).MultiplyBy(0.5f);
            var k = psiK.Add(chiK).Subtract(_psi.Jacobian(_chi, _geometry));
            var kPlusGh = k.Add(_h.MultiplyBy(_g)); 

            var dEta =   _eta.Jacobian(_psi, _geometry).Subtract(_eta.FluxDivergence(_chi, _geometry));
            var dDelta = _eta.Jacobian(_chi, _geometry).Subtract(_eta.FluxDivergence(_psi, _geometry)).Subtract(kPlusGh.Laplacian(_geometry));
            var dH =     _h.Jacobian(_psi, _geometry).Subtract(_h.FluxDivergence(_chi, _geometry));

            _dEtas.Add(dEta);
            _dDeltas.Add(dDelta);
            _dHs.Add(dH);

            _solver.Solve(ref _psi, _eta.Subtract(_f));
            _solver.Solve(ref _chi, _delta);

            _eta = _eta.Add(scheme(ref _dEtas).MultiplyBy(_t));
            _delta = _delta.Add(scheme(ref _dDeltas).MultiplyBy(_t));
            _h = _h.Add(scheme(ref _dHs).MultiplyBy(_t));

        }

        private void InitializeFields()
        {
            _eta = new ScalarField<Cell>(Cells);
            _delta = new ScalarField<Cell>(Cells);
            _h = new ScalarField<Cell>(Cells);

            _psi = new ScalarField<Cell>(Cells);
            _chi = new ScalarField<Cell>(Cells);

            _dEtas = new List<ScalarField<Cell>>();
            _dDeltas = new List<ScalarField<Cell>>();
            _dHs = new List<ScalarField<Cell>>();

            StepSimulation(MatsunoScheme);
            StepSimulation(MatsunoScheme);
        }

        public void UpdateCellConditions()
        {
            var gradPsi = _psi.Gradient(_geometry);
            var gradChi = _chi.Gradient(_geometry);

            foreach (var cell in Cells)
            {
                cell.Height = _h[cell];
                var kCrossGradPsiAtCell = Vector3.Cross(_geometry.Positions[cell].normalized, gradPsi[cell]);
                var gradChiAtCell = gradChi[cell];

                cell.Velocity = kCrossGradPsiAtCell + gradChiAtCell;
            }
        }

        public delegate ScalarField<Cell> UpdateScheme(ref List<ScalarField<Cell>> derivatives);

        public static ScalarField<Cell> MatsunoScheme(ref List<ScalarField<Cell>> derivatives)
        {
            return derivatives.Last();
        }

        public static ScalarField<Cell> AdamsBashforthScheme(ref List<ScalarField<Cell>> derivatives)
        {
            var firstTerm = derivatives[2].MultiplyBy(23f/12f);
            var secondTerm = derivatives[1].MultiplyBy(-16f/12f);
            var thirdTerm = derivatives[0].MultiplyBy(5f/12f);

            derivatives.RemoveAt(0);

            return firstTerm.Add(secondTerm).Add(thirdTerm);
        }
    }
}
