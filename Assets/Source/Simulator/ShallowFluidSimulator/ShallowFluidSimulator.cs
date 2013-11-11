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

        private Dictionary<String, float[]> _fields;
        private DifferenceOperators _operators;

        public ShallowFluidSimulator(IAtmosphere atmosphere)
        {
            Cells = atmosphere.Cells;
            _operators = new DifferenceOperators(Cells);
            Debug.Log(Cells.Count);
        }
    }
}
