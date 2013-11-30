namespace Simulator.ShallowFluid.MultigridSolver.Relaxer
{
    public interface IRelaxationCalculator<T>
    {
        IGeometry<T> Geometry { set; } 
        void Relax(ref ScalarField<T> field, ScalarField<T> laplacianOfField);
    }
}
