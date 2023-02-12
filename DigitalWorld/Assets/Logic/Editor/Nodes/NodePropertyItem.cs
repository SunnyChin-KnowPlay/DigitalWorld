using System;
using System.Xml;

namespace DigitalWorld.Logic.Editor
{
    internal class NodePropertyItem : NodeBase
    {
        #region Params
        /// <summary>
        /// 属性的值类型
        /// </summary>
        public Type ValueType { get => valueType; set => valueType = value; }
        private Type valueType;

        public string desc;
        #endregion

        #region Clone
        public override object Clone()
        {
            NodePropertyItem v = new NodePropertyItem();
            this.CloneTo(v);
            return v;
        }

        public override T CloneTo<T>(T obj)
        {
            if (base.CloneTo(obj) is NodePropertyItem v)
            {
                v.valueType = this.valueType;
                v.desc = this.desc;
            }
            return obj;
        }
        #endregion

        #region Common
        public override string GetTitle()
        {
            return "PropertyItem";
        }
        #endregion

        #region Serialization
        public override void Decode(XmlElement node)
        {
            base.Decode(node);

            if (node.HasAttribute("desc"))
                this.desc = node.GetAttribute("desc");

            if (node.HasAttribute("valueType"))
                this.valueType = Utility.GetTemplateType(node.GetAttribute("valueType"));
        }

        public override void Encode(XmlElement node)
        {
            base.Encode(node);

            node.SetAttribute("desc", this.desc);
            node.SetAttribute("valueType", this.valueType.FullName);
        }
        #endregion
    }
}
