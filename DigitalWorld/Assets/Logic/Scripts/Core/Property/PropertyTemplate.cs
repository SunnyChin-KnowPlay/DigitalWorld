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

       
    }
}
