using System.Linq;
using ClimateSim.Assets.Source.Grids;
using ClimateSim.Grids;
using ClimateSim.Grids.IcosahedralGrid;
using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var options = new Options() { Radius = 10, Resolution = 5 };

        var grid = new IcosahedralGridGenerator(options);

        var obj = new GameObject("test");
        var mesh = obj.AddComponent<MeshFilter>().mesh;
        var renderer = obj.AddComponent<MeshRenderer>();
	    renderer.material = (Material) Resources.Load("OceanWater");

        Vector3[] vectors = new Vector3[grid.Vertices.Count];

        for (int i = 0; i < grid.Vertices.Count; i++)
        {
            grid.Vertices[i].Index = i;
            vectors[i] = grid.Vertices[i].Position*10;
        }

        int[] triangles = new int[3 * grid.Faces.Count];

        for (int i = 0; i < grid.Faces.Count; i++)
        {
            var face = grid.Faces[i];
            var vertices = face.Vertices;
            var center = CenterOfFace(face);
            var comparer = new CompareVectorsClockwise(center, new Vector3(0, 0, 1));
            vertices = vertices.OrderBy(vertex => vertex.Position, comparer).ToList();

            triangles[3 * i] = vertices[0].Index;
            triangles[3 * i + 1] = vertices[1].Index;
            triangles[3 * i + 2] = vertices[2].Index;
        }

        mesh.vertices = vectors;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private Vector3 CenterOfFace(IcosahedralFace face)
    {
        var vertexPositions = face.Vertices.Select(vertex => vertex.Position).ToList();
        var averageVertexPosition = vertexPositions.Aggregate((u, v) => u + v) / vertexPositions.Count();

        return averageVertexPosition;
    }
}
