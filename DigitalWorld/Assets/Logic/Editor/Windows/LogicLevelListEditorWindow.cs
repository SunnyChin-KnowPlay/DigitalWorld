using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DigitalWorld.Logic.Editor
{
    internal class LogicLevelListEditorWindow : EditorWindow
    {
        private enum FilterModeEnum
        {
            Index = 0,
            Keyword = 1,
        }

        private class LevelInfo
        {
            /// <summary>
            /// 关卡名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 关卡路径
            /// </summary>
            public string Path { get; set; }
            /// <summary>
            /// 描述信息
            /// </summary>
            public string Description { get; set; }
        }

        #region Params
        private Vector2 scrollViewPos = Vector2.zero;
        private string m_InputSearchText = string.Empty;
        private GUIStyle TextFieldRoundEdge;
        private GUIStyle TextFieldRoundEdgeCancelButton;
        private GUIStyle TextFieldRoundEdgeCancelButtonEmpty;
        private GUIStyle TransparentTextField;

        private FilterModeEnum filterMode = FilterModeEnum.Index;

        private readonly List<LevelInfo> levelInfos = new List<LevelInfo>();
        public ReorderableList LevelInfoList
        {
            get
            {
                if (null == levelInfoList)
                {
                    levelInfoList = new ReorderableList(levelInfos, typeof(LevelInfo))
                    {
                        drawHeaderCallback = OnDrawLevelHeader,
                        drawElementCallback = OnDrawLevelElement,
                        onAddCallback = OnAddLevel,
                        onRemoveCallback = OnRemoveLevel,
                        displayRemove = false,
                        draggable = false
                    };

                    this.Refresh();
                }

                return levelInfoList;
            }
        }
        private ReorderableList levelInfoList;
        #endregion

        #region Common
        public new void Show()
        {
            base.Show();

            this.Refresh();
        }

        private void Refresh()
        {
            string folderPath = Utility.LevelPath;
            string[] files = Directory.GetFiles(folderPath);

            this.levelInfos.Clear();

            for (int i = 0; i < files.Length; ++i)
            {
                string fileFullPath = files[i];

                string extName = Path.GetExtension(fileFullPath);
                if (extName != ".meta")
                {
                    LevelInfo levelInfo = new LevelInfo
                    {
                        Name = Path.GetFileNameWithoutExtension(fileFullPath),
                        Path = fileFullPath,
                        Description = ""
                    };
                    this.levelInfos.Add(levelInfo);
                }
            }
        }

        private void OnProjectChange()
        {
            this.Refresh();
        }
        #endregion

        #region GUI
        private void OnAddLevel(ReorderableList list)
        {
            LogicLevelCreateWizard.CreateLevel(Logic.Utility.LevelPath);
        }

        private void OnRemoveLevel(ReorderableList list)
        {

        }

        /// <summary>
        /// 绘制输入框，放在OnGUI函数里
        /// </summary>
        private void DrawInputTextField()
        {
            if (TextFieldRoundEdge == null)
            {
                TextFieldRoundEdge = new GUIStyle("SearchTextField");
                TextFieldRoundEdgeCancelButton = new GUIStyle("SearchCancelButton");
                TextFieldRoundEdgeCancelButtonEmpty = new GUIStyle("SearchCancelButtonEmpty");
                TransparentTextField = new GUIStyle(EditorStyles.whiteLabel);
                TransparentTextField.normal.textColor = EditorStyles.textField.normal.textColor;
            }

            //获取当前输入框的Rect(位置大小)
            Rect position = EditorGUILayout.GetControlRect();
            //设置圆角style的GUIStyle
            GUIStyle textFieldRoundEdge = TextFieldRoundEdge;
            //设置输入框的GUIStyle为透明，所以看到的“输入框”是TextFieldRoundEdge的风格
            GUIStyle transparentTextField = TransparentTextField;
            //选择取消按钮(x)的GUIStyle
            GUIStyle gUIStyle = (m_InputSearchText != "") ? TextFieldRoundEdgeCancelButton : TextFieldRoundEdgeCancelButtonEmpty;

            //输入框的水平位置向左移动取消按钮宽度的距离
            position.width -= gUIStyle.fixedWidth;
            //如果面板重绘
            if (Event.current.type == EventType.Repaint)
            {
                //根据是否是专业版来选取颜色
                GUI.contentColor = (EditorGUIUtility.isProSkin ? Color.black : new Color(0f, 0f, 0f, 0.5f));
                //当没有输入的时候提示“请输入”
                if (string.IsNullOrEmpty(m_InputSearchText))
                {
                    textFieldRoundEdge.Draw(position, new GUIContent("请输入"), 0);
                }
                else
                {
                    textFieldRoundEdge.Draw(position, new GUIContent(""), 0);
                }
                //因为是“全局变量”，用完要重置回来
                GUI.contentColor = Color.white;
            }
            Rect rect = position;
            //为了空出左边那个放大镜的位置
            float num = textFieldRoundEdge.CalcSize(new GUIContent("")).x - 2f;
            rect.width -= num;
            rect.x += num;
            rect.y += 1f;//为了和后面的style对其

            m_InputSearchText = EditorGUI.TextField(rect, m_InputSearchText, transparentTextField);
            //绘制取消按钮，位置要在输入框右边
            position.x += position.width;
            position.width = gUIStyle.fixedWidth;
            position.height = gUIStyle.fixedHeight;
            if (GUI.Button(position, GUIContent.none, gUIStyle) && m_InputSearchText != "")
            {
                m_InputSearchText = "";
                //用户是否做了输入
                GUI.changed = true;
                //把焦点移开输入框
                GUIUtility.keyboardControl = 0;
            }
        }

        private void OnDrawLevelHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Levels");
        }

        private void OnDrawLevelElement(Rect rect, int index, bool selected, bool focused)
        {
            Rect parentRect = rect;

            float width = rect.width;
            if (index < levelInfos.Count)
            {
                GUIStyle labelStyle = new GUIStyle("minibutton")
                {
                    alignment = TextAnchor.MiddleLeft,
                };

                rect.height = EditorGUIUtility.singleLineHeight;

                LevelInfo item = levelInfos[index];

                rect.xMin = rect.xMax = 0;

                rect.xMax = rect.xMin + width * 0.3f;
                EditorGUI.LabelField(rect, string.Format("{0}", item.Name), labelStyle);

                Rect separationRect = Rect.MinMaxRect(rect.xMax + 3, rect.yMin, rect.xMax + 5, rect.yMax);
                EditorGUI.DrawRect(separationRect, Logic.Utility.kSplitLineColor);

                rect.xMin = rect.xMax + 8;
                rect.xMax = parentRect.xMax;
                EditorGUI.LabelField(rect, string.Format("{0}", item.Description), labelStyle);

                rect.xMin = width - 12;
                rect.xMax = width;

                Rect lineRect = Rect.MinMaxRect(parentRect.xMin + 4, parentRect.yMax - 2, parentRect.xMax - 4, parentRect.yMax);
                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);

                if (GUI.Button(parentRect, GUIContent.none, EditorStyles.whiteLabel))
                {
                    this.OnClickLevel(item);
                }

            }
            else
            {
                levelInfos.RemoveAt(index);
            }
        }

        private void OnGUI()
        {


            GUIStyle style = new GUIStyle(GUI.skin.button);

            using (EditorGUILayout.HorizontalScope h = new EditorGUILayout.HorizontalScope())
            {
                this.filterMode = (FilterModeEnum)EditorGUILayout.EnumPopup("检索模式", this.filterMode, GUILayout.MaxWidth(300));
                DrawInputTextField();
            }

            scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);
            LevelInfoList.DoLayoutList();
            EditorGUILayout.EndScrollView();


            GUIStyle style2 = new GUIStyle(GUI.skin.box);

            using (EditorGUILayout.HorizontalScope h = new EditorGUILayout.HorizontalScope(style2))
            {
                if (GUILayout.Button("刷新"))
                {
                    Refresh();
                }
            }
        }

        private void OnClickLevel(LevelInfo info)
        {
            string relativePath = info.Path.Substring(Utility.ConfigsPath.Length + 1);
            if (string.IsNullOrEmpty(relativePath))
                return;

            bool ret = LogicLevelEditorWindow.CheckHasEditing(relativePath, out LogicLevelEditorWindow window);
            if (ret)
            {
                window.Focus();
            }
            else
            {
                TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(info.Path);
                using (StringReader reader = new StringReader(ta.text))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Level));
                    Level level = serializer.Deserialize(reader) as Level;
                    level.RelativeFolderPath = System.IO.Path.GetDirectoryName(relativePath);

                    window = LogicTriggerEditorWindow.CreateWindow<LogicLevelEditorWindow>(typeof(LogicLevelEditorWindow), null);
                    window.Show(level);
                }
            }
        }
        #endregion
    }
}
