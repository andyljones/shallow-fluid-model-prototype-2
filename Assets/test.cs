using System.Linq;
using Atmosphere;
using Atmosphere.MonolayerAtmosphere;
using Foam;
using Grids;
using Grids.IcosahedralGrid;
using Initialization;
using strange.extensions.injector.impl;
using Surfaces;
using Surfaces.FlatSurface;
using UnityEngine;

public class test : MonoBehaviour {

    public static int testDebug()
    {
        return 8;
    }


	// Use this for initialization
	void Start ()
	{
	    var options = new Options {Radius = 10f, Resolution = 1f, Height = 2f};

	    var binder = new InjectionBinder();
	    binder.Bind<IMonolayerAtmosphereOptions>().Bind<IFlatSurfaceOptions>().Bind<IIcosahedralGridOptions>().ToValue(options);
	    binder.Bind<IGrid>().To<IcosahedralGrid>();
	    binder.Bind<ISurface>().To<FlatSurface>();
	    binder.Bind<IAtmosphere>().To<MonolayerAtmosphere>();

	    IAtmosphere atmosphere = binder.GetInstance<IAtmosphere>() as IAtmosphere;

        var obj = new GameObject("test");
        var mesh = obj.AddComponent<MeshFilter>().mesh;
        var renderer = obj.AddComponent<MeshRenderer>();
	    renderer.material = (Material) Resources.Load("OceanWater");

	    var faces = atmosphere.Cells.SelectMany(cell => cell.Faces).Where(face => face.Edges.Count == 3).Distinct().ToList();
	    var vertices = atmosphere.Cells.SelectMany(cell => cell.Vertices).Distinct().ToList();

        Vector3[] vectors = new Vector3[vertices.Count];

        for (int i = 0; i < vertices.Count; i++)
        {
            vectors[i] = vertices[i].Position;
        }

        int[] triangles = new int[3 * faces.Count];

        for (int i = 0; i < faces.Count; i++)
        {
            var face = faces[i];
            var faceVertices = face.Vertices;
            var center = CenterOfFace(face);
            var comparer = new CompareVectorsClockwise(center, new Vector3(0, 0, 1));
            faceVertices = faceVertices.OrderBy(vertex => vertex.Position, comparer).ToList();

            triangles[3 * i] = vertices.IndexOf(faceVertices[0]);
            triangles[3 * i + 1] = vertices.IndexOf(faceVertices[1]);
            triangles[3 * i + 2] = vertices.IndexOf(faceVertices[2]);
        }

        foreach (var edge in faces.SelectMany(face => face.Edges))
	    {
            var lrObj = new GameObject("lr");
            var lr = lrObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.SetWidth(0.05f, 0.05f);
            lr.material = (Material)Resources.Load("Boundaries");
            lr.SetPosition(0, edge.Vertices[0].Position * 1.001f);
            lr.SetPosition(1, edge.Vertices[1].Position * 1.001f);
	    }

        mesh.vertices = vectors;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private Vector3 CenterOfFace(Face face)
    {
        var vertexPositions = face.Vertices.Select(vertex => vertex.Position).ToList();
        var averageVertexPosition = vertexPositions.Aggregate((u, v) => u + v) / vertexPositions.Count();

        return averageVertexPosition;
    }
}
