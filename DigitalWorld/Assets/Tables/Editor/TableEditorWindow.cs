using DigitalWorld.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Table.Editor
{
    [InitializeOnLoad]
    public class TableEditorWindow : EditorWindow
    {
        static TableEditorWindow()
        {
            //PlayerPrefs.DeleteAll();

            //Utilities.Utility.SetDefaultString(Utility.outputCodeKey, Path.Combine(Application.dataPath, Utility.defaultOutPutCodePath));
            //Utilities.Utility.SetDefaultString(Utility.modelKey, Utility.defaultModelPath);
            //Utilities.Utility.SetDefaultString(Utility.excelKey, Utility.defaultExcelPath);
            //Utilities.Utility.SetDefaultString(Utility.configXmlKey, Path.Combine(Application.dataPath, Utility.defaultConfigXml));
            //Utilities.Utility.SetDefaultString(Utility.configDataKey, Path.Combine(Application.dataPath, Utility.defaultConfigData));
        }

        #region GUI
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("Auto Process"))
            {
                AutoProcess();
            }

            if (GUILayout.Button("Generate Codes"))
            {
                GenerateCodes();
            }

            if (GUILayout.Button("Generate Excels"))
            {
                GenerateExcels();
            }

            if (GUILayout.Button("ConvertExcelsToXmls"))
            {
                ConvertExcelsToXmls();
            }

            if (GUILayout.Button("ConvertXmlsToExcels"))
            {
                ConvertXmlsToExcels();
            }

            if (GUILayout.Button("Edit Models"))
            {
                EditModels();
            }

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

