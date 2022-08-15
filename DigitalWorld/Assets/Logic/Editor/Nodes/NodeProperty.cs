using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;

namespace DigitalWorld.Logic.Editor
{
    internal class NodeProperty : NodeItem
    {
        /// <summary>
        /// 值类型
        /// </summary>
        protected string valueType;

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

            if (editing)
            {
                EditorGUILayout.LabelField("valueType", this.valueType);
            }
            else
            {
                this.valueType = NodeProperty.FindType(EditorGUILayout.Popup("valueType", NodeProperty.FindTypeIndex(this.valueType), NodeField.TypeDisplayArray));
            }
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
