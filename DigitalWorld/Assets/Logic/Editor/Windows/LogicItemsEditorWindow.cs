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
        private ReorderableList propertyList;

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

            propertyList = new ReorderableList(c.GetItems(EItemType.Property), typeof(NodeItem))
            {
                drawElementCallback = OnDrawPropertyElement,
                onAddCallback = (list) => OnAddItem(EItemType.Property),
                onRemoveCallback = (list) => OnRemoveItem(propertyList),
                drawHeaderCallback = OnDrawPropertyHead,
                draggable = false
            };

        }

        private ReorderableList GetList(EItemType type)
        {
            return type switch
            {
                EItemType.Action => actionList,
                EItemType.Property => propertyList,
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
                    OnEditItem(item);
                }

                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);

            }
            else
            {
                actionList.list.RemoveAt(index);
            }
        }

        private void OnDrawPropertyElement(Rect rect, int index, bool selected, bool focused)
        {
            Rect parentRect = rect;

            float width = rect.width;
            if (index < propertyList.list.Count)
            {
                Rect lineRect = Rect.MinMaxRect(parentRect.xMin + 4, parentRect.yMax - 2, parentRect.xMax - 4, parentRect.yMax);

                NodeItem item = propertyList.list[index] as NodeItem;

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
                    OnEditItem(item);
                }

                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);

            }
            else
            {
                actionList.list.RemoveAt(index);
            }
        }

        private void OnAddItem(EItemType type)
        {
            LogicItemCreateWizard.DisplayWizard(type, OnAddItem, NodeController.instance.GetNewId(type));
        }

        private void OnRemoveItem(ReorderableList list)
        {
            list.list.RemoveAt(list.index);
        }

        private void OnDrawActionHead(Rect rect)
        {
            EditorGUI.LabelField(rect, "Actions");
        }

        private void OnDrawPropertyHead(Rect rect)
        {
            EditorGUI.LabelField(rect, "Properties");
        }

        public void OnGUI()
        {
            NodeController c = C;



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
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();

            bool editing = c.Editing;
            EditorGUILayout.BeginHorizontal();

            if (editing)
            {
                EditorGUI.BeginDisabledGroup(!c.IsDirty);

                if (GUILayout.Button("Save & Quit"))
                {
                    SaveAndQuitEdit();
                    NodeController.instance.GenerateNodesCode();
                }
                EditorGUI.EndDisabledGroup();

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

                if (GUILayout.Button("Generate Codes"))
                {
                    NodeController.instance.GenerateNodesCode();
                }
            }


            EditorGUILayout.EndHorizontal();
        }

        private void OnEditItem(NodeItem item)
        {
            bool ret = LogicItemEditorWindow.CheckHasEditing(item.Name, out LogicItemEditorWindow window);
            if (ret)
            {
                window.Focus();
            }
            else
            {
                window = LogicItemEditorWindow.CreateWindow<LogicItemEditorWindow>(typeof(LogicItemEditorWindow), null);
                window.Show(item);
            }

            //EditorWindow window = LogicItemEditorWindow.CreateWindow<LogicItemEditorWindow>(typeof(LogicItemEditorWindow), null);
            //EditorWindow.GetWindow<LogicItemEditorWindow>
            //EditorWindow.GetWindow<LogicItemEditorWindow>("Edit " + NodeController.GetTitleWithType(type)).Show(type, item, OnRemoveItem, OnAddItem);
        }

        private void OnAddItem(EItemType type, NodeItem ba)
        {
            C.AddItem(type, ba);
            Focus();
        }
        #endregion
    }
}
