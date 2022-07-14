#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DigitalWorld.Logic
{
    public partial class Effect
    {
#if UNITY_EDITOR
        #region GUI
        public override void OnGUI()
        {
            base.OnGUI();
        }

        public override void OnGUITitle()
        {
            bool old = _isEditing;
            this._isEditing = EditorGUILayout.Toggle(old, GUILayout.Width(25));
            if (old != _isEditing)
            {
                if (this.Parent != null)
                    this.Parent.SetDirty();
            }

            this.OnGUIType();
        }
        #endregion
#endif
    }
}
