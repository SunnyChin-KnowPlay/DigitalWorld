#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
#endif
using UnityEngine;

namespace DigitalWorld.Logic
{
    public partial class Effect
    {
#if UNITY_EDITOR

        #region Params

        protected ReorderableList reorderableRequirementList;
        #endregion

        #region Common
        public Effect()
        {
            reorderableRequirementList = new ReorderableList(this._requirements, typeof(Requirement));
            reorderableRequirementList.drawElementCallback = OnDrawElement;
            reorderableRequirementList.onAddCallback = (list) => OnAdd();
            reorderableRequirementList.onRemoveCallback = (list) => OnRemove();
        }
        #endregion

        #region GUI
        public override void OnGUI()
        {
            base.OnGUI();
        }

        protected override void OnGUIEditing()
        {
            base.OnGUIEditing();

            OnGUIEditingRequirementsInfo();


        }

        protected override void OnGUITitleMenus()
        {
            if (GUILayout.Button("Edit"))
            {
                OnClickEdit();
            }

            base.OnGUITitleMenus();
        }

        protected override void OnGUITitleInfo()
        {
            base.OnGUITitleInfo();

            OnGUITitleRequirementsInfo();
            OnGUITitlePropertiesInfo();
        }

        protected virtual void OnGUIEditingRequirementsInfo()
        {
            GUILayout.BeginVertical();
            reorderableRequirementList.DoLayoutList();
            GUILayout.EndVertical();
        }

        protected virtual void OnGUITitleRequirementsInfo()
        {
            StringBuilder title = new StringBuilder();

            title.Append("<color=#50AFCCFF>Requirements:</color>(");

            int index = 0;
            foreach (Requirement requirement in this._requirements)
            {
                title.AppendFormat("<color=#59E323FF>{0}</color> = <color=#F2660BFF>{1}</color>", requirement.nodeName, requirement.isRequirement);

                index++;
                if (index < this._requirements.Count)
                {
                    title.Append(", ");
                }
            }

            title.Append(")");
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                richText = true
            };
            EditorGUILayout.SelectableLabel(title.ToString(), labelStyle, GUILayout.MaxHeight(16));
        }

        protected virtual void OnGUITitlePropertiesInfo()
        {

            StringBuilder title = new StringBuilder();
            title.Append("<color=#50AFCCFF>Properties:</color>(");

            System.Type type = this.GetType();
            // 这里获取到了所有的字段 把他们一个一个的显示出来
            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

            string fieldName = "";
            for (int i = 0; i < fields.Length; ++i)
            {
                FieldInfo field = fields[i];
                if (field.IsFamily)
                    continue;

                fieldName = field.Name;
                title.AppendFormat("<color=#59E323FF>{0}</color> = <color=#F2660BFF>{1}</color>", fieldName, field.GetValue(this)?.ToString());
                if (i < fields.Length - 1)
                {
                    title.Append(", ");
                }
            }

            title.Append(")");
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                richText = true
            };
            EditorGUILayout.SelectableLabel(title.ToString(), labelStyle, GUILayout.MaxHeight(16));
        }

        private void OnDrawElement(Rect rect, int index, bool selected, bool focused)
        {
            float width = rect.width;
            if (index < _requirements.Count)
            {
                Requirement item = _requirements[index];

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.xMax = rect.xMin + rect.width / 2 - 2;
                item.nodeName = EditorGUI.TextField(rect, item.nodeName);

                rect.xMin = rect.xMax + 4;
                rect.width = width / 2 - 2;
                item.isRequirement = EditorGUI.Toggle(rect, item.isRequirement);
            }
            else
            {
                reorderableRequirementList.list.RemoveAt(index);
            }
        }

        private void OnAdd()
        {
            int count = _requirements.Count;
            _requirements.Add(new Requirement { index = count });
            //_containerNamesSP.arraySize += 1;
            //_containerWidgetsSP.arraySize += 1;

            //SerializedProperty objectReferenceValuePro = _containerWidgetsSP.GetArrayElementAtIndex(_containerWidgetsSP.arraySize - 1);
            //objectReferenceValuePro.objectReferenceValue = null;
        }

        private void OnRemove()
        {
            _requirements.RemoveAt(reorderableRequirementList.index);


        }
        #endregion

        #region Listen
        protected virtual void OnClickEdit()
        {
            LogicHelper.ApplyEditNode(this.NodeType, this.Parent, this);
        }
        #endregion
#endif
    }
}
