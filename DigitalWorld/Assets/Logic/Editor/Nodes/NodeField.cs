using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using System;

namespace DigitalWorld.Logic.Editor
{


    public class NodeField : NodeBase
    {
        /*
         *  <field name="unitKey" baseClassT="" classT="int" desc="单位key">0</field>
         */
        public string baseClassT;
        public string classT;
        public string desc;

        #region Save & Load
        public override void Decode(XmlElement node)
        {
            if (node.HasAttribute("name"))
                name = node.GetAttribute("name");
            if (node.HasAttribute("baseClassT"))
                baseClassT = node.GetAttribute("baseClassT");
            if (node.HasAttribute("classT"))
                classT = node.GetAttribute("classT");
            if (node.HasAttribute("desc"))
                desc = node.GetAttribute("desc");
        }

        public override void Encode(XmlElement node)
        {
            node.SetAttribute("name", name);
            node.SetAttribute("baseClassT", baseClassT);
            node.SetAttribute("classT", classT);
            node.SetAttribute("desc", desc);
        }
        #endregion

        #region GUI
        public override void OnGUIBody()
        {
            base.OnGUIBody();

            name = EditorGUILayout.TextField("name", name);
            baseClassT = EditorGUILayout.TextField("baseClassT", baseClassT);
            classT = EditorGUILayout.TextField("classT", classT);
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
                v.baseClassT = this.baseClassT;
                v.classT = this.classT;
                v.desc = this.desc;
            }

            return obj;
        }
        #endregion
    }
}
