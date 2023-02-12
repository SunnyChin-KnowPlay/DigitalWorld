using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    public class LogicSettingEditorWindow : EditorWindow
    {
        #region Params
        private readonly List<SettingItem> items;
        private Vector2 scrollViewPos;
        #endregion

        #region Common
        internal LogicSettingEditorWindow()
        {
            items = new List<SettingItem>()
            {
                new SettingItem(typeof(bool), Utility.nodeDefaultEditingKey, "Node Auto Editing(节点自动展开)"),
                new SettingItem(typeof(bool), Utility.runningWindowAutoOpenKey, "Running Auto Open(启动调试时，自动打卡运行关卡窗口)"),
            };

            items.Sort(OnSortItems);
        }

        private int OnSortItems(SettingItem l, SettingItem r)
        {
            return l.name.CompareTo(r.name);
        }
        #endregion

        #region GUI
        private void OnGUI()
        {
            using (EditorGUILayout.ScrollViewScope v = new EditorGUILayout.ScrollViewScope(scrollViewPos, "FrameBox"))
            {
                OnGUIInputTextField();

                foreach (SettingItem item in items)
                {
                    OnGUIItem(item);
                }
            }
        }

        private string m_InputSearchText = string.Empty;
        private GUIStyle TextFieldRoundEdge;
        private GUIStyle TextFieldRoundEdgeCancelButton;
        private GUIStyle TextFieldRoundEdgeCancelButtonEmpty;
        private GUIStyle TransparentTextField;

        /// <summary>
        /// 绘制输入框，放在OnGUI函数里
        /// </summary>
        private void OnGUIInputTextField()
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

        private void OnGUIItem(SettingItem item)
        {
            System.Type type = item.itemType;

            using (EditorGUILayout.HorizontalScope h = new EditorGUILayout.HorizontalScope())
            {
                if (type == typeof(string))
                {
                    string value = EditorPrefs.GetString(item.playerPrefKey);
                    string newV = EditorGUILayout.TextField(item.name, value);
                    if (value != newV)
                    {
                        EditorPrefs.SetString(item.playerPrefKey, newV);
                    }
                }
                else if (type == typeof(int))
                {
                    int value = EditorPrefs.GetInt(item.playerPrefKey);
                    int newV = EditorGUILayout.IntField(item.name, value);
                    if (value != newV)
                    {
                        EditorPrefs.SetInt(item.playerPrefKey, newV);
                    }
                }
                else if (type == typeof(bool))
                {
                    bool value = EditorPrefs.GetBool(item.playerPrefKey);

                    EditorGUILayout.LabelField(item.name, GUILayout.MinWidth(400));
                    GUILayout.FlexibleSpace();

                    bool newV = EditorGUILayout.Toggle("", value, GUILayout.Width(12));
                    if (value != newV)
                    {
                        EditorPrefs.SetBool(item.playerPrefKey, newV);
                    }
                    GUILayout.Space(6);
                }
                else if (type == typeof(float))
                {
                    float value = EditorPrefs.GetFloat(item.playerPrefKey);
                    float newV = EditorGUILayout.FloatField(item.name, value);
                    if (value != newV)
                    {
                        EditorPrefs.SetFloat(item.playerPrefKey, newV);
                    }
                }

                Rect lineRect = Rect.MinMaxRect(h.rect.xMin + 4, h.rect.yMax - 2, h.rect.xMax - 4, h.rect.yMax);
                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);
            }
        }
        #endregion
    }
}
