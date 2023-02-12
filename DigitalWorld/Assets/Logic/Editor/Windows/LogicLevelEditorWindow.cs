using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal class LogicLevelEditorWindow : EditorWindow
    {
        #region Params
        protected Vector2 scrollViewPos = Vector2.zero;
        protected Rect bottomViewRect = Rect.zero;

        /// <summary>
        /// 当前正在编辑的触发器
        /// </summary>
        protected Level currentLevel = null;
        protected virtual Level CurrentLevel
        {
            get => currentLevel;
        }

        /// <summary>
        /// 所有正在编辑的关卡
        /// </summary>
        protected readonly static Dictionary<string, LogicLevelEditorWindow> editingLevels = new Dictionary<string, LogicLevelEditorWindow>();
        #endregion

        #region Common
        /// <summary>
        /// 检查是否有已打开正在编辑的窗口 如果有的话 直接还出去
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        internal static bool CheckHasEditing(string relativePath, out LogicLevelEditorWindow window)
        {
            relativePath = relativePath.Replace("\\", "/");
            if (editingLevels.ContainsKey(relativePath))
            {
                window = editingLevels[relativePath];
                return true;
            }

            window = null;
            return false;
        }

        protected virtual void OnDestroy()
        {
            this.SetLevel(null);
        }
        #endregion

        #region GUI
        protected virtual void SetTitle(string title)
        {
            this.titleContent = new GUIContent(title);
        }

        public virtual void Show(Level level)
        {
            base.Show();
            this.autoRepaintOnSceneChange = true;

            this.SetLevel(level);
        }

        protected virtual void SetLevel(Level level)
        {
            if (null != this.currentLevel)
            {
                editingLevels.Remove(currentLevel.RelativeAssetFilePath);
                currentLevel.OnGlobalSelectChanged -= OnGlobalSelectChanged;
                currentLevel = null;
                this.SetTitle("未定义关卡");
            }

            if (null != level)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(level.RelativeAssetFilePath);
                this.SetTitle(fileName);
                this.currentLevel = level;
                currentLevel.OnGlobalSelectChanged += OnGlobalSelectChanged;
                editingLevels.Add(level.RelativeAssetFilePath, this);
            }
        }

        protected virtual void OnGUI()
        {
            if (null == this.currentLevel || !editingLevels.ContainsKey(this.currentLevel.RelativeAssetFilePath))
            {
                this.Close();
            }

            if (null != CurrentLevel)
            {
                scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);
                this.CurrentLevel.OnGUI();
                EditorGUILayout.EndScrollView();

                OnGUIBottomMenus();
                OnGUIEvent();
            }
        }

        protected virtual void OnGUIEvent()
        {
            Event ev = Event.current;
            if (null != ev)
            {
                bool CtrlDown = (ev.modifiers & EventModifiers.Control) != 0;
                bool ShiftDown = (ev.modifiers & EventModifiers.Shift) != 0;

                if (ev.type == EventType.KeyUp)
                {
                    if (CtrlDown)
                    {
                        switch (ev.keyCode)
                        {
                            case KeyCode.X:
                            {
                                // 这里是剪切的逻辑
                                if (null != currentLevel.LastedSelectedNode)
                                {
                                    Logic.NodeBase node = currentLevel.LastedSelectedNode;
                                    GUIUtility.systemCopyBuffer = node.Copy();
                                    node.SetParent(null);
                                    node.Recycle();

                                    currentLevel.ClearSelected();
                                }
                                break;
                            }
                            case KeyCode.C:
                            {
                                // 这里是拷贝的逻辑

                                if (null != currentLevel.LastedSelectedNode)
                                {
                                    if (null != Logic.NodeBase.LastedSelectedFieldInfo)
                                    {
                                        // 拷贝的是字段
                                        GUIUtility.systemCopyBuffer = Convert.ToString(Logic.NodeBase.LastedSelectedFieldInfo.GetValue(currentLevel.LastedSelectedNode));
                                    }
                                    else
                                    {
                                        Logic.NodeBase node = currentLevel.LastedSelectedNode;
                                        GUIUtility.systemCopyBuffer = node.Copy();
                                    }
                                }
                                break;
                            }
                            case KeyCode.V:
                            {
                                // 这里是粘贴的逻辑
                                if (null != currentLevel.LastedSelectedNode && !string.IsNullOrEmpty(GUIUtility.systemCopyBuffer))
                                {
                                    if (null != Logic.NodeBase.LastedSelectedFieldInfo)
                                    {
                                        try
                                        {
                                            Logic.NodeBase.LastedSelectedFieldInfo.SetValue(currentLevel.LastedSelectedNode, Convert.ChangeType(GUIUtility.systemCopyBuffer, Logic.NodeBase.LastedSelectedFieldInfo.FieldType));
                                        }
                                        catch
                                        {

                                        }
                                        finally
                                        {
                                            currentLevel.SetDirty();
                                        }
                                    }
                                    else
                                    {
                                        currentLevel.LastedSelectedNode.Paste(GUIUtility.systemCopyBuffer);
                                    }
                                }

                                this.Repaint();
                                break;
                            }
                            case KeyCode.D:
                            {
                                // 直接复制的逻辑 Duplicate
                                if (null != currentLevel.LastedSelectedNode)
                                {
                                    Logic.NodeBase selectedNode = currentLevel.LastedSelectedNode;
                                    if (selectedNode.Clone() is Logic.NodeBase node)
                                    {
                                        node.Name = string.Empty;
                                        if (null != selectedNode.Parent)
                                        {
                                            node.SetParent(selectedNode.Parent);
                                        }
                                        this.Repaint();
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else if (ShiftDown)
                    {
                        if (ev.keyCode == KeyCode.Delete)
                        {
                            if (null != currentLevel.LastedSelectedNode)
                            {
                                Logic.NodeBase node = currentLevel.LastedSelectedNode;
                                if (node.IsCanDelete)
                                {
                                    node.SetParent(null);
                                    node.Recycle();

                                    this.Repaint();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ev.keyCode == KeyCode.Escape)
                        {
                            currentLevel.ClearSelected();
                            this.Repaint();
                        }
                    }
                }
                else if (ev.type == EventType.KeyDown)
                {
                    switch (ev.keyCode)
                    {
                        case KeyCode.UpArrow:
                        {
                            if (null != currentLevel)
                            {
                                currentLevel.AutoSelect(-1);
                                this.Repaint();
                            }
                            break;
                        }
                        case KeyCode.DownArrow:
                        {
                            if (null != currentLevel)
                            {
                                currentLevel.AutoSelect(1);
                                this.Repaint();
                            }
                            break;
                        }
                        case KeyCode.LeftArrow:
                        {
                            if (null != currentLevel.LastedSelectedNode)
                            {
                                currentLevel.LastedSelectedNode.IsEditing = false;
                                this.Repaint();
                            }
                            break;
                        }
                        case KeyCode.RightArrow:
                        {
                            if (null != currentLevel.LastedSelectedNode)
                            {
                                currentLevel.LastedSelectedNode.IsEditing = true;
                                this.Repaint();
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// GUI底部菜单
        /// </summary>
        protected virtual void OnGUIBottomMenus()
        {
            GUIStyle style = new GUIStyle("Tooltip");

            using (EditorGUILayout.HorizontalScope h = new EditorGUILayout.HorizontalScope(style))
            {
                if (GUILayout.Button("Save"))
                {
                    this.CurrentLevel.Save();
                }

                if (GUILayout.Button("Close"))
                {
                    this.Close();
                }

                bottomViewRect = h.rect;
            }
        }
        #endregion

        #region Listen
        /// <summary>
        /// 当全局选择节点改变了
        /// 这里需要处理一下scrollViewPos 如果通过计算 发现当前的rect超过了Pos的范围 则把Pos拉下去或者拉上来
        /// </summary>
        /// <param name="node"></param>
        /// <param name="lastedSelected"></param>
        protected virtual void OnGlobalSelectChanged(Logic.NodeBase root, int lastedSelected)
        {
            Logic.NodeBase selectedNode = root.LastedSelectedNode;
            if (null != selectedNode)
            {
                Rect rect = selectedNode.GetGUILayoutRect();

                Vector2 selectedNodePos = rect.position;
                Vector2 scrollViewSize = new Vector2(position.size.x, position.size.y - bottomViewRect.height); // 这里扣掉底部的菜单高度

                float nodeHeight = EditorGUIUtility.singleLineHeight * 2;

                if (selectedNodePos.y + nodeHeight > (scrollViewPos.y + scrollViewSize.y)) // 说明是向下超过底部了
                {
                    float offset = (selectedNodePos.y + nodeHeight) - scrollViewSize.y - scrollViewPos.y;
                    scrollViewPos.y += offset;
                }

                if (selectedNodePos.y - scrollViewPos.y < 0) // 说明是向上超过顶部了
                {
                    float offset = scrollViewPos.y - selectedNodePos.y;
                    scrollViewPos.y -= offset;
                }
            }
        }
        #endregion
    }
}
