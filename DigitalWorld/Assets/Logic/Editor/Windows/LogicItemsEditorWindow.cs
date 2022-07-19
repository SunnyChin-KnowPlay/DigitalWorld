using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    public class LogicItemsEditorWindow : EditorWindow
    {
        #region Params
        private Vector2 scrollPos = Vector2.zero;

        private string filter = string.Empty;

        private NodeController C
        {
            get
            {
                return NodeController.instance;
            }
        }
        #endregion

        #region GUI
        public void OnGUI()
        {
            NodeController c = C;

            bool editing = c.Editing;
            EditorGUILayout.BeginHorizontal();

            if (editing)
            {
                if (c.IsDirty)
                {
                    if (GUILayout.Button("保存并退出编辑"))
                    {
                        c.Save();
                    }

                    if (GUILayout.Button("退出编辑"))
                    {
                        c.StopEdit();
                    }
                }
                else
                {
                    if (GUILayout.Button("退出编辑"))
                    {
                        c.StopEdit();
                    }
                }
            }
            else
            {
                if (GUILayout.Button("开始编辑"))
                {
                    if (!c.Editing)
                        filter = string.Empty;
                    c.StartEdit();
                }
            }
            EditorGUILayout.EndHorizontal();

            bool isDisable = !NodeController.instance.Editing;
            EditorGUI.BeginDisabledGroup(isDisable);

            var TextFieldRoundEdge = new GUIStyle("SearchTextField");
            filter = EditorGUILayout.TextField(filter, TextFieldRoundEdge);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (EItemType type in Enum.GetValues(typeof(EItemType)))
            {
                EditorGUILayout.BeginHorizontal();

                bool v = c.GetItemsEditing(type);
                v = EditorGUILayout.Foldout(v, NodeController.GetTitleWithType(type));
                c.SetItemsEditing(type, v);

                if (GUILayout.Button("新增", GUILayout.MaxWidth(60)))
                {
                    EditorWindow.GetWindow<LevelItemEditorWindow>("新增 " + NodeController.GetTitleWithType(type)).Show(type, AddItem);
                }

                EditorGUILayout.EndHorizontal();

                if (v)
                {
                    var dict = c.GetItems(type);
                    if (null != dict)
                    {
                        List<NodeItem> list = new List<NodeItem>(dict.Count);

                        foreach (var kvp in dict)
                        {
                            list.Add(kvp.Value);
                        }
                        for (int i = 0; i < list.Count; ++i)
                        {
                            if (!string.IsNullOrEmpty(filter) && !list[i].Name.ToLower().Contains(filter))
                                continue;

                            GUIStyle style = new GUIStyle(GUI.skin.box);
                            EditorGUILayout.BeginHorizontal(style);
                            list[i].OnGUITitle();


                            if (GUILayout.Button("编辑", GUILayout.MaxWidth(60)))
                            {
                                EditorWindow.GetWindow<LevelItemEditorWindow>("编辑 " + NodeController.GetTitleWithType(type)).Show(type, list[i], RemoveItem, AddItem);
                            }

                            if (GUILayout.Button("删除", GUILayout.MaxWidth(60)))
                            {
                                NodeController.instance.SetDirty();
                                dict.Remove(list[i].Id);
                                EditorGUILayout.EndHorizontal();
                                break;
                            }

                            EditorGUILayout.EndHorizontal();

                        }
                    }
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
        }

        private void RemoveItem(EItemType type, NodeItem n)
        {
            C.RemoveItem(type, n);
        }

        private void AddItem(EItemType type, NodeItem ba)
        {
            C.AddItem(type, ba);
        }
        #endregion
    }
}
