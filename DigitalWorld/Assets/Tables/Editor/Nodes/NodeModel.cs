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

        #region Construction
        internal NodeModel()
        {
            reorderableFieldsList = new ReorderableList(this.fieldList, typeof(NodeField))
            {
                drawElementCallback = OnDrawFieldElement,
                drawHeaderCallback = OnDrawFieldHead,
                onAddCallback = (list) => OnAddField(),
                onRemoveCallback = (list) => OnRemoveField(),
                displayAdd = true,
                displayRemove = true,
                draggable = false,
            };
        }
        #endregion

        #region GUI
        private void OnAddField()
        {
            NodeField field = new NodeField();
            fieldList.Add(field);
        }

        private void OnRemoveField()
        {
            fieldList.RemoveAt(reorderableFieldsList.index);
        }

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

                string typeName = null != item.Type ? item.Type.FullName : "None";

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
            float width = rect.width;

            rect.height = EditorGUIUtility.singleLineHeight;

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,

            };

            rect.xMax = rect.xMin + width * 0.2f;
            EditorGUI.LabelField(rect, "Key", labelStyle);

            rect.xMin = rect.xMax + 4;
            rect.xMax = rect.xMin + width * 0.4f;

            EditorGUI.LabelField(rect, "Type", labelStyle);
          
            rect.xMin = rect.xMax + 4;
            rect.xMax = width;

            EditorGUI.LabelField(rect, "Desc", labelStyle);
        }

        public override void OnGUITitle()
        {
            GUIStyle style = new GUIStyle("IN Title");
            style.padding.left = 0;

            Color color = GUI.backgroundColor;
            GUI.backgroundColor = new Color32(160, 160, 160, 255);

            using EditorGUILayout.HorizontalScope h = new EditorGUILayout.HorizontalScope(style);
            GUI.backgroundColor = color;

            isEditing = EditorGUILayout.Toggle("", isEditing, EditorStyles.foldout, GUILayout.Width(12));


            this.OnGUIName();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("ExcelToXml"))
            {
                ExcelToXml();
            }

            if (GUILayout.Button("XmlToExcel"))
            {
                XmlToExcel();
            }

            if (GUI.Button(h.rect, GUIContent.none, EditorStyles.whiteLabel))
            {
                OnClickTitle(h);
            }
        }

        protected override void OnGUIEditing()
        {
            base.OnGUIEditing();

            reorderableFieldsList.DoLayoutList();
        }
        #endregion

        #region Logic
        private void ExcelToXml()
        {
            string content = string.Format($"确定要将\"{Name}\"的Excel表格导出到Xml配置吗？");
            if (EditorUtility.DisplayDialog("ExcelToXml", content, "导出", "取消"))
            {
                Utility.ExecuteTableGenerate(Utility.ConvertExcelToXmlCmd, this.Name);
            }
        }

        private void XmlToExcel()
        {
            string content = string.Format($"确定要将\"{Name}\"的Xml配置导入到Excel表格吗？");
            if (EditorUtility.DisplayDialog("ExcelToXml", content, "导入", "取消"))
            {
                Utility.ExecuteTableGenerate(Utility.ConvertXmlToExcelCmd, this.Name);
            }

        }
        #endregion

        #region Serialize
        public override void Serialize(XmlElement root)
        {
            base.Serialize(root);

            foreach (NodeField field in fieldList)
            {
                XmlElement ele = root.OwnerDocument.CreateElement("field");
                field.Serialize(ele);
                root.AppendChild(ele);
            }
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
