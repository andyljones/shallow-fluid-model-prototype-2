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
        public void Boundaries_List_Should_Contain_One_Array()
        {
            var expectedNumber = 1;
            var actualNumber = _helper.Boundaries.Count;

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void First_Element_Of_Boundaries_List_Should_Have_Six_Elements()
        {
            var expected = 6;
            var actual = _helper.Boundaries.First().Count();

            Assert.AreEqual(expected, actual);
        }
    }
}