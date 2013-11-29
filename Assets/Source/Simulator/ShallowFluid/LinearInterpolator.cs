using System.Collections.Generic;

namespace Simulator.ShallowFluid
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
