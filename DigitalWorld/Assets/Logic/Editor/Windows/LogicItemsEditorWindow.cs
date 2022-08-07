using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
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

        private ReorderableList actionList;
        private ReorderableList conditionList;
        #endregion

        #region Common
        private void StartEdit()
        {
            NodeController c = C;

            if (!c.Editing)
                filter = string.Empty;
            c.StartEdit();

            actionList = new ReorderableList(c.GetItems(EItemType.Action), typeof(NodeItem))
            {
                drawElementCallback = OnDrawActionElement,
                onAddCallback = (list) => OnAddItem(EItemType.Action),
                onRemoveCallback = (list) => OnRemoveItem(actionList),
                drawHeaderCallback = OnDrawActionHead,
                draggable = false
            };

            conditionList = new ReorderableList(c.GetItems(EItemType.Condition), typeof(NodeItem))
            {
                drawElementCallback = OnDrawConditionElement,
                onAddCallback = (list) => OnAddItem(EItemType.Condition),
                onRemoveCallback = (list) => OnRemoveItem(conditionList),
                drawHeaderCallback = OnDrawConditionHead,
                draggable = false
            };
        }

        private ReorderableList GetList(EItemType type)
        {
            return type switch
            {
                EItemType.Action => actionList,
                EItemType.Condition => conditionList,
                _ => null,
            };
        }

        private void QuitEdit()
        {
            NodeController c = C;

            c.StopEdit();
        }

        private void SaveAndQuitEdit()
        {
            NodeController c = C;

            c.Save();
        }
        #endregion


        #region GUI
        private void OnDrawActionElement(Rect rect, int index, bool selected, bool focused)
        {
            Rect parentRect = rect;

            float width = rect.width;
            if (index < actionList.list.Count)
            {
                Rect lineRect = Rect.MinMaxRect(parentRect.xMin + 4, parentRect.yMax - 2, parentRect.xMax - 4, parentRect.yMax);

                NodeItem item = actionList.list[index] as NodeItem;

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.xMax = rect.xMin + 12;
                EditorGUI.LabelField(rect, item.Id.ToString());


                rect.xMin = rect.xMax + 4;
                rect.width = width / 2 - 2;
                EditorGUI.LabelField(rect, string.Format("{0} - {1}", item.Name, item.Desc));

                rect.xMin = width - 40;
                rect.xMax = width;
                bool ret = GUI.Button(rect, new GUIContent("Edit"));
                if (ret)
                {
                    OnEditItem(EItemType.Action, item);
                }


                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);

            }
            else
            {
                actionList.list.RemoveAt(index);
            }
        }

        private void OnDrawConditionElement(Rect rect, int index, bool selected, bool focused)
        {
            float width = rect.width;
            if (index < conditionList.list.Count)
            {
                NodeItem item = conditionList.list[index] as NodeItem;

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.xMax = rect.xMin + 12;
                EditorGUI.LabelField(rect, item.Id.ToString());


                rect.xMin = rect.xMax + 4;
                rect.width = width / 2 - 2;
                EditorGUI.LabelField(rect, string.Format("{0} - {1}", item.Name, item.Desc));

                rect.xMin = width - 40;
                rect.xMax = width;
                bool ret = GUI.Button(rect, new GUIContent("Edit"));
                if (ret)
                {
                    OnEditItem(EItemType.Condition, item);
                }
            }
            else
            {
                conditionList.list.RemoveAt(index);
            }
        }

        private void OnAddItem(EItemType type)
        {
            EditorWindow.GetWindow<LevelItemEditorWindow>("Create " + NodeController.GetTitleWithType(type)).Show(type, OnAddItem);
        }

        private void OnRemoveItem(ReorderableList list)
        {
            list.list.RemoveAt(list.index);
        }

        private void OnDrawActionHead(Rect rect)
        {
            EditorGUI.LabelField(rect, "Actions");


        }

        private void OnDrawConditionHead(Rect rect)
        {
            EditorGUI.LabelField(rect, "Conditions");


        }

        public void OnGUI()
        {
            NodeController c = C;

            bool editing = c.Editing;
            EditorGUILayout.BeginHorizontal();

            if (editing)
            {
                if (c.IsDirty)
                {
                    if (GUILayout.Button("Save & Quit"))
                    {
                        SaveAndQuitEdit();
                    }
                }

                if (GUILayout.Button("Quit"))
                {
                    QuitEdit();
                }
            }
            else
            {
                if (GUILayout.Button("Start Edit"))
                {
                    StartEdit();

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
                GUIStyle style = new GUIStyle("IN Title");
                style.padding.left = 0;

                EditorGUILayout.BeginHorizontal(style);

                bool v = c.GetItemsEditing(type);
                v = EditorGUILayout.Foldout(v, NodeController.GetTitleWithType(type));
                c.SetItemsEditing(type, v);

                EditorGUILayout.EndHorizontal();

                if (v)
                {
                    ReorderableList list = GetList(type);
                    if (null != list)
                    {
                        list.DoLayoutList();
                    }

                    //reorderableRequirementList.DoLayoutList();

                    //var dict = c.GetItems(type);
                    //if (null != dict)
                    //{
                    //    List<NodeItem> list = new List<NodeItem>(dict.Count);
                    //    list.AddRange(dict);

                    //    for (int i = 0; i < list.Count; ++i)
                    //    {
                    //        if (!string.IsNullOrEmpty(filter) && !list[i].Name.ToLower().Contains(filter))
                    //            continue;

                    //        GUIStyle style = new GUIStyle(GUI.skin.box);
                    //        EditorGUILayout.BeginHorizontal(style);
                    //        list[i].OnGUITitle();


                    //        if (GUILayout.Button("编辑", GUILayout.MaxWidth(60)))
                    //        {
                    //            EditorWindow.GetWindow<LevelItemEditorWindow>("编辑 " + NodeController.GetTitleWithType(type)).Show(type, list[i], RemoveItem, AddItem);
                    //        }

                    //        if (GUILayout.Button("删除", GUILayout.MaxWidth(60)))
                    //        {
                    //            NodeController.instance.SetDirty();

                    //            for (int j = 0; j < dict.Count; ++j)
                    //            {
                    //                if (dict[j].Id == list[i].Id)
                    //                {
                    //                    dict.RemoveAt(j);
                    //                    break;
                    //                }
                    //            }

                    //            EditorGUILayout.EndHorizontal();
                    //            break;
                    //        }

                    //        EditorGUILayout.EndHorizontal();

                    //    }
                    //}
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
        }

        private void OnEditItem(EItemType type, NodeItem item)
        {
            EditorWindow.GetWindow<LevelItemEditorWindow>("Edit " + NodeController.GetTitleWithType(type)).Show(type, item, OnRemoveItem, OnAddItem);
        }

        private void OnRemoveItem(EItemType type, NodeItem n)
        {
            C.RemoveItem(type, n);
        }

        private void OnAddItem(EItemType type, NodeItem ba)
        {
            C.AddItem(type, ba);
        }
        #endregion
    }
}
