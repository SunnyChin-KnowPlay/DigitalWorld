using DigitalWorld.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using TableGenerator;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DigitalWorld.Table.Editor
{
    [InitializeOnLoad]
    public class TableEditorWindow : EditorWindow
    {
        #region Params
        private readonly List<NodeModel> models = new List<NodeModel>();
        protected ReorderableList reorderableModelsList;
        #endregion

        #region Construction
        static TableEditorWindow()
        {

        }
        #endregion

        #region Logic
        private void Load()
        {
            this.models.Clear();

            string fullPath = Table.Utility.ModelPath;

            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.Load(fullPath);
            //XmlElement root = xmlDocument["models"];
            //if (null != root)
            //{
            //    foreach (var node in root.ChildNodes)
            //    {
            //        XmlElement childEle = node as XmlElement;

            //        NodeModel model = new NodeModel();
            //        model.Deserialize(childEle);
            //        this.models.Add(model);
            //    }
            //}

            reorderableModelsList = new ReorderableList(this.models, typeof(NodeField))
            {
                drawElementCallback = OnDrawFieldElement,
                drawHeaderCallback = OnDrawFieldHead,

                displayAdd = false,
                displayRemove = false,
                draggable = false,
            };
        }

        private void ExcelToXml(string name)
        {
            Helper.ConvertExcelToXml(Table.Utility.ExcelTablePath, Table.Utility.ConfigSrcPath, name);
        }

        private void XmlToExcel(string name)
        {
            Helper.ConvertXmlToExcel(Table.Utility.ConfigSrcPath, Table.Utility.ExcelTablePath, name);
        }
        #endregion

        #region Mono
        private void OnFocus()
        {
            Load();
        }
        #endregion

        #region GUI
        private void OnGUI()
        {
            OnGUIButtons();

            OnGUIModels();
        }

        protected void OnDrawFieldElement(Rect rect, int index, bool selected, bool focused)
        {
            float width = rect.width;
            if (index < models.Count)
            {
                NodeModel item = models[index];

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;

                GUIStyle labelStyle = new GUIStyle(GUI.skin.textField);

                rect.xMax = rect.xMin + width * 0.2f;
                EditorGUI.LabelField(rect, item.Name, labelStyle);

                rect.xMin = rect.xMax + 4;
                rect.xMax = rect.xMin + width * 0.38f;

                EditorGUI.LabelField(rect, item.Description, labelStyle);

                EditorGUI.BeginDisabledGroup(!selected);

                rect.xMax = width;
                rect.xMin = rect.xMax - width * 0.2f;

                if (GUI.Button(rect, "ExcelToXml"))
                {
                    ExcelToXml(item.Name);
                }

                rect.xMax = rect.xMin - 4;
                rect.xMin = rect.xMax - width * 0.2f;

                if (GUI.Button(rect, "XmlToExcel"))
                {
                    XmlToExcel(item.Name);
                }

                EditorGUI.EndDisabledGroup();
            }
            else
            {
                models.RemoveAt(index);
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
            EditorGUI.LabelField(rect, "Table Name", labelStyle);

            rect.xMin = rect.xMax + 4;
            rect.xMax = rect.xMin + width * 0.2f;
            EditorGUI.LabelField(rect, "Table Desc", labelStyle);

            rect.xMax = width;
            rect.xMin = rect.xMax - width * 0.2f;

            rect.xMax = rect.xMin - 4;
            rect.xMin = rect.xMax - width * 0.2f;
            EditorGUI.LabelField(rect, "Convert", labelStyle);
        }

        private void OnGUIModels()
        {
            EditorGUILayout.BeginVertical();

            reorderableModelsList.DoLayoutList();

            EditorGUILayout.EndVertical();
        }

        private void OnGUIButtons()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("ConvertExcelsToXmls"))
            {
                ConvertExcelsToXmls();
            }

            if (GUILayout.Button("Auto Process"))
            {
                AutoProcess();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("ConvertXmlsToExcels"))
            {
                ConvertXmlsToExcels();
            }

            if (GUILayout.Button("Edit Models"))
            {
                EditModels();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region MenuItems
        private static void EditModels()
        {
            ModelsEditorWindow window = ModelsEditorWindow.FocusWindow();
            if (null != window)
            {
                window.titleContent = new GUIContent("Models Editor");
            }
        }

        [MenuItem("Table/Editor")]
        private static void Editor()
        {
            EditorWindow.GetWindow<TableEditorWindow>("Table Editor").Show();
        }

        private static void ConvertExcelsToXmls()
        {
            Helper.ConvertExcelsToXmls(Table.Utility.ExcelTablePath, Table.Utility.ConfigSrcPath);
        }

        private static void ConvertXmlsToExcels()
        {
            Helper.ConvertXmlsToExcel(Table.Utility.ConfigSrcPath, Table.Utility.ExcelTablePath);
        }

        private static void ConvertJSONToBytes()
        {
            string bytesDirectory = Utilities.Utility.GetString(Table.Utility.configDataKey, Path.Combine(Utilities.Utility.GetProjectDataPath(), Table.Utility.defaultConfigData));
            if (!string.IsNullOrEmpty(bytesDirectory))
            {
                if (!Directory.Exists(bytesDirectory))
                    Directory.CreateDirectory(bytesDirectory);
                else
                {
                    Utility.ClearDirectory(bytesDirectory);
                }
            }

            TableManager m = TableManager.Instance;
            m.DecodeJSON();
            m.Encode();

            AssetDatabase.Refresh();
        }

        private static void CopyXmlFromConfig()
        {
            string targetPath = Utilities.Utility.GetString(Table.Utility.configXmlKey, Path.Combine(Utilities.Utility.GetProjectDataPath(), Table.Utility.defaultConfigXml));
            Utility.ClearDirectory(targetPath);

            Utility.CopyDirectory(Utilities.Utility.GetString(Table.Utility.configSrcKey, Table.Utility.defaultConfigSrc), targetPath, null);
            AssetDatabase.Refresh();
        }

        private static void AutoProcess()
        {
            CopyXmlFromConfig();
            ConvertJSONToBytes();
        }
        #endregion




    }

}

