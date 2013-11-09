using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Atmosphere.MonolayerAtmosphere;
using Foam;
using Grids;
using Grids.GeodesicGridGenerator;
using Grids.IcosahedralGridGenerator;
using Initialization;
using Renderer;
using Renderer.ShallowFluid;
using strange.extensions.injector.impl;
using Surfaces;
using Surfaces.FlatSurface;
using UnityEngine;

public class test : MonoBehaviour {


	// Use this for initialization
	void Start ()
	{
	    var options = new Options
	    {
	        Radius = 10f,
	        Resolution = 2.5f,
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
	    binder.Bind<IAtmosphere>().To<MonolayerAtmosphere>();
	    binder.Bind<IRenderer>().To<ShallowFluidRenderer>();

	    var planetRenderer = binder.GetInstance<IRenderer>() as IRenderer;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
