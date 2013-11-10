using System.Linq;
using Foam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Fakes;

namespace Tests.SimulatorTests.ShallowFluidSimulatorTests
{
    [TestClass]
    public class DifferenceOperatorTests
    {
        private FakeAtmosphere _fakeAtmo;

        [TestInitialize]
        public void Create_Fake_Atmosphere_And_Simulator()
        {
            _fakeAtmo = new FakeAtmosphere();
        }

        [TestMethod]
        public void Average_At_Edge_Should_Be()
        {
        }
    }
}