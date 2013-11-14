using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Foam;
using Renderer.Heightmap;
using Simulator;
using UnityEngine;

namespace Renderer.ShallowFluid
{
    public class ShallowFluidRenderer : IRenderer
    {
        private ISimulator _simulator;
        private List<Cell> _cells;
        private IShallowFluidRendererOptions _options;
        private ArrowRenderer _arrowRenderer;
        private BoundaryRenderer _boundaryRenderer;
        private LayerRenderer _surfaceRenderer;
        private LayerRenderer _atmosphereRenderer;

        public ShallowFluidRenderer(ISimulator simulator, IHeightmap heightmap, IShallowFluidRendererOptions options)
        {
            _simulator = simulator;
            _cells = simulator.Cells;
            _options = options;

            var surfaceFaces = _cells.Select(cell => FoamUtils.BottomFaceOf(cell)).ToList();
            _surfaceRenderer = new LayerRenderer(surfaceFaces, heightmap, options.LayerMaterials[0]);

            var atmosphereFaces = _cells.Select(cell => FoamUtils.TopFaceOf(cell)).ToList();
            _atmosphereRenderer = new LayerRenderer(atmosphereFaces, heightmap, options.LayerMaterials[1],
                boundaryMaterialName: _options.BoundaryMaterial);

            _arrowRenderer = new ArrowRenderer(_cells, heightmap, _options.ArrowLengthMultiplier, _options.ArrowMaterial);
        }

        public void UpdateRender()
        {
            _simulator.StepSimulation();
            _simulator.UpdateCellConditions();

            _atmosphereRenderer.UpdateLayer();

            _arrowRenderer.UpdateArrows();
        }
    }
}