using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal abstract class NodeItem : NodeBase
    {
        #region Params
        public delegate void OnCallbackModifyItem(EItemType type, NodeItem ba);

        protected string desc = "";
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }

        protected List<NodeField> fields = new List<NodeField>();

        protected readonly ReorderableList fieldList;
        #endregion

        #region Common
        internal NodeItem()
        {
            fieldList = new ReorderableList(fields, typeof(NodeField))
            {
                drawElementCallback = OnDrawFieldElement,
                onAddCallback = (list) => OnAddField(),
                onRemoveCallback = (list) => OnRemoveField(),
                drawHeaderCallback = OnDrawFieldHead,
                draggable = true,
            };
        }

        #endregion

        #region GUI
        protected virtual void OnDrawFieldElement(Rect rect, int index, bool selected, bool focused)
        {
            float width = rect.width;
            if (index < fields.Count)
            {
                NodeField item = fields[index];

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;

                rect.xMax = rect.xMin + width * 0.2f;
                item.Name = EditorGUI.TextField(rect, item.Name);

                rect.xMin = rect.xMax + 4;
                rect.xMax = rect.xMin + width * 0.4f;
                item.typeName = NodeField.FindTypeName(EditorGUI.Popup(rect, NodeField.FindTypeIndex(item.typeName), NodeField.TypeDisplayArray));
                System.Type type = NodeField.FindType(NodeField.FindTypeIndex(item.typeName));
                item.baseTypeName = type.BaseType.ToString();

                rect.xMin = rect.xMax + 4;
                rect.xMax = width;

                item.desc = EditorGUI.TextField(rect, item.desc);
            }
            else
            {
                fields.RemoveAt(index);
            }
        }

        private void OnDrawFieldHead(Rect rect)
        {
            EditorGUI.LabelField(rect, "Fields");
        }

        protected virtual void OnAddField()
        {
            fields.Add(new NodeField());
        }

        protected virtual void OnRemoveField()
        {
            fields.RemoveAt(fieldList.index);
        }

        public override void OnGUIBody()
        {
            base.OnGUIBody();

            fieldList.DoLayoutList();
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
            if (base.CloneTo(obj) is NodeItem v)
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
