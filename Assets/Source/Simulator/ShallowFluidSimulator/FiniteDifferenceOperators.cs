using System;
using System.Collections.Generic;
using System.Linq;
using Foam;

namespace Simulator.ShallowFluidSimulator
{
    public static class FiniteDifferenceOperators
    {
        public static float AverageAtEdge(this Dictionary<Cell, float> cellValues, Edge edge)
        {
            return edge.Cells.Average(cell => cellValues[cell]);
        }
    }
}