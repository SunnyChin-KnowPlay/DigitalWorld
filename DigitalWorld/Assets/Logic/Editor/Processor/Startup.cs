﻿using System.Xml;
using UnityEditor;
using UnityEngine;
using DigitalWorld.Logic.Editor;
using DigitalWorld.Logic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

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
            Listen();
        }

        ~Startup()
        {
            Unlisten();
        }

        private static void Listen()
        {
            Selection.selectionChanged += OnSelectionChanged;

            Logic.LogicHelper.OnEditNode += OnEditNode;

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void Unlisten()
        {
            Selection.selectionChanged -= OnSelectionChanged;

            Logic.LogicHelper.OnEditNode -= OnEditNode;

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
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
                        if (path.Contains(Logic.Utility.TriggerPath))
                        {
                            // 这里说明是行为
                            OnSelectedTrigger(path);
                        }
                        //else if (path.Contains(Logic.Utility.LevelPath))
                        //{
                        //    OnSelectedLevel(path);
                        //}
                    }
                }
            }
        }


        private static void OnSelectedTrigger(string path)
        {
            string relativePath = path.Substring(Logic.Utility.ConfigsPath.Length + 1);
            if (string.IsNullOrEmpty(relativePath))
                return;

            bool ret = LogicTriggerEditorWindow.CheckHasEditing(relativePath, out LogicTriggerEditorWindow window);
            if (ret)
            {
                window.Focus();
            }
            else
            {
                TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                JsonSerializerSettings setting = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };
                Trigger trigger = JsonConvert.DeserializeObject<Trigger>(ta.text);
                trigger.RelativeFolderPath = System.IO.Path.GetDirectoryName(relativePath);

                window = LogicTriggerEditorWindow.CreateWindow<LogicTriggerEditorWindow>(typeof(LogicTriggerEditorWindow), null);
                window.Show(trigger);

                //using (StringReader reader = new StringReader(ta.text))
                //{
                //    XmlSerializer serializer = new XmlSerializer(typeof(Trigger));
                //    Trigger trigger = serializer.Deserialize(reader) as Trigger;
                //    trigger.RelativeFolderPath = System.IO.Path.GetDirectoryName(relativePath);

                //    window = LogicTriggerEditorWindow.CreateWindow<LogicTriggerEditorWindow>(typeof(LogicTriggerEditorWindow), null);
                //    window.Show(trigger);
                //}
            }
        }

        private static void OnEditNode(ENodeType nodeType, Logic.NodeBase parent, Logic.NodeBase initialNode)
        {
            LogicEffectEditorWindow window = null;
            switch (nodeType)
            {
            }

            if (null != window)
            {
                window.Show(parent, initialNode);
            }
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange c)
        {
            if (c == PlayModeStateChange.EnteredPlayMode)
            {

            }
        }
        #endregion
    }
}