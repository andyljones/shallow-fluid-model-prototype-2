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
using strange.extensions.injector.impl;
using Surfaces;
using Surfaces.FlatSurface;
using Tests.Fakes;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class test : MonoBehaviour {


	// Use this for initialization
	void Start ()
	{
	    var stopwatch = new Stopwatch();
        stopwatch.Start();

	    var options = new Options
	    {
	        Radius = 10f,
	        Resolution = 5f,
	        Height = 1f,
	        LayerMaterials = new List<string> {"Materials/OceanWater", "Materials/Sky"},
	        BoundaryMaterial = "Materials/Boundaries"
	    };

	    var binder = new InjectionBinder();
	    binder.Bind<IMonolayerAtmosphereOptions>()
	        .Bind<IFlatSurfaceOptions>()
	        .Bind<IIcosahedralGridOptions>()
	        .Bind<IShallowFluidRendererOptions>()
	        .ToValue(options);
	    binder.Bind<IGrid>().To<GeodesicGrid>();
	    binder.Bind<ISurface>().To<FlatSurface>();
	    binder.Bind<IAtmosphere>().To<FakeAtmosphere>();
	    binder.Bind<IRenderer>().To<ShallowFluidRenderer>();

	    var planetRenderer = binder.GetInstance<IRenderer>() as IRenderer;

	    stopwatch.Stop();
        Debug.Log("TIME: " + stopwatch.ElapsedMilliseconds);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
