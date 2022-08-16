#if UNITY_EDITOR
using Dream.FixMath;
using System;
using System.Reflection;
using UnityEditor;
#endif
using UnityEngine;

namespace DigitalWorld.Logic
{
    public partial class NodeBase
    {




#if UNITY_EDITOR

        #region Params
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

        protected Vector2 fieldScrollPos;
        #endregion

        #region Common
        /// <summary>
        /// 重置脏是由上至下的
        /// 如果重置了 则自己的所有子节点也一并重置 因为都保存了
        /// </summary>
        public virtual void ResetDirty()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].ResetDirty();
            }

            if (_dirty)
            {
                _dirty = false;
                if (null != OnDirtyChanged)
                {
                    OnDirtyChanged.Invoke(_dirty);
                }
            }
        }

        protected virtual string GetFieldDesc(string fieldName)
        {
            return string.Empty;
        }

        protected void ForeachSetEditing(bool isEditing)
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                this._children[i].ForeachSetEditing(isEditing);
            }
            this._isEditing = isEditing;
        }
        #endregion

        #region GUI
        /// <summary>
        /// 正在渲染扩展编辑
        /// </summary>
        public virtual void OnGUIExtEditing()
        {
            Type type = this.GetType();
            // 这里获取到了所有的字段 把他们一个一个的显示出来
            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

            GUIStyle style = new GUIStyle(GUI.skin.box);
            fieldScrollPos = EditorGUILayout.BeginScrollView(fieldScrollPos, style);

            for (int i = 0; i < fields.Length; ++i)
            {
                FieldInfo field = fields[i];
                if (field.IsFamily)
                    continue;


                this.OnGUIEditingField(field);


            }
            EditorGUILayout.EndScrollView();
        }

        public virtual void OnGUI()
        {

            OnGUITitle();
            if (this.IsEditing)
            {
                OnGUIEditing();
            }


        }

        public virtual void OnGUITitle()
        {
            GUIStyle style = new GUIStyle("IN Title");
            style.padding.left = 0;

            int x = 1;

            EditorGUILayout.BeginHorizontal(style);


            _isEditing = EditorGUILayout.Toggle("", _isEditing, EditorStyles.foldout, GUILayout.Width(12));


            this.OnGUIEnabled();
            this.OnGUIIndex();
            this.OnGUITitleTypeName();

            //EditorGUILayout.Space(selfTypeName != null ? selfTypeName.Length * 5 : 0);




            this.OnGUIName();
            this.OnGUITitleInfo();

            GUILayout.FlexibleSpace();

            this.OnGUITitleMenus();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 标题信息
        /// </summary>
        protected virtual void OnGUITitleInfo()
        {

        }

        protected virtual void OnGUITitleMenus()
        {
            if (GUILayout.Button("OpenAll"))
            {
                ForeachSetEditing(true);
            }

            if (GUILayout.Button("CloseAll"))
            {
                ForeachSetEditing(false);
            }
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

        protected virtual void OnGUIEnabled()
        {
            bool old = _enabled;
            this._enabled = EditorGUILayout.Toggle(old, GUILayout.Width(12));
            if (old != _enabled)
            {
                this.SetDirty();
            }
        }

        protected virtual void OnGUIIndex()
        {
            // 没有父节点的 不会有索引相关的设定
            if (null == this._parent)
                return;

            string title = string.Format("{0}.{1:00}", "Node", _index);
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.alignment = TextAnchor.MiddleCenter;

            if (GUILayout.Button(title, style, GUILayout.Width(60)))
            {
                GenericMenu menu = new GenericMenu();
                bool isDisable = false;

                for (int k = 0; k < _parent.Children.Count; ++k)
                {
                    isDisable = k == this._index;
                    string nameText = string.Format("{0}.{1:00}", "Node", k);
                    if (isDisable)
                    {
                        menu.AddDisabledItem(new GUIContent(nameText));
                    }
                    else
                    {
                        menu.AddItem(new GUIContent(nameText), false, (object userData) =>
                        {
                            int newIndex = (int)userData;

                            _parent.Children.RemoveAt(this._index);
                            _parent.Children.Insert(newIndex, this);

                            this.SetDirty();
                            _parent.ResetChildrenIndex();
                        }, k);
                    }
                }
                menu.ShowAsContext();
            }
        }

        protected virtual void OnGUITitleTypeName()
        {
            string selfTypeName = this.SelfTypeName;
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField(selfTypeName, labelStyle, GUILayout.Width(160));
        }

        protected virtual void OnGUIEditingField(FieldInfo field)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            EditorGUILayout.BeginHorizontal(style);

            bool ret = OnGUICustomEditingField(field);
            if (!ret)
            {
                if (field.FieldType == typeof(int))
                {
                    int v = (int)field.GetValue(this);
                    v = EditorGUILayout.IntField(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                    field.SetValue(this, v);
                }
                else if (field.FieldType == typeof(float))
                {
                    float v = (float)field.GetValue(this);
                    v = EditorGUILayout.FloatField(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                    field.SetValue(this, v);
                }
                else if (field.FieldType == typeof(bool))
                {
                    bool v = (bool)field.GetValue(this);
                    v = EditorGUILayout.Toggle(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                    field.SetValue(this, v);
                }
                else if (field.FieldType == typeof(string))
                {
                    string v = (string)field.GetValue(this);
                    v = EditorGUILayout.TextField(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                    field.SetValue(this, v);
                }
                else if (field.FieldType == typeof(FixVector3))
                {
                    FixVector3 v = (FixVector3)field.GetValue(this);
                    Vector3Int vi = new Vector3Int(v.x, v.y, v.z);
                    vi = EditorGUILayout.Vector3IntField(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), vi);
                    v = new FixVector3(vi.x, vi.y, vi.z);
                    field.SetValue(this, v);
                }
                else if (field.FieldType == typeof(Color))
                {
                    Color v = (Color)field.GetValue(this);
                    v = EditorGUILayout.ColorField(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                    field.SetValue(this, v);
                }
                else if (field.FieldType.BaseType == typeof(Enum))
                {
                    Enum v = (Enum)field.GetValue(this);
                    v = EditorGUILayout.EnumPopup(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);

                    field.SetValue(this, v);
                }

            }

            EditorGUILayout.EndHorizontal();
        }

        protected bool OnGUICustomEditingField(FieldInfo info)
        {
            string methodName = string.Format("OnGUICustomEditingField{0}", info.Name);
            Type t = this.GetType();
            MethodInfo methodInfo = t.GetMethod(methodName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (methodInfo == null)
                return false;

            object[] objs = new object[1] { info };
            methodInfo.Invoke(this, objs);
            return true;
        }

        protected virtual void OnGUIEditing()
        {
            OnGUIChildren();
        }

        protected virtual void OnGUIChildren()
        {
            if (this._children.Count > 0)
            {

                EditorGUILayout.BeginVertical();
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

        /// <summary>
        /// 当为字段渲染时
        /// </summary>
        protected virtual void OnGUIField()
        {

        }
        #endregion
#endif
    }
}
