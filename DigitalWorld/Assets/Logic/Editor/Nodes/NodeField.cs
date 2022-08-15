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

        public string type;
        public string desc;

     
        #endregion

        #region Save & Load
        public override void Decode(XmlElement node)
        {
            if (node.HasAttribute("name"))
                name = node.GetAttribute("name");

            if (node.HasAttribute("type"))
                type = node.GetAttribute("type");
            if (node.HasAttribute("desc"))
                desc = node.GetAttribute("desc");
        }

        public override void Encode(XmlElement node)
        {
            node.SetAttribute("name", name);

            node.SetAttribute("type", type);
            node.SetAttribute("desc", desc);
        }
        #endregion

        #region GUI
        public override void OnGUIBody()
        {
            base.OnGUIBody();

            name = EditorGUILayout.TextField("name", name);

            type = EditorGUILayout.TextField("type", type);
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

                v.type = this.type;
                v.desc = this.desc;
            }

            return obj;
        }

       

        #endregion
    }
}
