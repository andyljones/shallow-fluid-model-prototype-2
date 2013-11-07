using System.Linq;
using Grids;
using Initialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using strange.extensions.injector.impl;
using Surfaces;
using Surfaces.FlatSurface;
using Tests.Fakes;
using Tests.GridTests;
using UnityEngine;

namespace Tests.SurfaceTests.FlatSurfaceTests
{
    [TestClass]
    public class FlatSurfaceTests
    {
        private ISurface _surface;

        [TestInitialize]
        public void Create_Flat_Surface()
        {
            var options = new Options {Radius = 10};

            var binder = new InjectionBinder();
            binder.Bind<IGrid>().To<FakeGrid>();
            binder.Bind<IFlatSurfaceOptions>().ToValue(options);
            binder.Bind<ISurface>().To<FlatSurface>();
            _surface = binder.GetInstance<ISurface>() as ISurface;
        }

        [TestMethod]
        public void Surface_Should_Have_Two_Faces()
        {
            var expectedNumberOfFaces = 2;
            var actualNumberOfFaces = _surface.Faces.Count;

            Assert.AreEqual(expectedNumberOfFaces, actualNumberOfFaces);
        }

        [TestMethod]
        public void Vertices_Should_Have_Magnitude_10()
        {
            var expectedMagnitude = 10f;
            var tolerance = 0.1f;
            var vertices = _surface.Faces.SelectMany(face => face.Vertices);
            var magnitudes = vertices.Select(vertex => vertex.Position.magnitude);

            foreach (var magnitude in magnitudes )
            {
                Assert.IsTrue(Mathf.Abs(magnitude - expectedMagnitude) < tolerance);
            }
        }
    }
}