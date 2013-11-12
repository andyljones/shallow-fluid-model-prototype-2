using System;
using System.Collections.Generic;
using Foam;

namespace Simulator.ShallowFluidSimulator
{
    public class FloatField
    {
        public float[] Values;

        public FloatField(int size)
        {
            Values = new float[size];
        }

        public FloatField(float[] values)
        {
            Values = values;
        }

        public float this[int index]
        {
            get
            {
                return Values[index];
            }
            set
            {
                Values[index] = value;
            }
        }

        public static FloatField operator +(FloatField lhs, FloatField rhs)
        {
            if (lhs.Values.Length != rhs.Values.Length)
            {
                throw new ArgumentException("LHS float field has a different number of elements to the RHS!");
            }

            var sum = new float[lhs.Values.Length];

            for (int i = 0; i < lhs.Values.Length; i++)
            {
                sum[i] = lhs.Values[i] + rhs.Values[i];
            }

            return new FloatField(sum);
        }

        public static FloatField operator -(FloatField lhs, FloatField rhs)
        {
            if (lhs.Values.Length != rhs.Values.Length)
            {
                throw new ArgumentException("LHS FloatField has a different number of elements to the RHS!");
            }

            var sum = new float[lhs.Values.Length];

            for (int i = 0; i < lhs.Values.Length; i++)
            {
                sum[i] = lhs.Values[i] - rhs.Values[i];
            }

            return new FloatField(sum);
        }
        
        public static FloatField operator *(FloatField lhs, FloatField rhs)
        {
            if (lhs.Values.Length != rhs.Values.Length)
            {
                throw new ArgumentException("LHS FloatField has a different number of elements to the RHS!");
            }

            var sum = new float[lhs.Values.Length];

            for (int i = 0; i < lhs.Values.Length; i++)
            {
                sum[i] = lhs.Values[i] * rhs.Values[i];
            }

            return new FloatField(sum);
        }

        public static FloatField operator *(float lhs, FloatField rhs)
        {
            var sum = new float[rhs.Values.Length];

            for (int i = 0; i < rhs.Values.Length; i++)
            {
                sum[i] = lhs * rhs.Values[i];
            }

            return new FloatField(sum);
        }

        public static FloatField operator +(FloatField lhs, float rhs)
        {
            var sum = new float[lhs.Values.Length];

            for (int i = 0; i < lhs.Values.Length; i++)
            {
                sum[i] = lhs.Values[i] + rhs;
            }

            return new FloatField(sum);
        }
    }
}
