using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Foam;

namespace Simulator.ShallowFluidSimulator
{
    public class ShallowFluidSimulator : ISimulator
    {
        public List<Cell> Cells { get; private set; }

        public ShallowFluidSimulator(IAtmosphere atmosphere, IShallowFluidSimulatorOptions options)
        {
            Cells = atmosphere.Cells;
            PreprocessAtmosphere();
            InitializeAtmosphere();
        }

        private void InitializeAtmosphere()
        {
            foreach (var cell in Cells)
            {
                cell.Streamfunction = 0;
                cell.VelocityPotential = 0;
            }
        }

        private void PreprocessAtmosphere()
        {
            PreprocessCells();
            PreprocessFaces();
        }

        private void PreprocessFaces()
        {
            var faces = Cells.SelectMany(cell => cell.VerticalFaces);

            foreach (var face in faces)
            {
                face.Width = FoamUtils.WidthOfVerticalFace(face);
                face.DistanceBetweenFaceCenters = FoamUtils.DistanceBetweenCellCentersAcross(face);
            }
        }

        private void PreprocessCells()
        {
            foreach (var cell in Cells)
            {
                cell.Area = FoamUtils.HorizontalAreaOf(cell);
                cell.VerticalFaces = FoamUtils.FacesWithNeighbours(cell).ToArray();
            }
        }
    }
}
