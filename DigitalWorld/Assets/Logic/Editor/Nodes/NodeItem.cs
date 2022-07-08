using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal abstract class NodeItem : NodeBase
    {
        private Vector2 offset = Vector2.zero;

        public delegate void OnCallbackModifyItem(EItemType type, NodeItem ba);

        protected string desc = "";
        public string Desc
        {
            get { return desc; }
        }
        protected List<NodeField> fields = new List<NodeField>();
        private static List<NodeField> processFields = new List<NodeField>();

        public List<NodeField> Fields
        {
            get { return fields; }
        }

        protected bool editingFields = false;
        public bool EditingFields
        {
            get { return editingFields; }
            set { editingFields = value; }
        }

        #region GUI
        public override void OnGUITitle()
        {
            base.OnGUITitle();

            string field = string.Format("{0} - {1}", this.id, this.name);
            EditorGUILayout.LabelField(field);

        }

        public override void OnGUIBody()
        {
            base.OnGUIBody();

            EditorGUILayout.BeginHorizontal();
            editingFields = EditorGUILayout.Foldout(editingFields, "字段");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("新增"))
            {
                fields.Add(new NodeField());
            }

            if (GUILayout.Button("打开所有"))
            {
                editingFields = true;
                for (int i = 0; i < fields.Count; ++i)
                {
                    fields[i].Editing = true;
                }
            }

            if (GUILayout.Button("关闭所有"))
            {
                editingFields = false;
                for (int i = 0; i < fields.Count; ++i)
                {
                    fields[i].Editing = false;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (editingFields && fields.Count > 0)
            {
                GUIStyle style = new GUIStyle(GUI.skin.box);

                offset = EditorGUILayout.BeginScrollView(offset, style);

                processFields.Clear();
                processFields.AddRange(fields);

                for (int i = 0; i < processFields.Count; ++i)
                {
                    string name = string.Format("字段{0}", i + 1);

                    EditorGUILayout.BeginHorizontal();
                    processFields[i].Editing = EditorGUILayout.Foldout(processFields[i].Editing, name);

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("删除"))
                    {
                        fields.Remove(processFields[i]);
                    }

                    EditorGUILayout.EndHorizontal();

                    if (processFields[i].Editing)
                    {
                        EditorGUILayout.BeginVertical(style);
                        processFields[i].OnGUIBody();
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
        }

        public virtual void OnGUIParams(bool editing = false)
        {
            if (editing)
            {
                EditorGUILayout.LabelField("id", this.id.ToString());
                EditorGUILayout.LabelField("name", this.name);
            }
            else
            {
                this.Id = EditorGUILayout.IntField("id", this.id);
                this.Name = EditorGUILayout.TextField("name", this.name);
            }

            this.desc = EditorGUILayout.TextField("desc", this.desc);
        }

        #endregion

        #region Save & Load
        public override void Decode(XmlElement node)
        {
            base.Decode(node);

            if (node.HasAttribute("desc"))
            {
                this.desc = node.GetAttribute("desc");
            }

            this.fields.Clear();

            for (int i = 0; i < node.ChildNodes.Count; ++i)
            {
                XmlElement fieldEle = node.ChildNodes[i] as XmlElement;
                NodeField fn = new NodeField();
                fn.Decode(fieldEle);
                this.fields.Add(fn);
            }

        }

        public override void Encode(XmlElement node)
        {
            base.Encode(node);

            node.SetAttribute("desc", this.desc);


            for (int i = 0; i < fields.Count; ++i)
            {
                XmlElement fieldEle = node.OwnerDocument.CreateElement("field");
                node.AppendChild(fieldEle);

                this.fields[i].Encode(fieldEle);
            }
        }
        #endregion

        #region Common
        public NodeField FindField(string name)
        {
            for (int i = 0; i < fields.Count; ++i)
            {
                if (fields[i].Name == name)
                    return fields[i];
            }

            return null;
        }

        public override T CloneTo<T>(T obj)
        {
            NodeItem v = base.CloneTo(obj) as NodeItem;
            if (null != v)
            {
                v.desc = this.desc;

                v.fields.Clear();
                for (int i = 0; i < fields.Count; ++i)
                {
                    v.fields.Add(fields[i].Clone() as NodeField);
                }
            }

            return obj;


        }
        #endregion

    }
}
