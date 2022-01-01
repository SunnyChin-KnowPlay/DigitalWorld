using Dream.FixMath;
using System;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 属性值
    /// </summary>
    public sealed class PropertyValue : IEquatable<PropertyValue>
    {
        private readonly int minV;
        /// <summary>
        /// 最小值
        /// </summary>
        public int MinV { get { return minV; } }

        private int maxV;
        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxV { get { return maxV; } }

        private int currentV;

        public int Value
        {
            get
            {
                return currentV;
            }
        }

        /// <summary>
        /// 当前值域比值
        /// </summary>
        public FixFactor Factor
        {
            get
            {
                if (this.maxV == this.minV)
                    return FixFactor.one;

                return new FixFactor(this.currentV, this.Range);
            }
        }

        /// <summary>
        /// 极差 范围
        /// </summary>
        public int Range
        {
            get
            {
                return this.maxV - this.minV;
            }
        }

        /// <summary>
        /// 是否当前为最小值
        /// </summary>
        public bool IsMin
        {
            get { return this.currentV <= this.minV; }
        }

        /// <summary>
        /// 是否当前为最大值
        /// </summary>
        public bool IsMax
        {
            get { return this.currentV >= this.maxV; }
        }

        public PropertyValue()
        {
            this.currentV = int.MaxValue;
            this.minV = 0;
            this.maxV = int.MaxValue;
        }

        public PropertyValue(int min = 0, int max = int.MaxValue, int defaultValue = int.MaxValue)
        {
            this.currentV = defaultValue;
            this.minV = min;
            this.maxV = max;
        }

        public void AddMaxValue(int value)
        {
            this.maxV += value;
            this.Clamp();
        }

        public void MinusMaxValue(int value)
        {
            this.maxV -= value;
            this.Clamp();
        }

        public bool Equals(PropertyValue other)
        {
            return this.Value == other.Value;
        }

        private void Clamp()
        {
            this.currentV = Math.Clamp(this.currentV, this.minV, this.maxV);
        }

        #region Operator
        public static PropertyValue operator +(PropertyValue a, int b)
        {
            a.currentV += b;
            a.Clamp();
            return a;
        }

        public static PropertyValue operator +(PropertyValue a, FixFactor b)
        {
            int changeV = a.Range * b;
            a.currentV += changeV;
            a.Clamp();
            return a;
        }

        public static PropertyValue operator -(PropertyValue a, int b)
        {
            a.currentV -= b;
            a.Clamp();
            return a;
        }

        public static PropertyValue operator -(PropertyValue a, FixFactor b)
        {
            int changeV = a.Range * b;
            a.currentV -= changeV;
            a.Clamp();
            return a;
        }
        #endregion
    }
}
