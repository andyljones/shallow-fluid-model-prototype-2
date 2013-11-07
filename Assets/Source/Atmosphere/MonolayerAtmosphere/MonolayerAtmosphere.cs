using System.Collections.Generic;
using System.Linq;
using Foam;
using Surfaces;

namespace Atmosphere.MonolayerAtmosphere
{
    public class MonolayerAtmosphere : IAtmosphere
    {
        public List<Cell> Cells { get; private set; }

        public MonolayerAtmosphere(ISurface surface, IMonolayerAtmosphereOptions options)
        {
            var surfaceFaces = surface.Faces.DeepCopy();

            CreateAtmosphereFromSurface(surfaceFaces, options.Height);
        }

        private void CreateAtmosphereFromSurface(List<Face> surfaceFaces, float height)
        {
            Cells = new List<Cell>();

            var bottomFaces = surfaceFaces;
            var bottomToTopFaceDictionary = bottomFaces.DeepCopyDictionary();

            foreach (var facePair in bottomToTopFaceDictionary)
            {
                Cells.Add(CreateCellFromFacePair(facePair));
            }

            SetHeightOfTopFaces(bottomToTopFaceDictionary.Values.ToList(), height);
        }

        private Cell CreateCellFromFacePair(KeyValuePair<Face, Face> facePair)
        {
            var bottomFace = facePair.Key;
            var topFace = facePair.Value;
            var faces = new List<Face> {bottomFace, topFace};

            var bottomEdges = bottomFace.Edges;
            var topEdges = topFace.Edges;
            var edges = bottomEdges.Concat(topEdges).ToList();

            var bottomVertices = bottomFace.Vertices;
            var topVertices = topFace.Vertices;
            var vertices = bottomVertices.Concat(topVertices).ToList();

            return new Cell {Faces = faces, Edges = edges, Vertices = vertices};
        }



        private void SetHeightOfTopFaces(List<Face> topFaces, float height)
        {
            var vertices = topFaces.SelectMany(face => face.Vertices).Distinct();

            foreach (var vertex in vertices)
            {
                var direction = vertex.Position.normalized;
                var radius = vertex.Position.magnitude;

                var newPosition = (radius + height) * direction;

                vertex.Position = newPosition;
            }
        }
    }
}
