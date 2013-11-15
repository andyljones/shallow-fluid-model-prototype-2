using System.Collections.Generic;
using Atmosphere;
using Foam;

namespace Simulator.ShallowFluid
{
    class ShallowFluidSimulator : ISimulator
    {
        public List<Cell> Cells { get; private set; }

        public ShallowFluidSimulator(IAtmosphere atmosphere)
        {
            Cells = atmosphere.Cells;
        }

        public void StepSimulation()
        {
            
        }

        public void UpdateCellConditions()
        {
            
        }
    }
}
