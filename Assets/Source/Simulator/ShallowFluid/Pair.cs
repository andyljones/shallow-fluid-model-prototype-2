namespace Simulator.ShallowFluid
{
    public class Pair<T>
    {
        public Pair(T first, T second)
        {
            First = first;
            Second = second;
        }

        public T First { get; private set; }
        public T Second { get; private set; }
    }
}
