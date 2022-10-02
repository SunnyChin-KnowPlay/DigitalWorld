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
        private NodeController nodeController = null;

        public NodeController NodeController
        {
            get
            {
                if (null == nodeController)
                {
                    nodeController = new NodeController();
                }
                return nodeController;
            }
        }

        private ReorderableList actionList;
        private ReorderableList propertyList;
        private ReorderableList eventList;

        #endregion

        #region Common
        private void OnEnable()
        {
            NodeController c = NodeController;
            c.LoadAllItems();

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

            eventList = new ReorderableList(c.GetItems(EItemType.Event), typeof(NodeItem))
            {
                drawElementCallback = OnDrawEventElement,
                onAddCallback = (list) => OnAddItem(EItemType.Event),
                onRemoveCallback = (list) => OnRemoveItem(eventList),
                drawHeaderCallback = OnDrawEventHead,
                draggable = false
            };
        }

        private void OnDisable()
        {
            NodeController c = NodeController;
            c.ClearItems();

            actionList = null;
            propertyList = null;
            eventList = null;
        }

        private ReorderableList GetList(EItemType type)
        {
            return type switch
            {
                EItemType.Action => actionList,
                EItemType.Property => propertyList,
                EItemType.Event => eventList,
                _ => null,
            };
        }

        private void Save()
        {
            NodeController c = NodeController;
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
                GUIStyle labelStyle = new GUIStyle("minibutton")
                {
                    alignment = TextAnchor.MiddleLeft,
                };

                Rect lineRect = Rect.MinMaxRect(parentRect.xMin + 4, parentRect.yMax - 2, parentRect.xMax - 4, parentRect.yMax);

                NodeItem item = actionList.list[index] as NodeItem;

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.xMax = rect.xMin + 60;
                EditorGUI.LabelField(rect, item.Id.ToString(), labelStyle);

                Rect separationRect = Rect.MinMaxRect(rect.xMax + 3, rect.yMin, rect.xMax + 5, rect.yMax);
                EditorGUI.DrawRect(separationRect, Logic.Utility.kSplitLineColor);

                rect.xMin = rect.xMax + 8;
                rect.xMax = rect.xMin + width * 0.3f;
                EditorGUI.LabelField(rect, string.Format("{0}", item.Name), labelStyle);

                separationRect = Rect.MinMaxRect(rect.xMax + 3, rect.yMin, rect.xMax + 5, rect.yMax);
                EditorGUI.DrawRect(separationRect, Logic.Utility.kSplitLineColor);

                rect.xMin = rect.xMax + 8;
                rect.xMax = rect.xMin + width * 0.4f;
                EditorGUI.LabelField(rect, string.Format("{0}", item.Desc), labelStyle);

                rect.xMin = width - 12;
                rect.xMax = width;

                EditorGUI.BeginDisabledGroup(!selected);

                bool ret = GUI.Button(rect, Logic.Utility.GUIContentD__Menu, Logic.Utility.StyleD__MenuIconButton);
                if (ret)
                {
                    OnEditItem(item);
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);

            }
            else
            {
                actionList.list.RemoveAt(index);
            }
        }

        private void OnDrawEventElement(Rect rect, int index, bool selected, bool focused)
        {
            Rect parentRect = rect;

            float width = rect.width;
            if (index < eventList.list.Count)
            {
                GUIStyle labelStyle = new GUIStyle("minibutton")
                {
                    alignment = TextAnchor.MiddleLeft,
                };

                Rect lineRect = Rect.MinMaxRect(parentRect.xMin + 4, parentRect.yMax - 2, parentRect.xMax - 4, parentRect.yMax);

                NodeItem item = eventList.list[index] as NodeItem;

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.xMax = rect.xMin + 60;
                EditorGUI.LabelField(rect, item.Id.ToString(), labelStyle);

                Rect separationRect = Rect.MinMaxRect(rect.xMax + 3, rect.yMin, rect.xMax + 5, rect.yMax);
                EditorGUI.DrawRect(separationRect, Logic.Utility.kSplitLineColor);

                rect.xMin = rect.xMax + 8;
                rect.xMax = rect.xMin + width * 0.3f;
                EditorGUI.LabelField(rect, string.Format("{0}", item.Name), labelStyle);

                separationRect = Rect.MinMaxRect(rect.xMax + 3, rect.yMin, rect.xMax + 5, rect.yMax);
                EditorGUI.DrawRect(separationRect, Logic.Utility.kSplitLineColor);

                rect.xMin = rect.xMax + 8;
                rect.xMax = rect.xMin + width * 0.4f;
                EditorGUI.LabelField(rect, string.Format("{0}", item.Desc), labelStyle);


                rect.xMin = width - 12;
                rect.xMax = width;
                EditorGUI.BeginDisabledGroup(!selected);

                bool ret = GUI.Button(rect, Logic.Utility.GUIContentD__Menu, Logic.Utility.StyleD__MenuIconButton);
                if (ret)
                {
                    OnEditItem(item);
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);

            }
            else
            {
                eventList.list.RemoveAt(index);
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
                rect.xMax = rect.xMin + 60;
                EditorGUI.LabelField(rect, item.Id.ToString());


                rect.xMin = rect.xMax + 4;
                rect.width = width / 2 - 2;
                EditorGUI.LabelField(rect, string.Format("{0} - {1}", item.Name, item.Desc));

                rect.xMin = width - 12;
                rect.xMax = width;
                bool ret = GUI.Button(rect, Logic.Utility.GUIContentD__Menu, Logic.Utility.StyleD__MenuIconButton);
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
            LogicItemCreateWizard.DisplayWizard(type, OnAddItem, NodeController.GetNewId(type));
        }

        private void OnRemoveItem(ReorderableList list)
        {
            list.list.RemoveAt(list.index);
        }

        private void OnDrawActionHead(Rect rect)
        {
            float width = rect.width;


            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,

            };

            rect.xMin += 6;
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.xMax = rect.xMin + 60;
            EditorGUI.LabelField(rect, "Id", labelStyle);

            rect.xMin = rect.xMax + 8;
            rect.xMax = rect.xMin + width * 0.3f;
            EditorGUI.LabelField(rect, "Name", labelStyle);

            rect.xMin = rect.xMax + 8;
            rect.xMax = rect.xMin + width * 0.4f;
            EditorGUI.LabelField(rect, "Description", labelStyle);
        }

        private void OnDrawPropertyHead(Rect rect)
        {
            EditorGUI.LabelField(rect, "Properties");
        }

        private void OnDrawEventHead(Rect rect)
        {
            float width = rect.width;


            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,

            };

            rect.xMin += 6;
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.xMax = rect.xMin + 60;
            EditorGUI.LabelField(rect, "Id", labelStyle);

            rect.xMin = rect.xMax + 8;
            rect.xMax = rect.xMin + width * 0.3f;
            EditorGUI.LabelField(rect, "Name", labelStyle);

            rect.xMin = rect.xMax + 8;
            rect.xMax = rect.xMin + width * 0.4f;
            EditorGUI.LabelField(rect, "Description", labelStyle);
        }

        public void OnGUI()
        {
            NodeController c = NodeController;


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

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();


            EditorGUILayout.BeginVertical();
            EditorGUI.BeginDisabledGroup(!c.IsDirty);

            if (GUILayout.Button("Save"))
            {
                Save();
                c.GenerateNodesCode();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Quit"))
            {
                this.Close();
            }
            EditorGUILayout.EndVertical();


            //EditorGUILayout.BeginVertical();
            //if (GUILayout.Button("Generate Codes"))
            //{
            //    c.GenerateNodesCode();
            //}
            //EditorGUILayout.EndVertical();

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
        }

        private void OnAddItem(EItemType type, NodeItem ba)
        {
            NodeController.AddItem(type, ba);
            Focus();
        }
        #endregion
    }
}
