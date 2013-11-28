using System.Collections.Generic;
using System.Linq;
using Foam;
using NUnit.Framework;
using Simulator.ShallowFluid;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidTests
{
    [TestFixture]
    public class FoamGeometryTests
    {
        [Test]
        public void Constructor_ShouldTakeAGraphOfCells()
        {
            var fakeGraph = new Graph<Cell>();

            var geometry = new FoamGeometry(fakeGraph);

            Assert.That(geometry, Is.Not.Null);
        }

        [Test]
        public void Constructor_GivenAGraphOfCells_ShouldSetAreaToScalarFieldWithSameCellsAsKeys()
        {
            // Create a fake face with 3 vertices:
            var fakeFace = new Face();
            var fakeVertices = new List<Vertex>
            {
                new Vertex {Faces = new List<Face> {fakeFace}},
                new Vertex {Faces = new List<Face> {fakeFace}},
                new Vertex {Faces = new List<Face> {fakeFace}}
            };
            fakeFace.Vertices = fakeVertices;
            
            // Create a fake cell with two identical faces:
            var fakeCell = new Cell {Faces = new List<Face> {fakeFace, fakeFace}, Vertices = fakeVertices};
            var fakeGraph = new Graph<Cell> {{fakeCell, null}};
            var geometry = new FoamGeometry(fakeGraph);

            var areaKeys = geometry.Areas.Keys;

            Assert.That(areaKeys.Single(), Is.EqualTo(fakeGraph.Keys.Single()));
        }

        [Test]
        public void Constructor_GivenAGraphOfASingleCell_ShouldCalculateCorrectArea()
        {
            // Create a fake face with 3 vertices:
            var fakeFace = new Face();
            var fakeVertices = new List<Vertex>
            {
                new Vertex(1, 0, 0) {Faces = new List<Face> {fakeFace}},
                new Vertex(0, 1, 0) {Faces = new List<Face> {fakeFace}},
                new Vertex(0, 0, 1) {Faces = new List<Face> {fakeFace}}
            };
            fakeFace.Vertices = fakeVertices;

            // Create a fake cell with two identical faces:
            var fakeCell = new Cell { Faces = new List<Face> { fakeFace, fakeFace }, Vertices = fakeVertices };
            var fakeGraph = new Graph<Cell> { { fakeCell, null } };
            var geometry = new FoamGeometry(fakeGraph);

            var area = geometry.Areas.Values.Single();
            var expectedArea = Mathf.Sqrt(3) / 2;

            Assert.That(area, Is.InRange(expectedArea-0.001f, expectedArea+0.001f));
        }

        [Test]
        public void Constructor_GivenAGraphOfTwoCells_ShouldCalculateCorrectWidthOfSeparatingFace()
        {
            // Create 2 fake edges:
            var fakeVertices = new List<Vertex>
            {
                new Vertex(1, 0, 0),
                new Vertex(0, 1, 0),
                new Vertex(2, 0, 0),
                new Vertex(0, 2, 0)
            };
            var fakeEdgeA = new Edge { Vertices = fakeVertices.GetRange(0, 2) };
            fakeVertices[0].Edges.Add(fakeEdgeA);
            fakeVertices[1].Edges.Add(fakeEdgeA);
            var fakeEdgeB = new Edge { Vertices = fakeVertices.GetRange(2, 2) };
            fakeVertices[2].Edges.Add(fakeEdgeB);
            fakeVertices[3].Edges.Add(fakeEdgeB);

            // Create fake face from fake edges & vertices:
            var fakeFace = new Face {Edges = new List<Edge> {fakeEdgeA, fakeEdgeB}, Vertices = fakeVertices};
           
            // Create fake cells sharing the face:
            var fakeCellA = new Cell {Faces = new List<Face> {fakeFace}};
            var fakeCellB = new Cell {Faces = new List<Face> {fakeFace}};

            // Create graph with the two cells:
            var fakeGraph = new Graph<Cell> { { fakeCellA, new List<Cell> {fakeCellB} }, {fakeCellB, new List<Cell> {fakeCellA}} };
            var geometry = new FoamGeometry(fakeGraph);

            var width = geometry.Widths[fakeCellA][fakeCellB];
            var expectedWidth = Mathf.Sqrt(2) + 1/Mathf.Sqrt(2);

            Assert.That(width, Is.InRange(expectedWidth - 0.001f, expectedWidth + 0.001f));
        }
    }
}
