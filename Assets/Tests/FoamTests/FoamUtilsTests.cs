using Foam;
using NUnit.Framework;
using UnityEngine;

namespace Tests.FoamTests
{
    [TestFixture]
    public class FoamUtilsTests
    {
        private FakeAtmosphere _fakeAtmo;

        [SetUp]
        public void Create_Fake_Atmosphere_And_Simulator()
        {
            _fakeAtmo = new FakeAtmosphere();
        }

        [Test]
        public void There_Should_Be_Only_One_Face_Of_Cell0_With_A_Neighbour()
        {
            var facesWithNeighbours = _fakeAtmo.Cells[0].FacesWithNeighbours();

            var expectedNumberOFaces = 1;
            var actualNumberOfFaces = facesWithNeighbours.Count;

            Assert.AreEqual(expectedNumberOFaces, actualNumberOfFaces);
        }

        [Test]
        public void Cell0_Should_Have_Three_Vertical_Edges()
        {
            var verticalEdges = _fakeAtmo.Cells[0].VerticalEdges();

            var expectedNumber = 3;
            var actualNumber = verticalEdges.Count;

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [Test]
        public void Cell0s_Top_Face_Should_Be_Face1()
        {
            var expectedFace = _fakeAtmo.Cells[0].Faces[1];
            var actualFace = _fakeAtmo.Cells[0].TopFace();

            Assert.AreEqual(expectedFace, actualFace);
        }

        [Test]
        public void Cell0s_Bottom_Face_Should_Be_Face0()
        {
            var expectedFace = _fakeAtmo.Cells[0].Faces[0];
            var actualFace = _fakeAtmo.Cells[0].BottomFace();

            Assert.AreEqual(expectedFace, actualFace);
        }

        [Test]
        public void Cell0s_Bottom_Face_Should_Have_Area_50Sqrt3()
        {
            var expectedArea = 50 * Mathf.Sqrt(3);
            var actualArea = _fakeAtmo.Cells[0].Faces[0].Area();

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedArea - actualArea) < tolerance);
        }

        [Test]
        public void Cell0s_Top_Face_Should_Have_Area_169Sqrt3_Over2()
        {
            var expectedArea = 169 * Mathf.Sqrt(3) / 2;
            var actualArea = _fakeAtmo.Cells[0].Faces[1].Area();

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedArea - actualArea) < tolerance);
        }

        [Test]
        public void Center_Of_Cell0s_Bottom_Face_Should_Be_Correct()
        {
            var expectedVector = new Vector3(-1, 1, 1).normalized * 10f;
            var actualVector = _fakeAtmo.Cells[0].Faces[0].Center();

            var tolerance = 0.001f;

            Assert.IsTrue((expectedVector - actualVector).magnitude < tolerance);
        }

        [Test]
        public void Center_Of_Cell0_Should_Be_Correct()
        {
            var expectedVector = new Vector3(-1, 1, 1).normalized * 11.5f;
            var actualVector = _fakeAtmo.Cells[0].Center();

            var tolerance = 0.001f;

            Assert.IsTrue((expectedVector - actualVector).magnitude < tolerance);
        }

        [Test]
        public void Length_Of_Cell0s_Edge0_Should_Be_Sqrt200()
        {
            var expectedLength = Mathf.Sqrt(200);
            var actualLength = _fakeAtmo.Cells[0].Edges[0].Length();

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedLength - actualLength) < tolerance);
        }

        [Test]
        public void Distance_Between_Cell_Centers_Should_Be_Correct()
        {
            var expectedDistance = 2 * Mathf.Asin(1/Mathf.Sqrt(3)) * 11.5f;
            var actualDistance = _fakeAtmo.Cells[0].Faces[4].DistanceBetweenNeighbouringCellCenters();

            var tolerance = 0.001f;
            System.Console.WriteLine(actualDistance);

            Assert.IsTrue(Mathf.Abs(expectedDistance - actualDistance) < tolerance);
        }

        [Test]
        public void Top_Edge_Of_Cell0Face2_Should_Be_Cell0Edge3()
        {
            var expectedEdge = _fakeAtmo.Cells[0].Edges[3];
            var actualEdge = _fakeAtmo.Cells[0].Faces[2].TopEdge();

            Assert.AreEqual(expectedEdge, actualEdge);
        }

        [Test]
        public void Bottom_Edge_Of_Cell0Face2_Should_Be_Cell0Edge0()
        {
            var expectedEdge = _fakeAtmo.Cells[0].Edges[0];
            var actualEdge = _fakeAtmo.Cells[0].Faces[2].BottomEdge();

            Assert.AreEqual(expectedEdge, actualEdge);
        }

        [Test]
        public void Width_Of_Cell0Face2_Should_Be_Sqrt529_Over_2()
        {
            var expectedWidth = Mathf.Sqrt(529f / 2f);
            var actualWidth = _fakeAtmo.Cells[0].Faces[2].VerticalWidth();

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedWidth - actualWidth) < tolerance);
        }

        [Test]
        public void Horizontal_Area_Of_Cell0_Should_Be_269Sqrt3_Over_4()
        {
            var expectedArea = 269 * Mathf.Sqrt(3) / 4;
            var actualArea = _fakeAtmo.Cells[0].HorizontalArea();

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expectedArea - actualArea) < tolerance);
        }
    }
}
