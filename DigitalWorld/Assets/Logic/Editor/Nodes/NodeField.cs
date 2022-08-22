using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using System;

namespace DigitalWorld.Logic.Editor
{
    internal class NodeField : NodeBase
    {
        #region Params

        public string baseTypeName;
        public string typeName;
        public string desc;

        #endregion

        #region Save & Load
        public override void Decode(XmlElement node)
        {
            if (node.HasAttribute("name"))
                name = node.GetAttribute("name");
            if (node.HasAttribute("baseTypeName"))
                baseTypeName = node.GetAttribute("baseTypeName");
            if (node.HasAttribute("typeName"))
                typeName = node.GetAttribute("typeName");
            if (node.HasAttribute("desc"))
                desc = node.GetAttribute("desc");
        }

        public override void Encode(XmlElement node)
        {
            node.SetAttribute("name", name);
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
                v.desc = this.desc;
            }

            return obj;
        }
        #endregion
    }
}
