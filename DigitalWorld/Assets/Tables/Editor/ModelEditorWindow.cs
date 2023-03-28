using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Xml;
using TableGenerator;

namespace DigitalWorld.Table.Editor
{
    internal class ModelsEditorWindow : EditorWindow
    {
        #region Params
        private static ModelsEditorWindow window = null;

        private readonly List<NodeModel> models = new List<NodeModel>();
        private Vector2 scrollViewPosition;
        #endregion

        #region Window
        internal static ModelsEditorWindow FocusWindow()
        {
            if (null != window)
            {
                window.Focus();
            }
            else
            {
                window = EditorWindow.CreateWindow<ModelsEditorWindow>(typeof(TableEditorWindow), null);
                window.Show();
            }

            return window;
        }
        #endregion

        #region Mono
        private void OnEnable()
        {
            Load();
        }

        private void OnDisable()
        {
            window = null;
        }
        #endregion

        #region Logic
        private void Load()
        {
            string fullPath = Table.Utility.ModelPath;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fullPath);
            XmlElement root = xmlDocument["models"];
            if (null != root)
            {
                foreach (var node in root.ChildNodes)
                {
                    XmlElement childEle = node as XmlElement;

                    NodeModel model = new NodeModel();
                    model.Deserialize(childEle);
                    this.models.Add(model);
                }
            }
        }

        private void Save()
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlDeclaration dec = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(dec);

            XmlElement root = xmlDocument.CreateElement("models");
            if (null != root)
            {
                xmlDocument.AppendChild(root);
                root.SetAttribute("namespace", Table.Utility.defaultNamespaceName);

                foreach (NodeModel model in models)
                {
                    XmlElement ele = xmlDocument.CreateElement("model");
                    model.Serialize(ele);
                    root.AppendChild(ele);
                }

                string fullPath = Table.Utility.ModelPath;
                xmlDocument.Save(fullPath);
            }
        }

        private void ForeachSetEditing(bool isEditing)
        {
            for (int i = 0; i < this.models.Count; ++i)
            {
                models[i].IsEditing = isEditing;
            }
        }
        #endregion

        #region GUI
        private void OnGUI()
        {
            OnGUIButtons();
            OnGUITitle();

            scrollViewPosition = EditorGUILayout.BeginScrollView(scrollViewPosition);
            for (int i = 0; i < models.Count; ++i)
            {
                NodeModel model = models[i];
                model.OnGUI();
            }
            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();
            OnGUIBottom();
        }

        private void OnGUIButtons()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Generate Codes"))
            {
                GenerateCodes();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Generate Excels"))
            {
                GenerateExcels();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void OnGUIBottom()
        {
            GUIStyle style = new GUIStyle("IN Title");
            style.padding.left = 0;
            EditorGUILayout.BeginHorizontal(style);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Save"))
            {
                Save();
            }

            if (GUILayout.Button("Close"))
            {
                this.Close();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void OnGUITitle()
        {
            GUIStyle style = new GUIStyle("IN Title");
            style.padding.left = 0;

            EditorGUILayout.BeginHorizontal(style);
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField("Models", labelStyle);

            GUILayout.FlexibleSpace();

            OnGUITitleMenus();

            EditorGUILayout.EndHorizontal();
        }

        private void OnGUITitleMenus()
        {
            if (GUILayout.Button("Add"))
            {
                ModelCreateWindow window = ModelCreateWindow.DisplayWizard();
                window.OnCreateModel -= OnAddModel;
                window.OnCreateModel += OnAddModel;
            }

            if (GUILayout.Button("OpenAll"))
            {
                ForeachSetEditing(true);
            }

            if (GUILayout.Button("CloseAll"))
            {
                ForeachSetEditing(false);
            }

        }
        #endregion

        #region Listen
        private static void GenerateCodes()
        {
            Helper.GenerateCodesFromModel(Table.Utility.ModelPath, Table.Utility.CodeGeneratedPath);
           
            AssetDatabase.Refresh();
        }

        private static void GenerateExcels()
        {
            Helper.GenerateExcelsFromModel(Table.Utility.ModelPath, Table.Utility.ExcelTablePath);
        }

        private void OnAddModel(NodeModel model)
        {
            this.models.Add(model);
        }
        #endregion
    }
}
