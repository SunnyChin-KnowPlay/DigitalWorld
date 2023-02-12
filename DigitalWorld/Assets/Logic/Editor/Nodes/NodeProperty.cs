using System.Xml;
using UnityEditor;

namespace DigitalWorld.Logic.Editor
{
    internal class NodeProperty : NodeItem
    {
        #region Params
        /// <summary>
        /// 值类型
        /// </summary>
        public string ValueType { get => valueType; set => valueType = value; }
        protected string valueType;
        #endregion

        #region Clone
        public override object Clone()
        {
            NodeProperty v = new NodeProperty();
            this.CloneTo(v);
            return v;
        }

        public override T CloneTo<T>(T obj)
        {
            if (base.CloneTo(obj) is NodeProperty v)
            {
                v.valueType = this.valueType;
            }
            return obj;
        }
        #endregion

        #region Common
        public override string GetTitle()
        {
            return "Property";
        }

        #endregion

        #region GUI
        public override void OnGUIParams(bool editing = false)
        {
            base.OnGUIParams(editing);

        }

        public override void OnGUIBody()
        {
            this.valueType = NodeProperty.FindTypeName(EditorGUILayout.Popup("valueType", NodeProperty.FindTypeIndex(this.valueType), NodeField.TypeDisplayArray));
            base.OnGUIBody();
        }
        #endregion

        #region Serialization
        public override void Encode(XmlElement node)
        {
            base.Encode(node);

            node.SetAttribute("valueType", this.valueType);
        }

        public override void Decode(XmlElement node)
        {
            base.Decode(node);

            if (node.HasAttribute("valueType"))
            {
                this.valueType = node.GetAttribute("valueType");
            }
        }
        #endregion
    }
}
