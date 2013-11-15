using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Simulator.ShallowFluid;
using UnityEngine;

namespace Tests.SimulatorTests.ShallowFluidTests
{
    [TestFixture]
    public class VectorFieldTests
    {
        [Test]
        public void ParameterlessConstructor_CreatesVectorFieldOfSize0()
        {
            var field = new VectorField<int>();

            Assert.That(field.Count, Is.EqualTo(0));
        }

        [Test]
        public void DictionaryConstructor_CreatesEquivalentVectorField()
        {
            var dictionary = new Dictionary<int, Vector3> {{0, new Vector3(1, 0, 0)}};
            var field = new VectorField<int>(dictionary);

            Assert.That(field, Is.EquivalentTo(dictionary));
        }

        [Test]
        public void EnumerableConstructor_CreatesVectorFieldWithEnumerableAsKeys()
        {
            var enumerable = Enumerable.Range(7, 4);
            var field = new VectorField<int>(enumerable);

            Assert.That(field.Keys, Is.EquivalentTo(enumerable));
        }
    }
}
