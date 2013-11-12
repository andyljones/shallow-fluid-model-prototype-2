using System.Linq;
using Foam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator;
using Simulator.ShallowFluidSimulator;
using Tests.Fakes;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidSimulatorTests
{
    [TestClass]
    public class PreprocessorTests
    {
        private CellPreprocessor _preprocessor;

        [TestInitialize]
        public void Create_Fake_Atmosphere_And_Operators()
        {
            var fakeAtmo = new FakeAtmosphere();
            _preprocessor = new CellPreprocessor(fakeAtmo.Cells);
        }

        [TestMethod]
        public void Constructor_Should_Assign_Contiguous_Indices_To_Each_Cell()
        {
            var expected = new[] {0, 1};
            var actual = _preprocessor.CellIndexDict.Values;

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void Constructor_Should_Calculate_Areas_For_Each_Cell_Correctly()
        {
            var expectedArea = 269 * Mathf.Sqrt(3) / 4;
            var actualAreas = _preprocessor.Areas;

            var tolerance = 0.001f;

            Assert.IsTrue(actualAreas.All(area => Mathf.Abs(area - expectedArea) < tolerance));
        }

        [TestMethod]
        public void Constructor_Should_Calculate_Widths_Of_Each_Face_With_A_Neighbour_For_Each_Cell()
        {
            var expectedNumberOfCells = 2;
            var expectedWidthsPerCell = 1;
            var actualWidthsPerCell = _preprocessor.Widths.Select(arrayOfWidths => arrayOfWidths.Count()).ToList();

            Assert.AreEqual(expectedNumberOfCells, actualWidthsPerCell.Count());
            Assert.IsTrue(actualWidthsPerCell.All(count => count == expectedWidthsPerCell));
        }

        [TestMethod]
        public void Constructor_Should_Calculate_Width_Of_Central_Vertical_Face_Correctly()
        {
            var expectedWidth = Mathf.Sqrt(529f / 2f);
            var actualWidth = _preprocessor.Widths[0][0];

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedWidth - actualWidth) < tolerance);
        }

        [TestMethod]
        public void Constructor_Should_Calculate_Neighbour_Indices_Correctly()
        {
            var expected = new[] {new[] {1}, new[] {0}};
            var actual = _preprocessor.IndicesOfNeighbours;

            CollectionAssert.AreEqual(expected[0], actual[0]);
            CollectionAssert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected.Length, actual.Length);
        }

        [TestMethod]
        public void Constructor_Should_Calculate_Distances_Between_Cells_Correctly()
        {
            var expectedDistance = 23f / 3f;
            var actualDistance = _preprocessor.DistancesBetweenCenters[0][0];

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedDistance - actualDistance) < tolerance);
        }

        [TestMethod]
        public void Constructor_Should_Calculate_Distances_Between_Neighbours_For_Each_Cell()
        {
            var expectedNumberOfCells = 2;
            var expectedDistancesPerCell = 1;
            var actualDistancesPerCell = _preprocessor.Widths.Select(arrayOfWidths => arrayOfWidths.Count()).ToList();

            Assert.AreEqual(expectedNumberOfCells, actualDistancesPerCell.Count());
            Assert.IsTrue(actualDistancesPerCell.All(count => count == expectedDistancesPerCell));
        }
    }
}