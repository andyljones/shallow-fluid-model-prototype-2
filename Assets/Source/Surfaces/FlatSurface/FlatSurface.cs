using System.Collections.Generic;
using System.Linq;
using Foam;
using Grids;

namespace Surfaces.FlatSurface
{
    /// <summary>
    /// Takes the provided grid and uses it to initialise a flat surface with radius given by the options argument.
    /// </summary>
    public class FlatSurface : ISurface
    {
        public List<Face> Faces { get; private set; }

        public FlatSurface(IGrid grid, IFlatSurfaceOptions options)
        {
            // Make a copy of the linked structure representing the list.
            var gridCopier = new FoamCopier(grid.Faces);
            Faces = gridCopier.FaceDictionary.Values.ToList();
            
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
