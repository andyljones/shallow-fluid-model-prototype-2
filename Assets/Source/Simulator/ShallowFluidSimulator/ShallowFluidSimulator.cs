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

        public ShallowFluidSimulator(IAtmosphere atmosphere)
        {
            Cells = atmosphere.Cells;
        }
    }
}
