using System;
using System.Xml;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 属性基类
    /// </summary>
    public abstract partial class PropertyBase : NodeBase, IFormattable
    {
        #region Params
        public override ENodeType NodeType => ENodeType.Property;

        /// <summary>
        /// 类型编码
        /// </summary>
        public abstract ETypeCode TypeCode { get; }
        #endregion

        #region Pool
        public override void OnAllocate()
        {

            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
        }
        #endregion

        #region Clone
        public override T CloneTo<T>(T obj)
        {
            if (base.CloneTo(obj) is PropertyBase pb)
            {
               
            }
            return obj;
        }
        #endregion

        #region Formattable
        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("typecode is:{0}", TypeCode);
        }
        #endregion

        #region Buffer
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

       
        }

        protected override void OnEncode()
        {
            base.OnEncode();

        }

        protected override void OnDecode()
        {
            base.OnDecode();

        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

        }
        #endregion
    }
}
