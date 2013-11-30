namespace Simulator.ShallowFluid.MultigridSolver
{
    public interface IRelaxationCalculator<T>
    {
        void Relax(out ScalarField<T> U, ScalarField<T> f);
    }
}
