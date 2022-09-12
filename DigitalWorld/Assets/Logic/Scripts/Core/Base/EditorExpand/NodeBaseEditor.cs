#if UNITY_EDITOR
using Dream.FixMath;
using System;
using System.Reflection;
using UnityEditor;
#endif
using UnityEngine;

namespace DigitalWorld.Logic
{
    public abstract partial class NodeBase
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

        public void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            OnGUITitle();
            if (this.IsEditing)
            {
                OnGUIEditing();
            }
            if (EditorGUI.EndChangeCheck())
            {
                this.SetDirty();
            }
        }

        /// <summary>
        /// GUI渲染标题
        /// 标题包含各类信息 横向展示
        /// 开关 索引 类型名 名字 以及标题信息
        /// 接下来还有快捷操作按钮集合
        /// 标题底层覆盖函数 用来实现点击开关
        /// </summary>
        public virtual void OnGUITitle()
        {
            GUIStyle style = new GUIStyle("IN Title");
            style.padding.left = 0;

            Color color = GUI.backgroundColor;
            GUI.backgroundColor = new Color32(160, 160, 160, 255);

            using EditorGUILayout.HorizontalScope h = new EditorGUILayout.HorizontalScope(style);
            GUI.backgroundColor = color;

            _isEditing = EditorGUILayout.Toggle("", _isEditing, EditorStyles.foldout, GUILayout.Width(12));


            this.OnGUIEnabled();
            this.OnGUIIndex();
            this.OnGUITitleTypeName();
            this.OnGUIName();
            this.OnGUITitleInfo();

            GUILayout.FlexibleSpace();

            this.OnGUITitleMenus();

            if (GUI.Button(h.rect, GUIContent.none, EditorStyles.whiteLabel))
            {
                OnClickTitle(h);
            }
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
            this._description = EditorGUILayout.TextArea(this._description, GUILayout.MinHeight(20));
        }

        protected virtual void OnGUIName()
        {
            _name = EditorGUILayout.TextField(_name, GUILayout.MaxWidth(160));
        }

        protected virtual void OnGUIType()
        {
            EditorGUILayout.LabelField(string.Format("{0}：", this.TypeName));
        }

        protected virtual void OnGUIEnabled()
        {
            this._enabled = EditorGUILayout.Toggle(this._enabled, GUILayout.Width(12));   
        }

        protected virtual void OnGUIIndex()
        {
            // 没有父节点的 不会有索引相关的设定
            if (null == this._parent)
                return;

            string title = string.Format("{0}.{1:000}", "Node", _index);
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.alignment = TextAnchor.MiddleCenter;

            if (GUILayout.Button(title, style, GUILayout.Width(72)))
            {
                GenericMenu menu = new GenericMenu();
                bool isDisable = false;

                for (int k = 0; k < _parent.Children.Count; ++k)
                {
                    isDisable = k == this._index;
                    string nameText = string.Format("{0}.{1:000}", "Node", k);
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
            string localTypeName = this.LocalTypeName;
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField(localTypeName, labelStyle, GUILayout.Width(240));
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

        protected void OnGUIEditingField(FieldInfo field, ref Rect rect, Rect parentRect)
        {
            if (field.FieldType == typeof(int))
            {
                int v = (int)field.GetValue(this);
                v = EditorGUI.IntField(rect, new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(float))
            {
                float v = (float)field.GetValue(this);
                v = EditorGUI.FloatField(rect, new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(bool))
            {
                bool v = (bool)field.GetValue(this);
                v = EditorGUI.Toggle(rect, new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(string))
            {
                string v = (string)field.GetValue(this);
                v = EditorGUI.TextField(rect, new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(FixVector3))
            {
                FixVector3 v = (FixVector3)field.GetValue(this);
                Vector3Int vi = new Vector3Int(v.x, v.y, v.z);
                vi = EditorGUI.Vector3IntField(rect, new GUIContent(field.Name, this.GetFieldDesc(field.Name)), vi);
                v = new FixVector3(vi.x, vi.y, vi.z);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(Color))
            {
                Color v = (Color)field.GetValue(this);
                v = EditorGUI.ColorField(rect, new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                field.SetValue(this, v);
            }
            else if (field.FieldType.BaseType == typeof(Enum))
            {
                Enum v = (Enum)field.GetValue(this);
                v = EditorGUI.EnumPopup(rect, new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);

                field.SetValue(this, v);
            }


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
        public virtual void OnGUIField(ref Rect rect, Rect parentRect)
        {

        }
        #endregion

        #region Listener
        protected virtual void OnClickTitle(EditorGUILayout.HorizontalScope horizontalScope)
        {
            _isEditing = !_isEditing;
        }
        #endregion
#endif
    }
}
