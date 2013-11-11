using System.Linq;
using Foam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator;
using Simulator.ShallowFluidSimulator;
using Tests.Fakes;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidSimulatorTests
{
    [TestClass]
    public class DifferenceOperatorTests
    {
        private DifferenceOperators _operators;

        [TestInitialize]
        public void Create_Fake_Atmosphere_And_Operators()
        {
            var fakeAtmo = new FakeAtmosphere();
            _operators = new DifferenceOperators(fakeAtmo.Cells);
        }
    }
}