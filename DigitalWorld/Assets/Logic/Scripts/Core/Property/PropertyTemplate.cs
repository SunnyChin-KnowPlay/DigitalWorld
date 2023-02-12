using System.Xml;
using UnityEngine;

namespace DigitalWorld.Logic.Properties
{
    public abstract partial class PropertyTemplate<T> : PropertyBase
    {

        #region Params
        public abstract T Value
        {
            get;
            set;
        }

        #endregion

        #region Clone

        public override TObj CloneTo<TObj>(TObj obj)
        {
            if (base.CloneTo(obj) is PropertyTemplate<T> t)
            {

            }
            return obj;
        }
        #endregion

        #region Logic
        public override object GetValue()
        {
            return Value;
        }
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();
        }
        #endregion

        #region Serialization
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();
        }

        protected override void OnDecode()
        {
            base.OnDecode();
        }

        protected override void OnEncode()
        {
            base.OnEncode();
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);
        }
        #endregion
    }
}
