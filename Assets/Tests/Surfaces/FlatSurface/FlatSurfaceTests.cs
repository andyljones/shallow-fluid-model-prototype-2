using System.Linq;
using ClimateSim.Assets.Source.Grids;
using ClimateSim.Grids;
using ClimateSim.Surfaces.FlatSurface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using UnityEngine;

namespace ClimateSim.Tests.Surfaces.FlatSurface
{
    [TestClass]
    public class FlatSurfaceTests
    {
        private FlatSurfaceGenerator _surface;

        [TestInitialize]
        public void Create_Flat_Surface()
        {
            var options = new Options {Radius = 10};

            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind<IGrid>().To<FakeGrid>();
                kernel.Bind<IFlatSurfaceOptions>().To<Options>().WithPropertyValue("Radius", 10f);
                _surface = kernel.Get<FlatSurfaceGenerator>();
            }
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