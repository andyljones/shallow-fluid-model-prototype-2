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
	    var options = new Options {Radius = 10f, Resolution = 10f, Height = 1f};

	    var binder = new InjectionBinder();
	    binder.Bind<IMonolayerAtmosphereOptions>().Bind<IFlatSurfaceOptions>().Bind<IIcosahedralGridOptions>().Bind<IShallowFluidRendererOptions>().ToValue(options);
	    binder.Bind<IGrid>().To<GeodesicGrid>();
	    binder.Bind<ISurface>().To<FlatSurface>();
	    binder.Bind<IAtmosphere>().To<MonolayerAtmosphere>(); //TODO: work out how singletons work
	    binder.Bind<IRenderer>().To<ShallowFluidRenderer>();

	    var planetRenderer = binder.GetInstance<IRenderer>() as IRenderer;

        var atmoObj = new GameObject("atmo");
        var mesh = atmoObj.AddComponent<MeshFilter>().mesh;
        var meshRenderer = atmoObj.AddComponent<MeshRenderer>();
        meshRenderer.material = (Material)Resources.Load("Sky");

	    mesh.vertices = planetRenderer.Vectors;
	    mesh.triangles = planetRenderer.AtmosphereTriangles;
        mesh.RecalculateNormals();

        var surfObj = new GameObject("atmo");
        var surfmesh = surfObj.AddComponent<MeshFilter>().mesh;
        var surfmeshRenderer = surfObj.AddComponent<MeshRenderer>();
        surfmeshRenderer.material = (Material)Resources.Load("OceanWater");

        surfmesh.vertices = planetRenderer.Vectors;
        surfmesh.triangles = planetRenderer.SurfaceTriangles;
        surfmesh.RecalculateNormals();


	    var atmo = binder.GetInstance<IAtmosphere>() as IAtmosphere;
	    var faces = atmo.Cells.SelectMany(cell => cell.Faces).Where(face => face.Edges.Count != 4).Distinct().ToList();

        foreach (var edge in faces.SelectMany(face => face.Edges).Distinct())
        {
            var lrObj = new GameObject("lr");
            var lr = lrObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.SetWidth(0.05f, 0.05f);
            lr.material = (Material)Resources.Load("Boundaries");
            lr.SetPosition(0, edge.Vertices[0].Position * 1.001f);
            lr.SetPosition(1, edge.Vertices[1].Position * 1.001f);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
