﻿using System.Linq;
using FakeItEasy;
using Foam;
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

        //TODO: Write new tests for this thing
        //TODO: Write tests for boundary renderer
        [TestInitialize]
        public void Create_Fake_Atmosphere_And_Mesh_Helper()
        {
            var fakeOptions = A.Fake<IShallowFluidRendererOptions>();
            A.CallTo(() => fakeOptions.DetailMultiplier).Returns(1.5f);

            var fakeAtmo = new FakeAtmosphere();
            var faces = fakeAtmo.Cells.Select(cell => FoamUtils.TopFaceOf(cell)).Distinct().ToList();
            _helper = new MeshHelper(faces, 1f);
        }
    }
}