using System.Collections.Generic;

namespace Simulator.ShallowFluid
{
    public class LinearInterpolator<T> : IInterpolator<T>
    {
        public LinearInterpolator(ScalarFieldMap<T> fields)
        {
            
        }

        public ScalarField<T> Interpolate(ScalarField<T> field)
        {
            throw new System.NotImplementedException();
        }
    }
}
