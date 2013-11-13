using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Foam;
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

        public ShallowFluidRenderer(ISimulator simulator, IShallowFluidRendererOptions options)
        {
            _simulator = simulator;
            _cells = simulator.Cells;
            _options = options;

            var surfaceFaces = _cells.Select(cell => FoamUtils.BottomFaceOf(cell)).ToList();
            _surfaceRenderer = new LayerRenderer(surfaceFaces, 1, options.LayerMaterials[0]);

            var atmosphereFaces = _cells.Select(cell => FoamUtils.TopFaceOf(cell)).ToList();
            _atmosphereRenderer = new LayerRenderer(atmosphereFaces, options.DetailMultiplier, options.LayerMaterials[1],
                boundaryMaterialName: _options.BoundaryMaterial);

            //_boundaryRenderer = new BoundaryRenderer(_cells, _atmosphereRenderer.Helper, _options.BoundaryMaterial);
            _arrowRenderer = new ArrowRenderer(_cells, _options.DetailMultiplier, _options.ArrowMaterial);
        }

        public void UpdateRender()
        {
            _simulator.StepSimulation();
            _simulator.UpdateCellConditions();

            _atmosphereRenderer.UpdateLayer(.00002f);

            _arrowRenderer.UpdateArrows();
        }
    }
}