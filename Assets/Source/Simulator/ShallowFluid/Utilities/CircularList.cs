using System.Collections.Generic;

namespace Simulator.ShallowFluid.Utilities
{
    public class CircularList<T> : List<T>
    {
        new public T this[int index]
        {
            get
            {
                return base[MathMod(index, Count)];
            }
            set
            {
                base[MathMod(index, Count)] = value;
            }
        }

        private int MathMod(int x, int m)
        {
            return ((x % m) + m) % m;
        }
    }
}
