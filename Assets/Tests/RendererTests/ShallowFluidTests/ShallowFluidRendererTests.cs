using Atmosphere;
using Atmosphere.MonolayerAtmosphere;
using Grids;
using Grids.GeodesicGridGenerator;
using Grids.IcosahedralGridGenerator;
using Initialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renderer;
using Renderer.ShallowFluid;
using strange.extensions.injector.impl;
using Surfaces;
using Surfaces.FlatSurface;

namespace Tests.RendererTests.ShallowFluidTests
{
    [TestClass]
    public class ShallowFluidRendererTests
    {
        private IRenderer _atmoRenderer;

        [TestInitialize]
        public void Create_Shallow_Fluid_Renderer()
        {
            var options = new Options { Radius = 10f, Resolution = 5f, Height = 2f };

            var binder = new InjectionBinder();
            binder.Bind<IMonolayerAtmosphereOptions>().Bind<IFlatSurfaceOptions>().Bind<IIcosahedralGridOptions>().Bind<IShallowFluidRendererOptions>().ToValue(options);
            binder.Bind<IGrid>().To<GeodesicGrid>();
            binder.Bind<ISurface>().To<FlatSurface>();
            binder.Bind<IAtmosphere>().To<MonolayerAtmosphere>();
            binder.Bind<IRenderer>().To<ShallowFluidRenderer>();

            //var atmo = binder.GetInstance<IAtmosphere>() as IAtmosphere;
            _atmoRenderer = binder.GetInstance<IRenderer>() as IRenderer;
        }

        [TestMethod]
        public void Does_Atmo_Renderer_Exist()
        {
            
        }
    }
}