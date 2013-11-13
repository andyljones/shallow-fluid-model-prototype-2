using System.Collections.Generic;
using System.Diagnostics;
using Atmosphere;
using Atmosphere.MonolayerAtmosphere;
using Grids;
using Grids.GeodesicGridGenerator;
using Grids.IcosahedralGridGenerator;
using Initialization;
using Renderer;
using Renderer.ShallowFluid;
using Simulator;
using Simulator.ShallowFluidSimulator;
using strange.extensions.injector.impl;
using Surfaces;
using Surfaces.FlatSurface;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class test : MonoBehaviour {
    private IRenderer _planetRenderer;


    // Use this for initialization
	void Start ()
	{
	    var stopwatch = new Stopwatch();
        stopwatch.Start();

	    var options = new Options
	    {
	        Radius = 6000f,
	        Resolution = 375f,
	        Height = 8f,
            DayLength = 80000,
            Timestep = 50,
	        LayerMaterials = new List<string> {"Materials/OceanWater", "Materials/Sky"},
	        BoundaryMaterial = "Materials/Boundaries",
            ArrowMaterial = "Materials/WindArrows",
            DetailMultiplier = 1.05f
	    };

	    var binder = new InjectionBinder();
	    binder.Bind<IMonolayerAtmosphereOptions>()
	        .Bind<IFlatSurfaceOptions>()
	        .Bind<IIcosahedralGridOptions>()
            .Bind<IShallowFluidSimulatorOptions>()
	        .Bind<IShallowFluidRendererOptions>()
	        .ToValue(options);
	    binder.Bind<IGrid>().To<GeodesicGrid>();
	    binder.Bind<ISurface>().To<FlatSurface>();
	    binder.Bind<IAtmosphere>().To<MonolayerAtmosphere>();
	    binder.Bind<ISimulator>().To<ShallowFluidSimulator>();
	    binder.Bind<IRenderer>().To<ShallowFluidRenderer>();

	    _planetRenderer = binder.GetInstance<IRenderer>() as IRenderer;

	    stopwatch.Stop();
        Debug.Log("TIME: " + stopwatch.ElapsedMilliseconds);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    _planetRenderer.UpdateRender();
	}
}
