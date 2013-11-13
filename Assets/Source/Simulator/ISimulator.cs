using System.Collections.Generic;
using Foam;

namespace Simulator
{
    public interface ISimulator
    {
        List<Cell> Cells { get; }

        void StepSimulation();

        void UpdateCellConditions();
    }
}
