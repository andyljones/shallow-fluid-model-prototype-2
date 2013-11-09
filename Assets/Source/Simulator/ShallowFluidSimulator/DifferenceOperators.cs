using System;
using System.Collections.Generic;
using System.Linq;
using Foam;

namespace Simulator.ShallowFluidSimulator
{
    public static class DifferenceOperators
    {

        public static float AverageAtEdge(this float[] cellScalars, Edge edge)
        {
            return edge.Cells.Average(cell => cellScalars[cell.Index]);
        }

        //public static float JacobianOf(float[] cellScalarsA, float[] cellScalarsB, float[] cellAreas, Cell cell)
        //{
            
        //}
    }
}