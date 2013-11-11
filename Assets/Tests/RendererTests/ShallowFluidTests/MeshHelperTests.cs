using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renderer;
using Renderer.ShallowFluid;
using Tests.Fakes;

namespace Tests.RendererTests.ShallowFluidTests
{
    [TestClass]
    public class MeshHelperTests
    {
        private MeshHelper _helper;

        [TestInitialize]
        public void Create_Fake_Atmosphere_And_Mesh_Helper()
        {
            var fakeOptions = A.Fake<IShallowFluidRendererOptions>();
            A.CallTo(() => fakeOptions.DetailMultiplier).Returns(1.5f);

            var fakeAtmo = new FakeAtmosphere();
            _helper = new MeshHelper(fakeAtmo.Cells, fakeOptions);
        }

        [TestMethod]
        public void Boundaries_List_Should_Contain_Two_Arrays()
        {
            var expectedNumber = 2;
            var actualNumber = _helper.Boundaries.Count;

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void First_Element_Of_Boundaries_List_Should_Have_Five_Elements()
        {
            var expected = 5;
            var actual = _helper.Boundaries.First().Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Second_Element_Of_Boundaries_List_Should_Have_Two_Elements()
        {
            var expected = 2;
            var actual = _helper.Boundaries[1].Count();

            Assert.AreEqual(expected, actual);
        }
    }
}