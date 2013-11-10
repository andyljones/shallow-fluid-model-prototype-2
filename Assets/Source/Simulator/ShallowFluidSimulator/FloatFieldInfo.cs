using System;
using System.Reflection;

namespace Simulator.ShallowFluidSimulator
{
    public class FloatFieldInfo
    {
        public FieldInfo Field { get; private set; }

        public FloatFieldInfo(FieldInfo field)
        {
            if (field.FieldType == typeof(float))
            {
                Field = field;
            }
            else
            {
                throw new ArgumentException("Field does not contain a float type!");
            }
        }

        public float GetValue(object obj)
        {
            return (float) Field.GetValue(obj);
        }

        public void SetValue(object obj, float value)
        {
            Field.SetValue(obj, value);
        }
    }
}
