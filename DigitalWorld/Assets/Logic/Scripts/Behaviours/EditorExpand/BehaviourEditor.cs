#if UNITY_EDITOR
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
        #endregion

        #region GUI
        public override void OnGUITitle()
        {
            GUIStyle style = new GUIStyle("IN Title");
            style.padding.left = 0;

            EditorGUILayout.BeginHorizontal(style);

            bool old = _enabled;
            this._enabled = EditorGUILayout.Toggle(old, GUILayout.Width(18));
            if (old != _enabled)
            {
                if (this.Parent != null)
                    this.Parent.SetDirty();
            }

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
            if (GUILayout.Button("Create Action"))
            {
                OnClickCreateAction();
            }

            if (GUILayout.Button("OpenAll"))
            {
                for (int i = 0; i < this._children.Count; ++i)
                {
                    this._children[i].IsEditing = true;
                }
            }

            if (GUILayout.Button("CloseAll"))
            {
                for (int i = 0; i < this._children.Count; ++i)
                {
                    this._children[i].IsEditing = false;
                }
            }
        }

        protected override void OnGUIEditing()
        {
            base.OnGUIEditing();
        }

        private void OnClickCreateAction()
        {
            LogicHelper.ApplyAddNode(ENodeType.Action, this);
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
