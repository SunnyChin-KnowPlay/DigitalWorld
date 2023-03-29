using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Xml;
using TableGenerator;
using Newtonsoft.Json;
using System.IO;
using DigitalWorld.Logic;

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

            using FileStream fs = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new StreamReader(fs);
            string jsonResult = streamReader.ReadToEnd();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            Model model = JsonConvert.DeserializeObject<Model>(jsonResult, settings);
            this.models.Clear();
            this.models.AddRange(model.models);
        }

        private void Save()
        {
            Model model = new Model
            {
                NamespaceName = Table.Utility.defaultNamespaceName,
                models = this.models,
            };

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Newtonsoft.Json.Formatting.Indented,
            };

            string fullPath = Table.Utility.ModelPath;
            using FileStream fs = File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using StreamWriter sw = new StreamWriter(fs);
            string jsonResult = JsonConvert.SerializeObject(model, settings);
            sw.Write(jsonResult);

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
