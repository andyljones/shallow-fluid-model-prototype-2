using System;
using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Simulator.ShallowFluidSimulator
{
    public static class DifferenceOperators
    {

        public static float AverageAtEdge(this FloatFieldInfo cellField, Edge edge)
        {
            return edge.Cells.Average(cell => cellField.GetValue(cell));
        }

        //public static float FluxDivergence(FloatFieldInfo fieldA, FloatFieldInfo fieldB, Cell cell)
        //{
            
        //}
    }
}