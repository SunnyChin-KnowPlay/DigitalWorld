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
            base.OnGUI();

            GUIStyle style = new GUIStyle("Tooltip");

            EditorGUILayout.BeginVertical(style);

            EditorGUILayout.BeginHorizontal();

            _isEditing = EditorGUILayout.Toggle("Behaviour", _isEditing);


            //this.OnGUIIndex();
            //this.OnGUITitleMenu();

            //this.OnGUIUid();
            //this.OnGUIRefense();
            //this.OnGUISwitchMenus();
            //this.OnGUIStatusTitle();

            EditorGUILayout.EndHorizontal();

            if (_isEditing)
            {
                string srcDesc = this._description;
                this._description = EditorGUILayout.TextArea(this._description, GUILayout.MinHeight(20));
                if (srcDesc != _description)
                {
                    this.SetDirty();
                }

                EditorGUILayout.BeginVertical(style);
                OnGUIEffects();
                EditorGUILayout.EndVertical();

            }

            EditorGUILayout.EndVertical();
        }

        protected virtual void OnGUIEffects()
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                Effect effect = this._children[i] as Effect;
                if (null != effect)
                {
                    effect.OnGUI();
                }
            }
        }
        #endregion

        #region Common
        public virtual void Save()
        {
            //if (!string.IsNullOrEmpty(sPath))
            //{
            //    bool ret = this.CheckCanSave();
            //    if (!ret)
            //        return;

            //    Write();

            //    this.ResetDirty();
            //    //if (Utility.AutoRefresh)
            //    //    AssetDatabase.Refresh();
            //}
        }
        #endregion
#endif
    }
}
