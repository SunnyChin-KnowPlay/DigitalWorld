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
        internal virtual bool IsEditing { get => _isEditing; set => _isEditing = value; }
        protected bool _isEditing = true;

        /// <summary>
        /// 描述信息
        /// </summary>
        public virtual string Desc
        {
            get
            {
                return string.Empty;
            }
        }
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

        protected virtual string GetFieldDesc(string fieldName)
        {
            return string.Empty;
        }
        #endregion

        #region GUI
        public virtual void OnGUI()
        {
            OnGUITitle();
            if (this.IsEditing)
            {
                OnGUIBody();
            }
        }

        public virtual void OnGUITitle()
        {
            GUIStyle style = new GUIStyle("Tooltip");
            EditorGUILayout.BeginHorizontal(style);

            bool old = _enabled;
            this._enabled = EditorGUILayout.Toggle(old, GUILayout.Width(25));
            if (old != _enabled)
            {
                if (this.Parent != null)
                    this.Parent.SetDirty();
            }

            this.OnGUIName();
            this.OnGUIIndex();

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void OnGUIDescription()
        {
            string srcDesc = this._description;
            this._description = EditorGUILayout.TextArea(this._description, GUILayout.MinHeight(20));
            if (srcDesc != _description)
            {
                this.SetDirty();
            }
        }

        protected virtual void OnGUIName()
        {
            string old = _name;
            _name = EditorGUILayout.TextField(_name, GUILayout.MaxWidth(160));
            if (old != _name)
            {
                this.SetDirty();
            }
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
            OnGUIChildren();
        }

        protected virtual void OnGUIChildren()
        {
            if (this._children.Count > 0)
            {
                GUIStyle style = new GUIStyle("Tooltip");
                EditorGUILayout.BeginVertical(style);
                for (int i = 0; i < this._children.Count; ++i)
                {
                    NodeBase node = this._children[i];
                    if (null != node)
                    {
                        node.OnGUI();
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
        #endregion
#endif
    }
}
