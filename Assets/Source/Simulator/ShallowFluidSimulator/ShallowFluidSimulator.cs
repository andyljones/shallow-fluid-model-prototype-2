using System;
using System.Collections.Generic;
using System.Linq;
using Atmosphere;
using Foam;
using UnityEngine;

namespace Simulator.ShallowFluidSimulator
{
    public class ShallowFluidSimulator : ISimulator
    {
        public List<Cell> Cells { get; private set; }

        private Dictionary<Cell, int> _cellIndexDictionary;
        private int[][] _neighbourIndices;
        private float[][] _faceWidths;
        private float[][] _distancesBetweenCenters;
        private Vector3[] _localNormals;
        private float[] _areas;
        private float[] _f;

        private float[,] _dEtaDt;
        private float[,] _dDeltaDt;
        private float[,] _dHDt;

        private float[] _phi;
        private float[] _chi;
        private float[] _eta;
        private float[] _delta;
        private float[] _h;

        public ShallowFluidSimulator(IAtmosphere atmosphere, IShallowFluidSimulatorOptions options)
        {
            Cells = atmosphere.Cells;
            PreprocessAtmosphere();
            InitializeAtmosphere(options.Height);
        }

        private void InitializeAtmosphere(float height)
        {
            _dEtaDt = new float[Cells.Count, 3];
            _dDeltaDt = new float[Cells.Count, 3];
            _dHDt = new float[Cells.Count, 3];

            _phi = new float[Cells.Count];
            _chi = new float[Cells.Count];
            _eta = new float[Cells.Count];
            _delta = new float[Cells.Count];
            _h = new float[Cells.Count];

            foreach (var cell in Cells)
            {
                var cellIndex = _cellIndexDictionary[cell];
                _h[cellIndex] = height;
            }
        }

        private void PreprocessAtmosphere()
        {
            AssignIndicesToCells();
            ConstructNeighbourArrays();
        }

        private void ConstructNeighbourArrays()
        {
            _neighbourIndices = new int[Cells.Count][];
            _faceWidths = new float[Cells.Count][];
            _distancesBetweenCenters = new float[Cells.Count][];
            _localNormals = new Vector3[Cells.Count];
            _areas = new float[Cells.Count];

            foreach (var cell in Cells)
            {
                var facesWithNeighbours = FoamUtils.FacesWithNeighbours(cell);

                var cellCenter = FoamUtils.CenterOf(cell);
                var localEast = Vector3.Cross(cellCenter, new Vector3(0, 0, 1));
                var clockwiseComparer = new CompareVectorsClockwise(FoamUtils.CenterOf(cell), localEast);
                var sortedFaces = facesWithNeighbours.OrderBy(FoamUtils.CenterOf, clockwiseComparer);

                var neighbourIndices = sortedFaces.Select(face => _cellIndexDictionary[FoamUtils.NeighbourAcross(face, cell)]);
                var faceWidths = sortedFaces.Select(face => FoamUtils.WidthOfVerticalFace(face));
                var distanceBetweenCenters = sortedFaces.Select(face => FoamUtils.DistanceBetweenCellCentersAcross(face));

                var cellIndex = _cellIndexDictionary[cell];
                _neighbourIndices[cellIndex] = neighbourIndices.ToArray();
                _faceWidths[cellIndex] = faceWidths.ToArray();
                _distancesBetweenCenters[cellIndex] = distanceBetweenCenters.ToArray();
                _localNormals[cellIndex] = FoamUtils.CenterOf(cell).normalized;
                _areas[cellIndex] = FoamUtils.HorizontalAreaOf(cell);
            }
        }

        private void AssignIndicesToCells()
        {
            _cellIndexDictionary = new Dictionary<Cell, int>();

            for(int i = 0; i < Cells.Count; i++)
            {
                _cellIndexDictionary.Add(Cells[i], i);
            }
        }
    }
}
