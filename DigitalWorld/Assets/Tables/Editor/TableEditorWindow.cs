using DigitalWorld.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Table.Editor
{
    [InitializeOnLoad]
    public class TableEditorWindow : EditorWindow
    {
        #region Params
        private readonly List<NodeModel> models = new List<NodeModel>();
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
            Rect position = this.position;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();

         
            if (GUILayout.Button("Generate Codes", GUILayout.Width(position.width * 0.5f)))
            {
                GenerateCodes();
            }

            if (GUILayout.Button("Generate Excels", GUILayout.Width(position.width * 0.5f)))
            {
                GenerateExcels();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("ConvertExcelsToXmls", GUILayout.Width(position.width * 0.5f)))
            {
                ConvertExcelsToXmls();
            }

            if (GUILayout.Button("ConvertXmlsToExcels", GUILayout.Width(position.width * 0.5f)))
            {
                ConvertXmlsToExcels();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();


            if (GUILayout.Button("Auto Process", GUILayout.Width(position.width * 0.5f)))
            {
                AutoProcess();
            }

            if (GUILayout.Button("Edit Models", GUILayout.Width(position.width * 0.5f)))
            {
                EditModels();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

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

        [MenuItem("Table/Generate/Generate Codes")]
        private static void GenerateCodes()
        {
            Utility.ExecuteTableGenerate(Utility.GenerateCodesCmd);

            AssetDatabase.Refresh();
        }

        [MenuItem("Table/Generate/Generate Excels")]
        private static void GenerateExcels()
        {
            Utility.ExecuteTableGenerate(Utility.GenerateExcelsCmd);
        }

        [MenuItem("Table/Convert/ConvertExcelsToXmls")]
        private static void ConvertExcelsToXmls()
        {
            Utility.ExecuteTableGenerate(Utility.ConvertExcelsToXmlsCmd);
        }

        [MenuItem("Table/Convert/ConvertXmlsToExcels")]
        private static void ConvertXmlsToExcels()
        {
            Utility.ExecuteTableGenerate(Utility.ConvertXmlsToExcelsCmd);
        }

        [MenuItem("Table/Convert/ConvertXmlToBytes")]
        private static void ConvertXmlToBytes()
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
            m.DecodeXml();
            m.Encode();

            AssetDatabase.Refresh();
        }

        [MenuItem("Table/CopyXmlFromConfig")]
        private static void CopyXmlFromConfig()
        {
            string targetPath = Utilities.Utility.GetString(Table.Utility.configXmlKey, Path.Combine(Utilities.Utility.GetProjectDataPath(), Table.Utility.defaultConfigXml));
            Utility.ClearDirectory(targetPath);

            Utility.CopyDirectory(Utilities.Utility.GetString(Table.Utility.configSrcKey, Table.Utility.defaultConfigSrc), targetPath, null);
            AssetDatabase.Refresh();
        }

        [MenuItem("Table/Auto Process")]
        private static void AutoProcess()
        {
            GenerateCodes();

            CopyXmlFromConfig();
            ConvertXmlToBytes();
        }
        #endregion




    }

}

