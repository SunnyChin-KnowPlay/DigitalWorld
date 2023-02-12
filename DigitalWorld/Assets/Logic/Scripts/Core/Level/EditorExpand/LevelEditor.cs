using System.Xml;
using UnityEngine;
using DigitalWorld.Asset;
using DigitalWorld.Logic;
using DigitalWorld.Logic.Properties;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DigitalWorld.Logic
{
    public partial class Level
    {
#if UNITY_EDITOR
        #region Params
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
        protected override void OnGUITitleContent()
        {
            GUIStyle style = new GUIStyle("minibuttonmid")
            {
                margin = new RectOffset(2, 2, 1, 1),
            };

            if (GUILayout.Button(new GUIContent("Open Actions"), style, GUILayout.Width(120), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                OnClickOpenActions();
            }

            if (GUILayout.Button(new GUIContent("Close Actions"), style, GUILayout.Width(120), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                OnClickCloseActions();
            }

            if (GUILayout.Button(new GUIContent("Open Properties"), style, GUILayout.Width(120), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                OnClickOpenProperties();
            }

            if (GUILayout.Button(new GUIContent("Close Properties"), style, GUILayout.Width(120), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                OnClickCloseProperties();
            }

            if (GUILayout.Button(new GUIContent("Add Trigger"), style, GUILayout.Width(120), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                OnClickAddTrigger();
            }

            base.OnGUITitleContent();
        }
        #endregion

        #region Listen
        protected virtual void OnClickOpenActions()
        {
            this.ForeachChildren((NodeBase node) =>
            {
                if (node is Actions.ActionBase pb)
                {
                    pb.IsEditing = true;
                }
            });
        }

        protected virtual void OnClickCloseActions()
        {
            this.ForeachChildren((NodeBase node) =>
            {
                if (node is Actions.ActionBase pb)
                {
                    pb.IsEditing = false;
                }
            });
        }

        protected virtual void OnClickOpenProperties()
        {
            this.ForeachChildren((NodeBase node) =>
            {
                if (node is PropertyBase pb)
                {
                    pb.IsEditing = true;
                }
            });
        }

        protected virtual void OnClickCloseProperties()
        {
            this.ForeachChildren((NodeBase node) =>
            {
                if (node is PropertyBase pb)
                {
                    pb.IsEditing = false;
                }
            });
        }

        protected virtual void OnClickAddTrigger()
        {
            Trigger behaviour = new Trigger
            {
                Name = FindNewTriggerName(),
                RelativeFolderPath = null,
            };

            behaviour.SetParent(this);
        }
        #endregion

        #region Common
        /// <summary>
        /// 获取一个新的可用的触发器名字
        /// </summary>
        /// <returns>可使用的触发器名字</returns>
        protected string FindNewTriggerName()
        {
            int index = 0;

            while (index < 9999999)
            {
                string name = string.Format("Trigger_{0}", index);
                bool v = JudgeChildNameCanUse(name);
                if (v)
                {
                    return name;
                }
                ++index;
            }

            return null;
        }

        public override void CheckCanSave()
        {
            base.CheckCanSave();


        }
        private void SaveStream()
        {
            int size = this.CalculateSize();
            byte[] data = new byte[size];
            this.Encode(data, 0);


            _ = Utility.AESEncrypt(data, 0, size, Utility.GetKey(), out bool ret);
            if (ret)
            {
                string fullPath = System.IO.Path.Combine(RelativeFolderPath, this.Name);
                fullPath += ".asset";
                fullPath = System.IO.Path.Combine(Utility.LogicResPath, fullPath);

                string directorPath = System.IO.Path.GetDirectoryName(fullPath);
                if (!System.IO.Directory.Exists(directorPath))
                {
                    System.IO.Directory.CreateDirectory(directorPath);
                }

                AssetDatabase.DeleteAsset(fullPath);
                ByteAsset.CreateAsset(data, fullPath);
            }
            else
            {
                UnityEngine.Debug.LogError("输出level数据流失败");
            }
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
