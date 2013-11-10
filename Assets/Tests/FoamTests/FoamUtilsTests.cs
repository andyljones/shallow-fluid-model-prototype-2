using Foam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Fakes;
using UnityEngine;

namespace Tests.FoamTests
{
    [TestClass]
    public class FoamUtilsTests
    {
        private FakeAtmosphere _fakeAtmo;

        [TestInitialize]
        public void Create_Fake_Atmosphere_And_Simulator()
        {
            _fakeAtmo = new FakeAtmosphere();
        }

        [TestMethod]
        public void There_Should_Be_Only_One_Face_Of_Cell0_With_A_Neighbour()
        {
            var facesWithNeighbours = _fakeAtmo.Cells[0].FacesWithNeighbours();

            var expectedNumberOFaces = 1;
            var actualNumberOfFaces = facesWithNeighbours.Count;

            Assert.AreEqual(expectedNumberOFaces, actualNumberOfFaces);
        }

        [TestMethod]
        public void Cell0_Should_Have_Three_Vertical_Edges()
        {
            var verticalEdges = _fakeAtmo.Cells[0].VerticalEdges();

            var expectedNumber = 3;
            var actualNumber = verticalEdges.Count;

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void Cell0s_Top_Face_Should_Be_Face1()
        {
            var expectedFace = _fakeAtmo.Cells[0].Faces[1];
            var actualFace = FoamUtils.TopFaceOf(_fakeAtmo.Cells[0]);

            Assert.AreEqual(expectedFace, actualFace);
        }

        [TestMethod]
        public void Cell0s_Bottom_Face_Should_Be_Face0()
        {
            var expectedFace = _fakeAtmo.Cells[0].Faces[0];
            var actualFace = FoamUtils.BottomFaceOf(_fakeAtmo.Cells[0]);

            Assert.AreEqual(expectedFace, actualFace);
        }

        [TestMethod]
        public void Cell0s_Bottom_Face_Should_Have_Area_50Sqrt3()
        {
            var expectedArea = 50 * Mathf.Sqrt(3);
            var actualArea = FoamUtils.AreaOf(_fakeAtmo.Cells[0].Faces[0]);

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedArea - actualArea) < tolerance);
        }

        [TestMethod]
        public void Cell0s_Top_Face_Should_Have_Area_169Sqrt3_Over2()
        {
            var expectedArea = 169 * Mathf.Sqrt(3) / 2;
            var actualArea = FoamUtils.AreaOf(_fakeAtmo.Cells[0].Faces[1]);

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedArea - actualArea) < tolerance);
        }

        [TestMethod]
        public void Center_Of_Cell0s_Bottom_Face_Should_Be_Correct()
        {
            var expectedVector = new Vector3(-10, 10, 10) / 3;
            var actualVector = FoamUtils.CenterOf(_fakeAtmo.Cells[0].Faces[0]);

            var tolerance = 0.001f;

            Assert.IsTrue((expectedVector - actualVector).magnitude < tolerance);
        }

        [TestMethod]
        public void Center_Of_Cell0_Should_Be_Correct()
        {
            var expectedVector = new Vector3(-11.5f, 11.5f, 11.5f) / 3;
            var actualVector = FoamUtils.CenterOf(_fakeAtmo.Cells[0]);

            var tolerance = 0.001f;

            Assert.IsTrue((expectedVector - actualVector).magnitude < tolerance);
        }

        [TestMethod]
        public void Length_Of_Cell0s_Edge0_Should_Be_Sqrt200()
        {
            var expectedLength = Mathf.Sqrt(200);
            var actualLength = FoamUtils.LengthOf(_fakeAtmo.Cells[0].Edges[0]);

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedLength - actualLength) < tolerance);
        }

        [TestMethod]
        public void Distance_Between_Cell_Centers_Should_Be_23_Over_3()
        {
            var expectedDistance = 23f/3f;
            var actualDistance = FoamUtils.DistanceBetweenCellCentersAcross(_fakeAtmo.Cells[0].Faces[4]);

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedDistance - actualDistance) < tolerance);
        }

        [TestMethod]
        public void Top_Edge_Of_Cell0Face2_Should_Be_Cell0Edge3()
        {
            var expectedEdge = _fakeAtmo.Cells[0].Edges[3];
            var actualEdge = FoamUtils.TopEdgeOf(_fakeAtmo.Cells[0].Faces[2]);

            Assert.AreEqual(expectedEdge, actualEdge);
        }

        [TestMethod]
        public void Bottom_Edge_Of_Cell0Face2_Should_Be_Cell0Edge0()
        {
            var expectedEdge = _fakeAtmo.Cells[0].Edges[0];
            var actualEdge = FoamUtils.BottomEdgeOf(_fakeAtmo.Cells[0].Faces[2]);

            Assert.AreEqual(expectedEdge, actualEdge);
        }

        [TestMethod]
        public void Width_Of_Cell0Face2_Should_Be_Sqrt264_Point_5()
        {
            var expectedWidth = Mathf.Sqrt(264.5f);
            var actualWidth = FoamUtils.WidthOfVerticalFace(_fakeAtmo.Cells[0].Faces[2]);

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedWidth - actualWidth) < tolerance);
        }

        [TestMethod]
        public void Horizontal_Area_Of_Cell0_Should_Be_269Sqrt3_Over_4()
        {
            var expectedArea = 269 * Mathf.Sqrt(3) / 4;
            var actualArea = FoamUtils.HorizontalAreaOf(_fakeAtmo.Cells[0]);

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedArea - actualArea) < tolerance);
        }
    }
}
