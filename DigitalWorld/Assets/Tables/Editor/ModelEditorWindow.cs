using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Xml;

namespace DigitalWorld.Table.Editor
{
    internal class ModelsEditorWindow : EditorWindow
    {
        #region Params
        private static ModelsEditorWindow window = null;

        private List<NodeModel> models = new List<NodeModel>();
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
            string fullPath = Utility.defaultModelPath;

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

        private void OnDisable()
        {
            window = null;
        }
        #endregion

        #region GUI
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < models.Count; ++i)
            {
                NodeModel model = models[i];
                model.OnGUI();
            }

            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();


        }
        #endregion
    }
}
