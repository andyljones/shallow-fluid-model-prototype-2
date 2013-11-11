using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.ShallowFluidSimulator;

namespace Tests.SimulatorTests.ShallowFluidSimulatorTests
{
    [TestClass]
    public class FloatFieldTests
    {
        private FloatField _fieldB;
        private FloatField _fieldA;

        [TestInitialize]
        public void Create_Two_Float_Fields()
        {
            _fieldA = new FloatField(new float[] {2, 4, 6});
            _fieldB = new FloatField(new float[] {-4, -1, 7});
        }

        [TestMethod]
        public void Constructor_Should_Take_An_Int_And_Create_An_Array_Of_That_Size()
        {
            var expected = 8;
            var actual = new FloatField(8).Values.Length;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Trying_To_Add_Fields_Of_Different_Size_Should_Throw_An_Error()
        {
            var fieldC = new FloatField(4);

            var waste = _fieldA + fieldC;
        }

        [TestMethod]
        public void Sum_Of_A_And_B_Should_Be_Negative2_3_13()
        {
            var expected = new FloatField(new float[] {-2, 3, 13});
            var actual = _fieldA + _fieldB;

            CollectionAssert.AreEqual(expected.Values, actual.Values);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Trying_To_Subtract_Fields_Of_Different_Size_Should_Throw_An_Error()
        {
            var fieldC = new FloatField(4);

            var waste = _fieldA - fieldC;
        }

        [TestMethod]
        public void Difference_Of_A_And_B_Should_Be_6_5_Negative1()
        {
            var expected = new FloatField(new float[] { 6, 5, -1 });
            var actual = _fieldA - _fieldB;

            CollectionAssert.AreEqual(expected.Values, actual.Values);
        }
    }
}
