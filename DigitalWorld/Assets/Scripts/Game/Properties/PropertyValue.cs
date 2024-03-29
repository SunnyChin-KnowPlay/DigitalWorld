﻿using Dream.FixMath;
using System;

using DigitalWorld.Defined.Game;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 属性值
    /// </summary>
    public sealed class PropertyValue : IEquatable<PropertyValue>
    {
        #region Event
        /// <summary>
        /// 属性改变代理
        /// 其中新旧值会经过Clamp处理
        /// 改变值则是期望值
        /// 所以newerValue - oldValue可能不等于expectChangeValue
        /// </summary>
        /// <param name="propertyType">属性类型</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="newerValue">新值</param>
        /// <param name="expectChangeValue">期望改变值</param>
        public delegate void OnPropertyChangedHandle(EPropertyType propertyType, int oldValue, int newerValue, int expectChangeValue);
        public event OnPropertyChangedHandle OnPropertyChanged;
        #endregion

        #region Params
        /// <summary>
        /// 属性类型
        /// </summary>
        public EPropertyType PropertyType => propertyType;
        private readonly EPropertyType propertyType;

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
        private readonly int standV;

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
        #endregion

        #region Logic
        public PropertyValue(EPropertyType propertyType, int min = 0, int max = int.MaxValue, int defaultValue = int.MaxValue, int standValue = 0)
        {
            this.propertyType = propertyType;
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
        #endregion

        #region Event Process
        private void InvokeChange(int oldValue, int newerValue, int expectChangeValue)
        {
            if (null != OnPropertyChanged)
            {
                OnPropertyChanged.Invoke(this.propertyType, oldValue, newerValue, expectChangeValue);
            }
        }
        #endregion

        #region Operator
        public static PropertyValue operator +(PropertyValue a, int b)
        {
            int curValue = a.baseV;

            a.baseV += b;
            a.Clamp();

            a.InvokeChange(curValue, a.baseV, b);

            return a;
        }

        public static PropertyValue operator +(PropertyValue a, FixFactor b)
        {
            int curValue = a.baseV;

            int changeV = a.Range * b;
            a.baseV += changeV;
            a.Clamp();

            a.InvokeChange(curValue, a.baseV, changeV);

            return a;
        }

        public static PropertyValue operator -(PropertyValue a, int b)
        {
            int curValue = a.baseV;

            a.baseV -= b;
            a.Clamp();

            a.InvokeChange(curValue, a.baseV, -b);
            return a;
        }

        public static PropertyValue operator -(PropertyValue a, FixFactor b)
        {
            int curValue = a.baseV;

            int changeV = a.Range * b;
            a.baseV -= changeV;
            a.Clamp();

            a.InvokeChange(curValue, a.baseV, -changeV);
            return a;
        }
        #endregion
    }
}
