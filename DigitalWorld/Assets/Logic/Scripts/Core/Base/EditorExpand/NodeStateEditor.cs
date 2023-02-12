#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace DigitalWorld.Logic
{

    public partial class NodeState
    {
        #region Params
        protected ReorderableList reorderableRequirementList;

        #endregion

        #region Common
        public NodeState()
        {
            reorderableRequirementList = new ReorderableList(this._requirements, typeof(Requirement))
            {
                drawElementCallback = OnDrawRequiementElement,
                onAddCallback = (list) => OnAddRequiement(),
                onRemoveCallback = (list) => OnRemoveRequiement(),
                drawHeaderCallback = OnDrawRequiementHead,
            };


        }
        #endregion

        #region Pool

        #endregion

        #region Logic
        /// <summary>
        /// 检查要求是否已经使用了?
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckIsRequirementUsed(ELocationMode mode, string name)
        {
            for (int i = 0; i < this._requirements.Count; ++i)
            {
                Requirement req = _requirements[i];

                if (mode == ELocationMode.Name)
                {
                    if (req.locationMode == mode && req.nodeName == name)
                        return true;
                }
                else
                {
                    if (req.locationMode == mode)
                        return true;
                }
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

                if (!CheckIsRequirementUsed(ELocationMode.Name, brother.Name))
                    return true;
            }

            return false;
        }

        private bool FindFirstUnusedRequirement(out Requirement req)
        {
            req = null;
            if (null == _parent)
                return false;

            bool ret = CheckIsRequirementUsed(ELocationMode.Previous, string.Empty);
            if (!ret)
            {
                req = new Requirement()
                {
                    locationMode = ELocationMode.Previous,
                    requirementState = EState.Succeeded,
                };
                return true;
            }

            List<NodeBase> children = _parent.Children;
            foreach (NodeBase brother in children)
            {
                if (brother == this)
                    continue;

                if (!CheckIsRequirementUsed(ELocationMode.Name, brother.Name))
                {
                    req = new Requirement()
                    {
                        locationMode = ELocationMode.Name,
                        nodeName = brother.Name,
                        requirementState = EState.Succeeded,
                    };
                    return true;
                }
            }

            ret = CheckIsRequirementUsed(ELocationMode.Next, string.Empty);
            if (!ret)
            {
                req = new Requirement()
                {
                    locationMode = ELocationMode.Next,
                    requirementState = EState.Succeeded,
                };
                return true;
            }

            return false;
        }
        #endregion

        #region GUI
        protected virtual string GetStateStyleName(EState state)
        {
            string styleName;
            switch (state)
            {
                case EState.Running:
                {
                    styleName = "flow node 3";
                    break;
                }
                case EState.Succeeded:
                {
                    styleName = "flow node 1";
                    break;
                }
                case EState.Failed:
                {
                    styleName = "flow node 6";
                    break;
                }
                case EState.Idle:
                {
                    styleName = "flow node 0";
                    break;
                }
                default:
                {
                    styleName = "Popup";
                    break;
                }
            }

            return styleName;
        }

        protected override void OnGUITitleContent()
        {
            base.OnGUITitleContent();

            string styleName;
            switch (_motionMode)
            {
                case EMotionMode.Repeat:
                {
                    styleName = "flow node 3";
                    break;
                }
                case EMotionMode.Loop:
                {
                    styleName = "flow node 1";
                    break;
                }
                case EMotionMode.Duplicate:
                {
                    styleName = "flow node 5";
                    break;
                }
                default:
                {
                    styleName = "flow node 2";
                    break;
                }
            }

            //if (this.IsSelected)
            //{
            //    styleName += " on";
            //}

            GUIStyle style = new GUIStyle(styleName)
            {
                alignment = TextAnchor.MiddleCenter,
                contentOffset = new Vector2(0, -14),
                fontSize = 12,
            };

            this._motionMode = (EMotionMode)EditorGUILayout.EnumPopup(this._motionMode, style, GUILayout.Width(120));

            if (EditorApplication.isPlaying)
            {
                using (EditorGUI.DisabledGroupScope d = new EditorGUI.DisabledGroupScope(true))
                {
                    GUIStyle textStyle = new GUIStyle(GUI.skin.textField)
                    {

                    };
                    textStyle.normal.textColor = Color.white * 2;

                    string timeText = string.Format("{0}/{1}ms", this.RunningTime, this.TotalTime);


                    EditorGUILayout.LabelField(new GUIContent(timeText, "运行时间(毫秒)"), textStyle, GUILayout.Width(120));
                    Rect rect = GUILayoutUtility.GetLastRect();

                    float percent = 0;
                    if (this.TotalTime > 0)
                    {
                        percent = Mathf.Clamp(this.RunningTime / (float)this.TotalTime, 0, 1);
                    }

                    EditorGUI.DrawRect(new Rect(rect.x, rect.y, percent * rect.width, rect.height), new Color(0, 200 / 255f, 0, 0.3f));

                    EditorGUILayout.LabelField(new GUIContent(string.Format("{0}", this.RunningCount), "运行次数"), textStyle, GUILayout.Width(120));
                }

                this.OnGUITitleState();
            }
        }

        protected virtual void OnGUITitleState()
        {
            string styleName = GetStateStyleName(this.State);

            //if (this.IsSelected)
            //{
            //    styleName += " on";
            //}

            GUIStyle style = new GUIStyle(styleName)
            {
                alignment = TextAnchor.MiddleCenter,
                contentOffset = new Vector2(0, -14),
                fontSize = 12,
                //fontStyle = FontStyle.Bold,     
            };

            string tooltip = "State - ";
            switch (this.State)
            {
                case EState.Idle:
                {
                    tooltip += "Idle";
                    break;
                }
                case EState.Running:
                {
                    tooltip += "Running";
                    break;
                }
                case EState.Succeeded:
                {
                    tooltip += "Succeeded";
                    break;
                }
                case EState.Failed:
                {
                    tooltip += "Failed";
                    break;
                }
            }

            GUILayout.Button(new GUIContent(this.State.ToString(), tooltip), style, GUILayout.Width(140), GUILayout.Height(EditorGUIUtility.singleLineHeight));


            //EditorGUILayout.LabelField();
        }

        protected override void OnGUIEditing()
        {
            OnGUIEditingRequirementsInfo();

            base.OnGUIEditing();
        }

        protected virtual void OnGUIEditingRequirementsInfo()
        {
            this.CalculateBrotherNames();

            bool isDisplayAdd = CheckDisplayAdd();
            if (isDisplayAdd)
            {
                GUIStyle style = new GUIStyle("FrameBox");
                using (EditorGUILayout.VerticalScope v = new EditorGUILayout.VerticalScope(style))
                {
                    reorderableRequirementList.DoLayoutList();
                    reorderableRequirementList.displayAdd = isDisplayAdd;
                }
            }
        }

        //protected override void OnGUITitleContent()
        //{
        //    base.OnGUITitleContent();


        //}
        #endregion

        #region GUI/Requirement
        private void OnDrawRequiementHead(Rect rect)
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold
            };
            labelStyle.normal.textColor = Color.white;
            EditorGUI.LabelField(rect, "Requirements", labelStyle);

            rect.xMin += 100;
            rect.xMax = rect.xMin + 100;

            GUIStyle style = new GUIStyle("DropDown")
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
            };

            style.normal.textColor = this._requirementLogic == ECheckLogic.And ? new Color(239 / 255f, 86 / 255f, 78 / 255f, 1) : new Color(93 / 255f, 129 / 255f, 255 / 255f, 1);
            style.focused.textColor = this._requirementLogic == ECheckLogic.And ? new Color(239 / 255f, 86 / 255f, 78 / 255f, 1) * 1.5f : new Color(93 / 255f, 129 / 255f, 255 / 255f, 1) * 1.5f;

            _requirementLogic = (ECheckLogic)EditorGUI.EnumPopup(rect, this._requirementLogic, style);
        }

        private void OnDrawRequiementElement(Rect rect, int index, bool selected, bool focused)
        {
            Rect parentRect = rect;
            float width = rect.width;
            if (index < _requirements.Count)
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                Rect lineRect = Rect.MinMaxRect(parentRect.xMin + 4, parentRect.yMax - 2, parentRect.xMax - 4, parentRect.yMax);

                Requirement item = _requirements[index];
                int currentSelectedIndex = this.GetBrotherIndex(item.nodeName);

                rect.xMax = rect.xMin + width * 0.2f;

                item.locationMode = (ELocationMode)EditorGUI.EnumPopup(rect, item.locationMode);

                if (item.locationMode == ELocationMode.Name)
                {
                    rect.xMin = rect.xMax + 4;
                    rect.xMax = rect.xMin + width * 0.4f;
                    int newSelectedIndex = EditorGUI.Popup(rect, currentSelectedIndex, this.brotherNames.ToArray());
                    if (newSelectedIndex != currentSelectedIndex && newSelectedIndex >= 0)
                    {
                        item.nodeName = this.brotherNames[newSelectedIndex];
                    }
                }

                rect.xMin = rect.xMax + 4;
                rect.xMax = width;

                string styleName = GetStateStyleName(item.requirementState);
                GUIStyle stateStyle = new GUIStyle(styleName)
                {
                    alignment = TextAnchor.MiddleCenter,
                    contentOffset = styleName == "Popup" ? Vector2.zero : new Vector2(0, -14),
                    fontSize = 12,
                };
                item.requirementState = (EState)EditorGUI.EnumFlagsField(rect, item.requirementState, stateStyle);

                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);
            }
            else
            {
                reorderableRequirementList.list.RemoveAt(index);
            }
        }

        private void OnAddRequiement()
        {
            bool ret = FindFirstUnusedRequirement(out Requirement requirement);
            if (ret)
            {
                _requirements.Add(requirement);
            }
        }

        private void OnRemoveRequiement()
        {
            _requirements.RemoveAt(reorderableRequirementList.index);
        }
        #endregion
    }

}
#endif