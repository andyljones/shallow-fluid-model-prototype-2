namespace Simulator.ShallowFluid.MultigridSolver.Interpolator
{
    public class LinearInterpolator<T> : IInterpolator<T>
    {
        //TODO: Write local coord generator, then write linear interpolator
        public LinearInterpolator(ScalarFieldMap<T> fields)
        {
            
        }

        public ScalarField<T> Interpolate(ScalarField<T> field)
        {
            throw new System.NotImplementedException();
        }
    }
}
