﻿#if UNITY_EDITOR
using DigitalWorld.Asset;
using System;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
#endif

namespace DigitalWorld.Logic
{
    public partial class Trigger
    {
#if UNITY_EDITOR
        #region Params
        /// <summary>
        /// 文件的相对路径 基于logic的相对和基于配置流的相对一致
        /// </summary>
        public string RelativeFolderPath
        {
            get => relativeFolderPath;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string path = value.Replace('\\', '/');
                    relativeFolderPath = path;
                }
                else
                {
                    relativeFolderPath = value;
                }
            }
        }
        public string RelativeAssetFilePath
        {
            get
            {
                string path = System.IO.Path.Combine(RelativeFolderPath, _name);
                path += ".asset";
                return path.Replace('\\', '/');
            }
        }
        private string relativeFolderPath;

        public override bool IsEditing
        {
            get
            {
                if (this.IsRoot)
                    return true;
                return base.IsEditing;
            }
        }
        #endregion

        #region GUI
        protected override void OnGUIChildrenTitle()
        {
            base.OnGUIChildrenTitle();

            this.OnGUIActionExplore();
        }

        protected override void OnGUITitleContent()
        {
            this.OnGUIEventExplore();
            base.OnGUITitleContent();
        }

        protected void OnGUIActionExplore()
        {
            GUIStyle style = new GUIStyle("OL Title");
            style.padding.left = 0;

            Color color = GUI.backgroundColor;

            using (EditorGUILayout.HorizontalScope h = new EditorGUILayout.HorizontalScope(style))
            {
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontStyle = FontStyle.Bold,
                };
                labelStyle.normal.textColor = this.GetNodeColor(typeof(Actions.ActionBase)) * 2f;

                EditorGUILayout.LabelField("Action Explore", labelStyle, GUILayout.Width(160));

                GUILayout.FlexibleSpace();

                if (Enum.GetValues(typeof(EAction)) != null && Enum.GetValues(typeof(EAction)).Length > 0)
                {
                    selectedAction = FindAction(EditorGUILayout.Popup(FindActionIndex(selectedAction), ActionDisplayNames, GUILayout.Width(500)));

                    labelStyle = new GUIStyle(GUI.skin.button)
                    {
                        //fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleCenter,
                    };
                    labelStyle.hover.textColor = this.GetNodeColor(typeof(Actions.ActionBase)) * 2f;

                    if (GUILayout.Button("Create Action", labelStyle, GUILayout.Width(160)))
                    {
                        OnClickCreateAction();
                    }
                }
            }

            GUI.backgroundColor = color;
        }

        protected virtual void OnGUIEventExplore()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);

            EditorGUILayout.LabelField("Event Listening", labelStyle, GUILayout.Width(100));

            if (Enum.GetValues(typeof(EEvent)) != null && Enum.GetValues(typeof(EEvent)).Length > 0)
            {
                GUIStyle popStyle = new GUIStyle("MiniPopup")
                {
                    //fontStyle = FontStyle.Bold
                    margin = new RectOffset(3, 3, 0, 0),
                };
                _listenerEvent = FindEvent(EditorGUILayout.Popup(FindEventIndex(_listenerEvent), EventDisplayNames, popStyle, GUILayout.Width(300)));
            }
        }

        private void OnClickCreateAction()
        {
            NodeBase node = Utility.CreateNewAction(this.selectedAction);
            if (null != node)
            {
                node.SetParent(this);
            }
        }
        #endregion

        #region Common

        private void SaveStream()
        {
            int size = this.CalculateSize();
            byte[] data = new byte[size];
            this.Encode(data, 0);

            string fullPath = System.IO.Path.Combine(RelativeFolderPath, this.Name);
            fullPath += ".asset";
            fullPath = System.IO.Path.Combine(Utility.LogicResPath, fullPath);

            string directorPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directorPath))
            {
                Directory.CreateDirectory(directorPath);
            }

            AssetDatabase.DeleteAsset(fullPath);
            ByteAsset.CreateAsset(data, fullPath);

        }

        private void SaveXml()
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlDeclaration dec = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(dec);

            XmlElement root = xmlDocument.CreateElement("node");
            xmlDocument.AppendChild(root);

            this.EncodeXml(root);

            string fullPath = System.IO.Path.Combine(RelativeFolderPath, this.Name);
            fullPath += ".asset";
            fullPath = System.IO.Path.Combine(Utility.ConfigsPath, fullPath);


            TextAsset ta = new TextAsset(xmlDocument.InnerXml);
            AssetDatabase.DeleteAsset(fullPath);
            AssetDatabase.CreateAsset(ta, fullPath);

        }

        public virtual void Save()
        {
            if (!string.IsNullOrEmpty(RelativeFolderPath))
            {
                try
                {
                    this.CheckCanSave();
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }

                SaveXml();
                SaveStream();

                this.ResetDirty();
                AssetDatabase.Refresh();
            }
        }
        #endregion
#endif
    }
}
