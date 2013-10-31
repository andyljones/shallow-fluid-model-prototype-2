using ClimateSim.Grids.IcosahedralGrid;

using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClimateSim.Tests.Grids.IcosahedralGrid
{
    [TestClass]
    class IcosahedralGridTest
    {
        private IcosahedralGridGenerator _grid;

        [TestInitialize]
        public void Create_Icosahedral_Grid()
        {
            var options = A.Fake<IIcosahedralGridOptions>();
            A.CallTo(() => options.Radius).Returns(1f);
            A.CallTo(() => options.Resolution).Returns(1f);

            var _grid = new IcosahedralGridGenerator(options);
        }
    }
}
