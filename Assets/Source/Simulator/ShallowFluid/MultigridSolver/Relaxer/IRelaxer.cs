namespace Simulator.ShallowFluid.MultigridSolver.Relaxer
{
    public interface IRelaxer<T>
    {
        void Relax(ref ScalarField<T> field, ScalarField<T> laplacianOfField);
    }
}
