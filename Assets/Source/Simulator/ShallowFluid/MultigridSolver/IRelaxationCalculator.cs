namespace Simulator.ShallowFluid.MultigridSolver
{
    public interface IRelaxationCalculator<T>
    {
        void Relax(ref ScalarField<T> field, ScalarField<T> laplacianOfField);
    }
}
