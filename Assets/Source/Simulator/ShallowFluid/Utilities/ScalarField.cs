using System.Collections.Generic;
using System.Linq;

namespace Simulator.ShallowFluid
{
    /// <summary>
    ///  Represents a mapping of objects of type T to floats.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScalarField<T> : Dictionary<T, float>
    {
        /// <summary>
        /// Creates an empty scalar field.
        /// </summary>
        public ScalarField() : base()
        {
        }

        /// <summary>
        /// Interprets the provided dictionary as a scalar field.
        /// </summary>
        /// <param name="dictionary"></param>
        public ScalarField(Dictionary<T, float> dictionary) : base(dictionary)
        {       
        }

        /// <summary>
        /// Creates a scalar field using the given enumerable as keys. Values are initialized to 0f.
        /// </summary>
        /// <param name="enumerable"></param>
        public ScalarField(IEnumerable<T> enumerable) : base(enumerable.ToDictionary(obj => obj, obj => default(float)))
        {
        }

        public static ScalarField<T> operator +(ScalarField<T> lhs, ScalarField<T> rhs)
        {
            var result = new ScalarField<T>();

            foreach (var node in lhs.Keys)
            {
                result.Add(node, lhs[node] + rhs[node]);
            }

            return result;
        }

        public static ScalarField<T> operator -(ScalarField<T> lhs, ScalarField<T> rhs)
        {
            var result = new ScalarField<T>();

            foreach (var node in lhs.Keys)
            {
                result.Add(node, lhs[node] - rhs[node]);
            }

            return result;
        }

        public static ScalarField<T> operator *(float lhs, ScalarField<T> rhs)
        {
            var result = new ScalarField<T>();

            foreach (var node in rhs.Keys)
            {
                result.Add(node, lhs * rhs[node]);
            }

            return result;
        }

        public static ScalarField<T> operator -(ScalarField<T> lhs, float rhs)
        {
            var result = new ScalarField<T>();

            foreach (var node in lhs.Keys)
            {
                result.Add(node, lhs[node] - rhs);
            }

            return result;
        }

        public static ScalarField<T> operator *(ScalarField<T> lhs, ScalarField<T> rhs)
        {
            var result = new ScalarField<T>();

            foreach (var node in lhs.Keys)
            {
                result.Add(node, lhs[node] * rhs[node]);
            }

            return result;
        }
    }
}
