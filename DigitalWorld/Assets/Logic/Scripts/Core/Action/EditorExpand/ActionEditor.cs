#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Purchasing;
#endif
using UnityEngine;

namespace DigitalWorld.Logic.Actions
{
    public partial class ActionBase
    {
#if UNITY_EDITOR
        #region Params


        #endregion

        #region Common
        public ActionBase()
        {
            this.selectedAction = FindAction(this.LocalTypeName);
        }

        /// <summary>
        /// 当被编辑器创建时的回调
        /// </summary>
        public virtual void OnCreate()
        {

        }

        protected virtual string GetPropertyDesc(string propertyName)
        {
            return string.Empty;
        }
        #endregion

        #region GUI
        public override Color TitleBackgroundColor => new Color32(200, 200, 200, 255);

        protected override void OnGUITitleTypeName()
        {
            GUIStyle style = GetGUITitleTypeNameStyle(this.NodeType);

            if (Enum.GetValues(typeof(EAction)) != null && Enum.GetValues(typeof(EAction)).Length > 0)
            {
                EAction newActionId = FindAction(EditorGUILayout.Popup(FindActionIndex(selectedAction), GetActionDisplayNamesWithoutSelectedDesc(selectedAction), style, GUILayout.Width(240)));

                if (newActionId != selectedAction)
                {
                    if (Utility.CreateNewAction(newActionId) is ActionBase newAction)
                    {
                        int nodeIndex = this.Index;
                        NodeBase parent = this.Parent;
                        this.SetParent(null);
                        newAction.Name = this.Name;
                        newAction.SetParent(parent, nodeIndex);
                    }
                    selectedAction = newActionId;
                }
            }
            else
            {
                EditorGUILayout.LabelField(this.LocalTypeName, style, GUILayout.Width(240));
            }
        }

        protected void OnGUIPropertyExplore()
        {
            GUIStyle style = new GUIStyle("OL Title");
            style.padding.left = 0;

            Color color = GUI.backgroundColor;

            using (EditorGUILayout.HorizontalScope h = new EditorGUILayout.HorizontalScope(style))
            {
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontStyle = FontStyle.Bold,
                };
                labelStyle.normal.textColor = this.GetNodeColor(typeof(Properties.PropertyBase)) * 2f;

                EditorGUILayout.LabelField("Property Explore", labelStyle, GUILayout.Width(160));

                GUILayout.FlexibleSpace();
            }

            GUI.backgroundColor = color;
        }

        protected override void OnGUIChildrenTitle()
        {
            base.OnGUIChildrenTitle();
            if (this._children.Count > 0)
            {
                this.OnGUIPropertyExplore();
            }
        }
        #endregion

        #region Listen



        #endregion
#endif
    }
}
