using System.Collections.Generic;
using System.Linq;
using ClimateSim.Grids;

namespace ClimateSim.Surfaces.FlatSurface
{
    /// <summary>
    /// Takes the provided grid and uses it to initialise a flat surface with radius given by the options argument.
    /// </summary>
    public class FlatSurfaceGenerator
    {
        public List<Face> Faces;

        public FlatSurfaceGenerator(IGrid grid, IFlatSurfaceOptions options)
        {
            Faces = grid.Faces.DeepCopy();
            SetVertexRadius(options.Radius);
        }

        private void SetVertexRadius(float radius)
        {
            var vertices = Faces.SelectMany(face => face.Vertices);

            foreach (var vertex in vertices)
            {
                vertex.Position = radius*vertex.Position.normalized;
            }
        }
    }
}
