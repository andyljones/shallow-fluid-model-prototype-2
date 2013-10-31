using UnityEngine;

namespace ClimateSim.Grids.IcosahedralGrid
{
    public class IcosahedralGridGenerator
    {
        public Face[] Faces { get; private set; }


        private float _targetAngularResolution;

        public IcosahedralGridGenerator(IIcosahedralGridOptions options)
        {
            _targetAngularResolution = options.Resolution/options.Radius;
        }
    }
}
