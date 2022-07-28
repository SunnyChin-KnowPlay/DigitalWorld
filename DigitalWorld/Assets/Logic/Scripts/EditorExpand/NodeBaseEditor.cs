#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DigitalWorld.Logic
{
    public partial class NodeBase
    {
        #region Event
        public delegate void OnDirtyChangedHandle(bool dirty);
        public event OnDirtyChangedHandle OnDirtyChanged;
        #endregion

        public virtual void SetDirty()
        {
            if (null != _parent)
            {
                _parent.SetDirty();
            }
        }


#if UNITY_EDITOR
        #region Params
        /// <summary>
        /// 是否为脏
        /// </summary>
        public virtual bool IsDirty
        {
            get
            { return _dirty; }
        }
        protected bool _dirty = false;

        /// <summary>
        /// 是否正在编辑中
        /// </summary>
        public bool IsEditing => _isEditing;
        protected bool _isEditing = true;
        #endregion

        #region Common
        public virtual void ResetDirty()
        {
            _dirty = false;
            if (null != OnDirtyChanged)
            {
                OnDirtyChanged.Invoke(_dirty);
            }
        }
        #endregion

        #region GUI
        public virtual void OnGUI()
        {
            OnGUITitle();
            if (this._isEditing)
            {
                OnGUIBody();
            }
        }

        public virtual void OnGUITitle()
        {
            bool old = _isEditing;
            this._isEditing = EditorGUILayout.Toggle(old, GUILayout.Width(25));
            if (old != _isEditing)
            {
                if (this.Parent != null)
                    this.Parent.SetDirty();
            }

            this.OnGUIType();
            this.OnGUIIndex();
        }

        protected virtual void OnGUIType()
        {
            EditorGUILayout.LabelField(string.Format("{0}：", this.TypeName));
        }

        protected virtual void OnGUIIndex()
        {
            EditorGUILayout.LabelField(string.Format("{0}", _index + 1));
        }

        protected virtual void OnGUIBody()
        {

        }
        #endregion
#endif
    }
}
