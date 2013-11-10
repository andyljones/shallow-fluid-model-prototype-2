namespace Simulator.ShallowFluidSimulator
{
    public interface IShallowFluidSimulatorOptions
    {
        float Height { get; }
        float DayLength { get; }
        float Timestep { get; }
    }
}
