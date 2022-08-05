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
        protected List<string> brotherNames = new List<string>();

        protected bool _requirementEditing = true;
        #endregion

        #region Common
        public Effect()
        {
            reorderableRequirementList = new ReorderableList(this._requirements, typeof(Requirement))
            {
                drawElementCallback = OnDrawRequiementElement,
                onAddCallback = (list) => OnAddRequiement(),
                onRemoveCallback = (list) => OnRemoveRequiement(),
                drawHeaderCallback = OnDrawRequiementHead,
            };
        }

        protected List<string> CalculateBrotherNames()
        {
            brotherNames.Clear();

            if (null != this._parent)
            {
                List<NodeBase> children = _parent.Children;
                foreach (NodeBase brother in children)
                {
                    if (brother != this)
                    {
                        brotherNames.Add(brother.Name);
                    }
                }
            }

            return brotherNames;
        }

        protected int GetBrotherIndex(string name)
        {
            int index = -1;

            for (int i = 0; i < brotherNames.Count; ++i)
            {
                string n = brotherNames[i];
                if (n == name)
                {
                    index = i; break;
                }
            }

            return index;
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
            this.CalculateBrotherNames();

            GUILayout.BeginHorizontal();

            _requirementEditing = EditorGUILayout.Toggle("", _requirementEditing, EditorStyles.foldout, GUILayout.Width(12));
            if (_requirementEditing)
            {
                reorderableRequirementList.DoLayoutList();
            }
            else
            {
                EditorGUILayout.LabelField("Requirements");
            }

            GUILayout.EndHorizontal();

            reorderableRequirementList.displayAdd = CheckDisplayAdd();
        }

        protected virtual void OnGUITitleRequirementsInfo()
        {
            StringBuilder title = new StringBuilder();

            title.Append("<color=#50AFCCFF>Req:</color>(");

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
            EditorGUILayout.SelectableLabel(title.ToString(), labelStyle, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        }

        protected virtual void OnGUITitlePropertiesInfo()
        {
            StringBuilder title = new StringBuilder();
            title.Append("<color=#50AFCCFF>Pro:</color>(");

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
            EditorGUILayout.SelectableLabel(title.ToString(), labelStyle, GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
        }

        private void OnDrawRequiementHead(Rect rect)
        {
            EditorGUI.LabelField(rect, "Requirements");

            rect.xMin += 100;
            rect.xMax = rect.xMin + 300;

            _requirementLogic = (ECheckLogic)EditorGUI.EnumPopup(rect, this._requirementLogic);
        }

        private void OnDrawRequiementElement(Rect rect, int index, bool selected, bool focused)
        {
            Rect parentRect = rect;
            float width = rect.width;
            if (index < _requirements.Count)
            {
                Rect lineRect = Rect.MinMaxRect(parentRect.xMin + 4, parentRect.yMax - 2, parentRect.xMax - 4, parentRect.yMax);

                Requirement item = _requirements[index];

                int currentSelectedIndex = this.GetBrotherIndex(item.nodeName);

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.xMax = rect.xMin + rect.width / 3 - 2;
                int newSelectedIndex = EditorGUI.Popup(rect, currentSelectedIndex, this.brotherNames.ToArray());
                if (newSelectedIndex != currentSelectedIndex && newSelectedIndex >= 0)
                {
                    item.nodeName = this.brotherNames[newSelectedIndex];
                }

                rect.xMin = rect.xMax + 4;
                rect.width = width / 2 - 2;
                item.isRequirement = EditorGUI.Toggle(rect, "", item.isRequirement);

                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);
            }
            else
            {
                reorderableRequirementList.list.RemoveAt(index);
            }
        }

        private void OnAddRequiement()
        {
            Requirement requirement = new Requirement();
            bool ret = FindFirstUnusedRequirementName(out string name);
            if (ret)
                requirement.nodeName = name;

            _requirements.Add(requirement);
        }

        private void OnRemoveRequiement()
        {
            _requirements.RemoveAt(reorderableRequirementList.index);
        }

        /// <summary>
        /// 检查要求是否已经使用了?
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckIsRequirementUsed(string name)
        {
            for (int i = 0; i < this._requirements.Count; ++i)
            {
                if (_requirements[i].nodeName == name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是否还允许继续添加
        /// </summary>
        private bool CheckDisplayAdd()
        {
            if (null == _parent)
                return false;

            List<NodeBase> children = _parent.Children;
            if (children.Count - 1 > brotherNames.Count)
                return true;

            foreach (NodeBase brother in children)
            {
                if (brother == this)
                    continue;

                if (!CheckIsRequirementUsed(brother.Name))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 寻找第一个仍未被使用的要求名
        /// </summary>
        /// <returns></returns>
        private bool FindFirstUnusedRequirementName(out string name)
        {
            name = null;
            if (null == _parent)
                return false;

            List<NodeBase> children = _parent.Children;
            foreach (NodeBase brother in children)
            {
                if (brother == this)
                    continue;

                if (!CheckIsRequirementUsed(brother.Name))
                {
                    name = brother.Name;
                    return true;
                }
            }

            return false;
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
