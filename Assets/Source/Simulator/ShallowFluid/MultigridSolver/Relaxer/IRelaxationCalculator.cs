namespace Simulator.ShallowFluid.MultigridSolver.Relaxer
{
    public interface IRelaxationCalculator<T>
    {
        void Relax(ref ScalarField<T> field, ScalarField<T> laplacianOfField);
    }
}
