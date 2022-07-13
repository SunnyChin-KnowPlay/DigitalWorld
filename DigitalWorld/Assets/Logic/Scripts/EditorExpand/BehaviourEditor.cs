#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace DigitalWorld.Logic
{
    public partial class Behaviour
    {
        #region Params
        
        #endregion

#if UNITY_EDITOR
        #region GUI
        public override void OnGUI()
        {
            //base.OnGUI();

            //GUIStyle style = new GUIStyle("Tooltip");

            //EditorGUILayout.BeginVertical(style);

            //EditorGUILayout.BeginHorizontal();

            //isEditing = EditorGUILayout.Toggle("Behaviour", isEditing);


            //this.OnGUIIndex();
            //this.OnGUITitleMenu();

            //this.OnGUIUid();
            //this.OnGUIRefense();
            //this.OnGUISwitchMenus();
            //this.OnGUIStatusTitle();

            //GUILayout.FlexibleSpace();

            //EditorGUILayout.EndHorizontal();

            //if (isEditing)
            //{
            //    string srcDesc = this.desc;
            //    this.desc = EditorGUILayout.TextArea(this.desc, GUILayout.MinHeight(20));
            //    if (srcDesc != desc)
            //    {
            //        this.SetDirty();
            //    }

             
            //    OnGUIEvent();

            //    EditorGUILayout.BeginVertical(style);
            //    OnGUIEnterActions();
            //    EditorGUILayout.EndVertical();

            //    EditorGUILayout.BeginVertical(style);
            //    OnGUIConditions();
            //    EditorGUILayout.EndVertical();

            //    EditorGUILayout.BeginVertical(style);
            //    OnGUIActions();
            //    EditorGUILayout.EndVertical();

            //    EditorGUILayout.BeginVertical(style);
            //    OnGUIFailedActions();
            //    EditorGUILayout.EndVertical();

            //    EditorGUILayout.BeginVertical(style);
            //    OnGUIExitActions();
            //    EditorGUILayout.EndVertical();
            //}

            //EditorGUILayout.EndVertical();
        }
        #endregion
#endif
    }
}
