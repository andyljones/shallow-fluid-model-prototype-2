using System.Collections.Generic;
using FakeItEasy;
using Foam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.ShallowFluidSimulator;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidSimulatorTests
{
    [TestClass]
    public class DifferenceOperatorTests
    {
        private DifferenceOperators _operators;
        private FloatField _fieldA;
        private FloatField _fieldB;
        private Vector3[] _vectors;

        [TestInitialize]
        public void Create_DifferenceOperators_From_Fake_Preprocessor()
        {
            // Fake preprocessor encodes 7 hexagons with sides of unit length, laid out as so:
            //     __
            //  __/  \__
            // /  \_1/  \
            // \_6/  \_2/
            // /  \_0/  \
            // \_5/  \_3/
            //    \_4/
            //
            // We're only interested in whether the operators evaluate correctly on the zeroth hexagon, so much of
            // the information about the other six is neglected.

            var fakeCellIndices = new[] {0, 1, 2, 3, 4, 5, 6};
            var fakeNeighbourIndices = new[] {new[] {1, 2, 3, 4, 5, 6}, new int[0], new int[0], new int[0], new int[0], new int[0], new int[0]};
            var fakeArea = 3*Mathf.Sqrt(3)/2;
            var fakeAreas = new[] {fakeArea, fakeArea, fakeArea, fakeArea, fakeArea, fakeArea, fakeArea};
            var fakeWidths = new[]
            {
                new[] {1f, 1f, 1f, 1f, 1f, 1f}, new float[0], new float[0], new float[0], new float[0], new float[0],
                new float[0]
            };
            var fakeDistance = 2*Mathf.Sqrt(3);
            var fakeDistances = new[]
            {
                new[] {fakeDistance, fakeDistance, fakeDistance, fakeDistance, fakeDistance, fakeDistance}, new float[0],
                new float[0], new float[0], new float[0], new float[0], new float[0]
            };
            var fakeX = Mathf.Sqrt(3)/2;
            var fakeY = 1f/2f;
            var fakeNormals = new[]
            {
                new[]
                {
                    new Vector3(0, 1, 0), new Vector3(fakeX, fakeY, 0), new Vector3(fakeX, -fakeY, 0),
                    new Vector3(0, -1, 0), new Vector3(-fakeX, -fakeY, 0), new Vector3(-fakeX, fakeY, 0)
                },
                new Vector3[0], new Vector3[0], new Vector3[0], new Vector3[0], new Vector3[0], new Vector3[0]
            };

            var fakePreprocessor = A.Fake<IPreprocessor>();
            A.CallTo(() => fakePreprocessor.CellIndices).Returns(fakeCellIndices);
            A.CallTo(() => fakePreprocessor.IndicesOfNeighbours).Returns(fakeNeighbourIndices);
            A.CallTo(() => fakePreprocessor.Areas).Returns(fakeAreas);
            A.CallTo(() => fakePreprocessor.Widths).Returns(fakeWidths);
            A.CallTo(() => fakePreprocessor.DistancesBetweenCenters).Returns(fakeDistances);
            A.CallTo(() => fakePreprocessor.NormalsToFaces).Returns(fakeNormals);

            _operators = new DifferenceOperators(fakePreprocessor);
            _fieldA = new FloatField(new float[] {-4, -9, -5, -8,   5, 9, 6});
            _fieldB = new FloatField(new float[] {-5, -7,  2,  7, -10, 3, 1});
            _vectors = new Vector3[] {};
        }

        [TestMethod]
        public void Jacobian_Of_FieldA_And_Field_B_Evaluates_Correctly_In_Cell_0()
        {
            var expected = 4 / Mathf.Sqrt(3);
            var actual = _operators.Jacobian(_fieldA, _fieldB)[0];

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expected - actual) < tolerance);
        }

        [TestMethod]
        public void FluxDiv_Of_FieldA_And_Field_B_Evaluates_Correctly_In_Cell_0()
        {
            var expected = -67f / 9f;
            var actual = _operators.FluxDivergence(_fieldA, _fieldB)[0];

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expected - actual) < tolerance);
        }

        [TestMethod]
        public void Laplacian_Of_FieldA_Evaluates_Correctly_In_Cell_0()
        {
            var expected = 22f / 9f;
            var actual = _operators.Laplacian(_fieldA)[0];

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expected - actual) < tolerance);
        }

        [TestMethod]
        public void Inverting_To_Elliptic_Equation_Works_Correctly_In_Cell_0()
        {
            var expected = 43f / 6f;
            var actual = _operators.InvertElliptic(_fieldA, _fieldB)[0];

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expected - actual) < tolerance);
        }

        [TestMethod]
        public void Gradient_Works_Correctly_In_Cell0()
        {
            var expected = new Vector3(-14f/3f, -14/(3*Mathf.Sqrt(3)), 0);
            var actual = _operators.Gradient(_fieldA)[0];

            var tolerance = 0.001f;

            Assert.IsTrue(Mathf.Abs(expected.x - actual.x) < tolerance);
            Assert.IsTrue(Mathf.Abs(expected.y - actual.y) < tolerance);
            Assert.IsTrue(Mathf.Abs(expected.z - actual.z) < tolerance);
        }
    }
}