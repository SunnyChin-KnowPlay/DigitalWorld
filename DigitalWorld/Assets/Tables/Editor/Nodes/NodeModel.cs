using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Xml;

namespace DigitalWorld.Table.Editor
{
    internal class NodeModel : NodeBase
    {
        #region Params
        protected ReorderableList reorderableFieldsList;

        protected List<NodeField> fieldList = new List<NodeField>();
        #endregion

        internal NodeModel()
        {
            reorderableFieldsList = new ReorderableList(this.fieldList, typeof(NodeField))
            {
                drawElementCallback = OnDrawFieldElement,
                drawHeaderCallback = OnDrawFieldHead,
                displayAdd = true,
                displayRemove = true,
                draggable = false,
            };
        }

        #region GUI
        protected void OnDrawFieldElement(Rect rect, int index, bool selected, bool focused)
        {
            float width = rect.width;
            if (index < fieldList.Count)
            {
                NodeField item = fieldList[index];

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;

                rect.xMax = rect.xMin + width * 0.2f;
                item.Name = EditorGUI.TextField(rect, item.Name);

                rect.xMin = rect.xMax + 4;
                rect.xMax = rect.xMin + width * 0.4f;

                string typeName = item.Type.FullName;

                typeName = NodeField.FindTypeName(EditorGUI.Popup(rect, NodeField.FindTypeIndex(typeName), NodeField.TypeDisplayArray));
                System.Type type = NodeField.FindType(NodeField.FindTypeIndex(typeName));
                item.Type = type;
                //item.baseTypeName = type.BaseType.ToString();

                rect.xMin = rect.xMax + 4;
                rect.xMax = width;

                item.Description = EditorGUI.TextField(rect, item.Description);
            }
            else
            {
                fieldList.RemoveAt(index);
            }
        }



        protected void OnDrawFieldHead(Rect rect)
        {

        }

        protected override void OnGUIEditing()
        {
            base.OnGUIEditing();

            reorderableFieldsList.DoLayoutList();
        }
        #endregion

        #region Serialize
        public override void Serialize(XmlElement root)
        {
            base.Serialize(root);


        }

        public override void Deserialize(XmlElement root)
        {
            base.Deserialize(root);

            foreach (var node in root.ChildNodes)
            {
                XmlElement childEle = node as XmlElement;

                NodeField field = new NodeField();
                field.Deserialize(childEle);
                this.fieldList.Add(field);
            }
        }
        #endregion

    }
}
