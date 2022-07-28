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
        #endregion

        #region GUI
        protected override void OnGUIBody()
        {
            base.OnGUIBody();

            GUIStyle style = new GUIStyle("Tooltip");

            string srcDesc = this._description;
            this._description = EditorGUILayout.TextArea(this._description, GUILayout.MinHeight(20));
            if (srcDesc != _description)
            {
                this.SetDirty();
            }

            EditorGUILayout.BeginVertical(style);
            OnGUIEffects();
            EditorGUILayout.EndVertical();
        }

        protected virtual void OnGUIEffects()
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                Effect effect = this._children[i] as Effect;
                if (null != effect)
                {
                    effect.OnGUI();
                }
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
            AssetDatabase.CreateAsset(ta, fullPath);
        }

        private void SaveXml()
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlDeclaration dec = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(dec);

            XmlElement root = xmlDocument.CreateElement("behaviour");
            xmlDocument.AppendChild(root);


            this.Encode(root);

            string fullPath = System.IO.Path.Combine(RelativeFolderPath, this.Name);
            fullPath += ".asset";
            fullPath = System.IO.Path.Combine(Utility.ConfigsPath, fullPath);


            TextAsset ta = new TextAsset(xmlDocument.InnerXml);
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
