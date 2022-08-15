using System;
using UnityEngine;

namespace DigitalWorld.Logic
{
    

    public abstract partial class Property<T> : PropertyBase, IEquatable<Property<T>>
    {
        #region TypeCode
        public override ETypeCode TypeCode
        {
            get
            {
                Type type = typeof(T).BaseType;
                Enum.TryParse(type.Name, true, out ETypeCode ret);

                return ret;
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// 正在触发的事件
        /// </summary>
        public Event TriggeringEvent
        {
            get => triggeringEvent;
            internal set => triggeringEvent = value;
        }
        protected Event triggeringEvent;
        #endregion

        #region Value
        protected virtual T GetValue()
        {
            throw new NotImplementedException();
        }

        protected virtual void SetValue(T value)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Equatable
        public virtual bool Equals(Property<T> other)
        {
            return this.GetValue().Equals(other.GetValue());
        }
        public virtual int GetHashCode(object obj)
        {
            return GetValue().GetHashCode();
        }
        #endregion

        #region Formattable
        public override string ToString(string format, IFormatProvider formatProvider)
        {
            string ret = base.ToString(format, formatProvider);
            ret = string.Format("{0}, value is:{1}", ret, this.GetValue().ToString());
            return ret;
        }
        #endregion

        #region Clone
        public override TObj CloneTo<TObj>(TObj obj)
        {
            if (base.CloneTo(obj) is Property<T> pt)
            {

            }
            return obj;
        }
        #endregion

    }
}
