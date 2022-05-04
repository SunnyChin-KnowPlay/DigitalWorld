using Dream.FixMath;
using System;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 属性值
    /// </summary>
    public sealed class PropertyValue : IEquatable<PropertyValue>
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public int MinV { get { return minV; } }
        private readonly int minV;

        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxV { get { return maxV; } }
        private int maxV;

        /// <summary>
        /// 基础值
        /// </summary>
        private int baseV;

        /// <summary>
        /// 标准值
        /// </summary>
        public int StandV { get { return standV; } }
        private int standV;

        public int Value
        {
            get
            {
                return baseV;
            }
        }

        /// <summary>
        /// 当前值域比值
        /// </summary>
        public FixFactor FactorInRange
        {
            get
            {
                if (this.maxV == this.minV)
                    return FixFactor.one;

                return new FixFactor(this.baseV, this.Range);
            }
        }

        public FixFactor FactorByStand
        {
            get
            {
                return new FixFactor(this.Value, this.StandV);
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
            get { return this.baseV <= this.minV; }
        }

        /// <summary>
        /// 是否当前为最大值
        /// </summary>
        public bool IsMax
        {
            get { return this.baseV >= this.maxV; }
        }

        public PropertyValue()
        {
            this.standV = 0;
            this.baseV = int.MaxValue;
            this.minV = 0;
            this.maxV = int.MaxValue;
        }

        public PropertyValue(int min = 0, int max = int.MaxValue, int defaultValue = int.MaxValue, int standValue = 0)
        {
            this.standV = standValue;
            this.baseV = defaultValue;
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
            this.baseV = Math.Clamp(this.baseV, this.minV, this.maxV);
        }

        #region Operator
        public static PropertyValue operator +(PropertyValue a, int b)
        {
            a.baseV += b;
            a.Clamp();
            return a;
        }

        public static PropertyValue operator +(PropertyValue a, FixFactor b)
        {
            int changeV = a.Range * b;
            a.baseV += changeV;
            a.Clamp();
            return a;
        }

        public static PropertyValue operator -(PropertyValue a, int b)
        {
            a.baseV -= b;
            a.Clamp();
            return a;
        }

        public static PropertyValue operator -(PropertyValue a, FixFactor b)
        {
            int changeV = a.Range * b;
            a.baseV -= changeV;
            a.Clamp();
            return a;
        }
        #endregion
    }
}
