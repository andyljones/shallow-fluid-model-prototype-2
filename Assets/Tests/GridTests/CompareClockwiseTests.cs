using Foam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;

namespace Tests.GridTests
{
    [TestClass]
    public class CompareClockwiseTests
    {
        private CompareVectorsClockwise _comparer;

        [TestInitialize]
        // Keep in mind we're in a left-hand coordinate system here. Positive Y axis is pointing towards you, positive
        // X axis is heading off to your right, positive Z axis is heading upwards.
        public void Create_Comparer_Around_Y_Axis_With_Z_Axis_Baseline()
        {
            _comparer = new CompareVectorsClockwise(new Vector3(0, 1, 0), new Vector3(0, 0, 1));
        }

        [TestMethod]
        public void Negative_X_Axis_Should_Be_Greater_Than_Positive_X_Axis()
        {
            int result = _comparer.Compare(new Vector3(-1, 0, 0), new Vector3(1, 0, 0));
            int expectedResult = 1;
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Positive_X_Axis_Should_Be_Less_Than_Negative_X_Axis()
        {
            int result = _comparer.Compare(new Vector3(1, 0, 0), new Vector3(-1, 0, 0));
            int expectedResult = -1;
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Equal_Vectors_Should_Be_Equal()
        {
            int result = _comparer.Compare(new Vector3(1, 0, 0), new Vector3(1, 0, 0));
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Vectors_Equal_To_The_Center_Vector_Should_Be_Equal()
        {
            int result = _comparer.Compare(new Vector3(0, 1, 0), new Vector3(0, 1, 0));
            int expectedResult = 0;
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void If_One_Vector_Is_Equal_To_The_Center_Vector_No_Vector_Should_Be_Clockwise_From_It()
        {
            int result = _comparer.Compare(new Vector3(0, 1, 0), new Vector3(1, 0, 0));
            int expectedResult = 1;
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void The_Vector_45N45E_Should_Be_Less_Than_45S45E()
        {
            int result = _comparer.Compare(new Vector3(1, 1, 1), new Vector3(1, 1, -1));
            int expectedResult = -1;
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void The_Vector_45N45E_Should_Be_Less_Than_45S45W()
        {
            int result = _comparer.Compare(new Vector3(1, 1, 1), new Vector3(-1, 1, -1));
            int expectedResult = -1;
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void The_Vector_45N45E_Should_Be_Less_Than_45N45W()
        {
            int result = _comparer.Compare(new Vector3(1, 1, 1), new Vector3(-1, 1, 1));
            int expectedResult = -1;
            Assert.AreEqual(expectedResult, result);
        }
    }
}
