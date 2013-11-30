namespace Simulator.ShallowFluid.MultigridSolver.Interpolator
{
    public interface IInterpolator<T>
    {
        IGeometry<T> Geometry { set; } 
        void Interpolate(ScalarField<T> sourceField, ref ScalarField<T> targetField);
    }
}
