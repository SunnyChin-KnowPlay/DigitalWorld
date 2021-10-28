using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DigitalWorld.UI.Editor
{
    [CustomEditor(typeof(Container))]
    public class ContainerInspector : UnityEditor.Editor
    {
        private class ReorderableListItem
        {
            public int index;
        }

        private ReorderableList reorderableWidgetList;

        private SerializedProperty containerKeysSP;
        private SerializedProperty containerObjectsSP;

        private List<ReorderableListItem> widgetList;

        private void OnEnable()
        {
            containerKeysSP = serializedObject.FindProperty("widgetKeys");
            containerObjectsSP = serializedObject.FindProperty("widgetObjects");

            widgetList = new List<ReorderableListItem>();

            for (int i = 0; i < containerKeysSP.arraySize; i++)
            {
                widgetList.Add(new ReorderableListItem() { index = i });
            }

            reorderableWidgetList = new ReorderableList(widgetList, typeof(ReorderableListItem));
            reorderableWidgetList.drawElementCallback = DrawElement;
            reorderableWidgetList.drawHeaderCallback = (Rect rect) =>
            {
                GUI.Label(rect, "ContainerWidgets");
            };

            reorderableWidgetList.onAddCallback = (list) => Add();
            reorderableWidgetList.onRemoveCallback = (list) => Remove();
            reorderableWidgetList.onReorderCallback = (list) => OnReorder();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    reorderableWidgetList.DoLayoutList();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

           
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawElement(Rect rect, int index, bool selected, bool focused)
        {
            float width = rect.width;
            if (index < containerKeysSP.arraySize)
            {
                SerializedProperty itemData = containerKeysSP.GetArrayElementAtIndex(index);

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.xMax = rect.xMin + rect.width / 2 - 2;
                EditorGUI.PropertyField(rect, itemData, GUIContent.none);

                SerializedProperty itemData2 = containerObjectsSP.GetArrayElementAtIndex(index);
                rect.xMin = rect.xMax + 4;
                rect.width = width / 2 - 2;
                EditorGUI.PropertyField(rect, itemData2, GUIContent.none);
            }
            else
            {
                reorderableWidgetList.list.RemoveAt(index);
            }
        }

        private void Add()
        {
            int count = widgetList.Count;
            widgetList.Add(new ReorderableListItem { index = count });
            containerKeysSP.arraySize += 1;
            containerObjectsSP.arraySize += 1;

            SerializedProperty objectReferenceValuePro = containerObjectsSP.GetArrayElementAtIndex(containerObjectsSP.arraySize - 1);
            objectReferenceValuePro.objectReferenceValue = null;
        }

        private void Remove()
        {
            widgetList.RemoveAt(reorderableWidgetList.index);
            containerKeysSP.DeleteArrayElementAtIndex(reorderableWidgetList.index);
            SerializedProperty objectReferenceValuePro = containerObjectsSP.GetArrayElementAtIndex(reorderableWidgetList.index);
            objectReferenceValuePro.objectReferenceValue = null;
            containerObjectsSP.DeleteArrayElementAtIndex(reorderableWidgetList.index);
        }

        private void OnReorder()
        {
            int maxDif = 0;
            int oldIndex = -1;
            int newIndex = -1;
            for (int i = 0; i < reorderableWidgetList.list.Count; i++)
            {
                int dif = Mathf.Abs((reorderableWidgetList.list[i] as ReorderableListItem).index - i);
                if (dif > maxDif)
                {
                    maxDif = dif;
                    oldIndex = (reorderableWidgetList.list[i] as ReorderableListItem).index;
                    newIndex = i;
                }
            }
            //Debug.Log("change " + oldIndex + " / " + newIndex);
            containerKeysSP.MoveArrayElement(oldIndex, newIndex);
            containerObjectsSP.MoveArrayElement(oldIndex, newIndex);
            for (int i = 0; i < reorderableWidgetList.list.Count; i++)
            {
                (reorderableWidgetList.list[i] as ReorderableListItem).index = i;
            }
        }
    }
}
