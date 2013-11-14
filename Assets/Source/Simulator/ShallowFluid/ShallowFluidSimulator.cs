using System.Collections.Generic;
using System.Linq;
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

            foreach (var cell in Cells)
            {
                cell.Height = FoamUtils.ThicknessOf(cell);
            }
        }

        public void StepSimulation()
        {
            
        }

        public void UpdateCellConditions()
        {
            
        }
    }
}
