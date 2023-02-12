#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Properties
{
    public partial class PropertyBase
    {
        #region Params
        public override bool IsCanDelete => false;
        #endregion

        #region GUI
        protected override void OnGUIName()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.textField)
            {
                //fontStyle = FontStyle.Bold
                margin = new RectOffset(3, 3, 0, 5),
            };

            EditorGUILayout.LabelField(_name, labelStyle, GUILayout.MaxWidth(160));
        }

        protected override void OnGUIIndex()
        {
            EditorGUI.BeginDisabledGroup(true);
            base.OnGUIIndex();
            EditorGUI.EndDisabledGroup();
        }

        protected override void OnClickMenu()
        {
            bool isDisable;

            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Copy"), false, (object userData) =>
            {
                GUIUtility.systemCopyBuffer = this.Copy();
            }, null);
            isDisable = string.IsNullOrEmpty(GUIUtility.systemCopyBuffer);
            if (isDisable)
            {
                menu.AddDisabledItem(new GUIContent("Paste"));
            }
            else
            {
                menu.AddItem(new GUIContent("Paste"), false, (object userData) =>
                {
                    this.Paste(GUIUtility.systemCopyBuffer);
                    this.SetDirty();
                }, null);
            }

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Open Children"), false, (object userData) =>
            {
                this.OpenChildren();
            }, null);

            menu.AddItem(new GUIContent("Close Children"), false, (object userData) =>
            {
                this.CloseChildren();
            }, null);

            menu.ShowAsContext();
        }

        public abstract string GetParamsPreviewString();

        /// <summary>
        /// 是否可以直接设置值
        /// </summary>
        protected virtual bool IsCanForceSetValue { get => false; }
        #endregion
    }
}
#endif