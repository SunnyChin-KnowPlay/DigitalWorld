using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using System;

namespace DigitalWorld.Logic.Editor
{
    public class NodeField : NodeBase
    {
        #region Params

        public string baseTypeName;
        public string typeName;
        public string desc;

        #endregion

        #region Save & Load
        public override void Decode(XmlElement node)
        {
            base.Decode(node);
            if (node.HasAttribute("baseTypeName"))
                baseTypeName = node.GetAttribute("baseTypeName");
            if (node.HasAttribute("typeName"))
                typeName = node.GetAttribute("typeName");
            if (node.HasAttribute("desc"))
                desc = node.GetAttribute("desc");
        }

        public override void Encode(XmlElement node)
        {
            base.Encode(node);
            node.SetAttribute("baseTypeName", baseTypeName);
            node.SetAttribute("typeName", typeName);
            node.SetAttribute("desc", desc);
        }
        #endregion

        #region GUI
        public override void OnGUIBody()
        {
            base.OnGUIBody();

            name = EditorGUILayout.TextField("name", name);
            typeName = EditorGUILayout.TextField("typeName", typeName);
            System.Type type = NodeField.FindType(NodeField.FindTypeIndex(typeName));
            baseTypeName = type.BaseType.ToString();
            desc = EditorGUILayout.TextField("desc", desc);
        }
        #endregion

        #region Common
        public override string GetTitle()
        {
            return "字段";
        }

        public override object Clone()
        {
            NodeField v = new NodeField();
            this.CloneTo(v);
            return v;
        }

        public override T CloneTo<T>(T obj)
        {
            if (base.CloneTo(obj) is NodeField v)
            {
                v.typeName = this.typeName;
                v.baseTypeName = this.baseTypeName;
                v.desc = this.desc;
            }

            return obj;
        }
        #endregion
    }
}
