using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Simulator.ShallowFluid;

namespace Tests.SimulatorTests.ShallowFluidTests
{
    [TestFixture]
    public class ScalarFieldTests
    {
        [Test]
        public void ParameterlessConstructor_CreatesScalarFieldOfSize0()
        {
            var field = new ScalarField<int>();

            Assert.That(field.Count, Is.EqualTo(0));
        }

        [Test]
        public void DictionaryConstructor_CreatesEquivalentScalarField()
        {
            var dictionary = new Dictionary<int, float> {{0, 1.5f}};
            var field = new ScalarField<int>(dictionary);

            Assert.That(field, Is.EquivalentTo(dictionary));
        }

        [Test]
        public void EnumerableConstructor_CreatesScalarFieldWithEnumerableAsKeys()
        {
            var enumerable = Enumerable.Range(7, 4);
            var field = new ScalarField<int>(enumerable);

            Assert.That(field.Keys, Is.EquivalentTo(enumerable));
        }
    }
}
