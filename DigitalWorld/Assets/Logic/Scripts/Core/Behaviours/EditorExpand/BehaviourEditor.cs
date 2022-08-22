#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
#endif

namespace DigitalWorld.Logic
{
    public partial class Behaviour
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
                string path = value.Replace('\\', '/');
                relativeFolderPath = path;
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

        internal override bool IsEditing => true;

        protected EAction selectedAction;

        public static string[] ActionDisplayNames
        {
            get
            {
                if (null == actionDisplayNames)
                {
                    List<string> actionNames = new List<string>();

                    EAction[] actionArray = ActionArray;
                    for (int i = 0; i < actionArray.Length; ++i)
                    {
                        string name = actionArray[i].ToString().Replace('_', '/');
                        name += string.Format(" - {0}", LogicHelper.GetActionDesc((int)actionArray[i]));
                        actionNames.Add(name);
                    }

                    actionDisplayNames = actionNames.ToArray();
                }
                return actionDisplayNames;
            }
        }
        private static string[] actionDisplayNames = null;

        public static EAction[] ActionArray
        {
            get
            {
                if (null == actionArray)
                {
                    List<EAction> actionList = new List<EAction>();
                    foreach (EAction i in System.Enum.GetValues(typeof(EAction)))
                    {
                        actionList.Add(i);
                    }

                    actionArray = actionList.ToArray();
                }
                return actionArray;
            }
        }
        private static EAction[] actionArray = null;

        public static EAction FindAction(int index)
        {
            if (index < 0 || index >= ActionArray.Length)
                return ActionArray[0];

            return ActionArray[index];
        }

        public static int FindActionIndex(EAction v)
        {
            int index = 0;

            EAction[] types = ActionArray;

            for (int i = 0; i < types.Length; ++i)
            {
                if (types[i] == v)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        #endregion

        #region GUI
        public override void OnGUITitle()
        {
            GUIStyle style = new GUIStyle("IN Title");
            style.padding.left = 0;

            EditorGUILayout.BeginHorizontal(style);

            this._enabled = EditorGUILayout.Toggle(_enabled, GUILayout.Width(18));

            this.OnGUIName();
            GUILayout.FlexibleSpace();

            OnGUITopMenus();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 顶部操作菜单
        /// </summary>
        private void OnGUITopMenus()
        {
            if (GUILayout.Button("OpenAll", GUILayout.Width(79)))
            {
                for (int i = 0; i < this._children.Count; ++i)
                {
                    this._children[i].IsEditing = true;
                }
            }

            if (GUILayout.Button("CloseAll", GUILayout.Width(79)))
            {
                for (int i = 0; i < this._children.Count; ++i)
                {
                    this._children[i].IsEditing = false;
                }
            }
        }

        protected virtual void OnGUIExplore()
        {
            EditorGUILayout.BeginHorizontal();

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField("Action Explore", labelStyle, GUILayout.Width(160));

            GUILayout.FlexibleSpace();

            if (Enum.GetValues(typeof(EAction)) != null && Enum.GetValues(typeof(EAction)).Length > 0)
            {

                selectedAction = FindAction(EditorGUILayout.Popup(FindActionIndex(selectedAction), ActionDisplayNames, GUILayout.Width(300)));

                if (GUILayout.Button("Create Action", GUILayout.Width(160)))
                {
                    OnClickCreateAction();
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        protected override void OnGUIEditing()
        {
            this.OnGUIExplore();

            base.OnGUIEditing();
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
        private bool CheckCanSave()
        {
            //Utility.uidDict.Clear();

            //foreach (var v in triggers)
            //{
            //    v.AddUidToDict();
            //}

            //foreach (var v in steps)
            //{
            //    v.AddUidToDict();
            //}

            //string errIndex = null;
            //if (Utility.CheckUidRepeated(ref errIndex))
            //{
            //    string body = string.Format("当前包含重复的key，无法保存 key是{0}", errIndex);
            //    bool ret = EditorUtility.DisplayDialog("错误", body, "确定");
            //    return false;
            //}
            return true;
        }

        private void SaveStream()
        {
            int size = this.CalculateSize();
            byte[] data = new byte[size];
            this.Encode(data, 0);

            string text = System.Text.Encoding.UTF8.GetString(data, 0, size);

            string fullPath = System.IO.Path.Combine(RelativeFolderPath, this.Name);
            fullPath += ".asset";
            fullPath = System.IO.Path.Combine(Utility.LogicExportPath, fullPath);

            TextAsset ta = new TextAsset(text);
            AssetDatabase.DeleteAsset(fullPath);
            AssetDatabase.CreateAsset(ta, fullPath);
        }

        private void SaveXml()
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlDeclaration dec = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(dec);

            XmlElement root = xmlDocument.CreateElement("behaviour");
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
                bool ret = this.CheckCanSave();
                if (!ret)
                    return;

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
