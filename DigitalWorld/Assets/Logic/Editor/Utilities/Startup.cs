using System.Xml;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    [InitializeOnLoad]
    internal class Startup
    {
        /// <summary>
        /// 启动时，call一下单例，做准备工作
        /// </summary>
        static Startup()
        {
            _ = NodeController.instance;

            Listen();
        }

        ~Startup()
        {
            Unlisten();
        }

        private static void Listen()
        {
            Selection.selectionChanged += OnSelectionChanged;

            Logic.LogicHelper.OnAddNode += OnAddNode;
            Logic.LogicHelper.OnEditNode += OnEditNode;
        }

        private static void Unlisten()
        {
            Selection.selectionChanged -= OnSelectionChanged;

            Logic.LogicHelper.OnAddNode -= OnAddNode;
            Logic.LogicHelper.OnEditNode -= OnEditNode;
        }

        #region Listen
        private static void OnSelectionChanged()
        {
            UnityEngine.Object obj = Selection.activeObject;
            if (null != obj)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (!string.IsNullOrEmpty(path)) // 先看一下路径是否存在 
                {
                    // 然后看一下是否为文件
                    if (System.IO.File.Exists(path))
                    {
                        // 这里区别处理一下 看到底是behaviour还是trigger之类的

                        if (path.Contains(Logic.Utility.BehaviourPath))
                        {
                            // 这里说明是行为
                            OnSelectedBehaviour(path);
                        }
                    }
                }
            }
        }

        private static void OnSelectedBehaviour(string path)
        {
            string relativePath = path.Substring(DigitalWorld.Logic.Utility.ConfigsPath.Length + 1);
            if (string.IsNullOrEmpty(relativePath))
                return;

            bool ret = LogicBehaviourEditorWindow.CheckHasEditing(relativePath, out LogicBehaviourEditorWindow window);
            if (ret)
            {
                window.Focus();
            }
            else
            {
                Behaviour behaviour = new Behaviour();
                behaviour.RelativeFolderPath = System.IO.Path.GetDirectoryName(relativePath);
                TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                if (null != ta)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(ta.text);

                    XmlElement root = doc["behaviour"];
                    if (null != root)
                    {
                        behaviour.DecodeXml(root);
                    }
                }

                window = LogicBehaviourEditorWindow.CreateWindow<LogicBehaviourEditorWindow>(typeof(LogicBehaviourEditorWindow), null);
                window.Show(behaviour);
            }
        }

        private static void OnAddNode(ENodeType nodeType, Logic.NodeBase parent)
        {
            LogicEffectEditorWindow window = null;
            switch (nodeType)
            {
                case ENodeType.Action:
                {
                    window = LogicActionEditorWindow.GetWindow() as LogicEffectEditorWindow;
                    break;
                }
            }

            if (null != window)
            {
                window.Show(parent);
            }
        }

        private static void OnEditNode(ENodeType nodeType, Logic.NodeBase parent, Logic.NodeBase initialNode)
        {
            LogicEffectEditorWindow window = null;
            switch (nodeType)
            {
                case ENodeType.Action:
                {
                    window = LogicActionEditorWindow.GetWindow() as LogicEffectEditorWindow;
                    break;
                }

            }

            if (null != window)
            {
                window.Show(parent, initialNode);
            }
        }
        #endregion
    }
}
