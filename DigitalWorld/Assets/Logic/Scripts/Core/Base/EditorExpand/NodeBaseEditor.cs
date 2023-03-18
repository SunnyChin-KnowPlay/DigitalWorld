#if UNITY_EDITOR
using DigitalWorld.Logic.Actions;
using DigitalWorld.Logic.Properties;
using Dream.FixMath;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DigitalWorld.Logic
{
    public abstract partial class NodeBase
    {
        #region Packages
        protected struct FieldPkg
        {
            public FieldInfo field;
            public object obj;

            public FieldPkg(FieldInfo i, object o)
            {
                field = i;
                obj = o;
            }
        }
        #endregion

        #region Params
        protected static Color standTitleBackgroundColor = new Color(160 / 255f, 160 / 255f, 160 / 255f, 255 / 255f);
        protected static Color standEvenBackgroundColor = new Color(160 / 255f, 160 / 255f, 160 / 255f, 255 / 255f);
        protected static Color standOddBackgroundColor = new Color(220 / 255f, 220 / 255f, 220 / 255f, 255 / 255f);

        public bool IsSelected
        {
            get => LastedSelectedNode == this;
        }

        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 最后一次选择的节点
        /// </summary>
        public NodeBase LastedSelectedNode
        {
            get
            {
                return Root.GetNodeByGlobalIndex(Root.LastedSelectedGlobalIndex);
            }
        }

        public static FieldInfo LastedSelectedFieldInfo => lastedSelectedFieldInfo;
        private static FieldInfo lastedSelectedFieldInfo = null;

        [Newtonsoft.Json.JsonIgnore]
        public virtual Color TitleBackgroundColor
        {
            get
            {
                return standTitleBackgroundColor;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public Color StandBackgroundColor
        {
            get => this._index % 2 == 0 ? standEvenBackgroundColor : standOddBackgroundColor;
        }

        /// <summary>
        /// 判断自Root到自身是否都为Editing
        /// </summary>
        public bool IsEditingFromRoot
        {
            get
            {
                NodeBase node = this;
                while (null != node)
                {
                    if (!node.IsEditing)
                        return false;

                    node = node.Parent;
                }
                return true;
            }
        }

        /// <summary>
        /// 是否可以被删除
        /// </summary>
        public virtual bool IsCanDelete
        {
            get => true;
        }

        /// <summary>
        /// 是否正在编辑中
        /// </summary>
        public virtual bool IsEditing { get => _isEditing; set => _isEditing = value; }
        [XmlIgnore]
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

        [Newtonsoft.Json.JsonIgnore]
        protected Vector2 fieldScrollPos;

        /// <summary>
        /// 公共字段队列
        /// </summary>
        protected List<FieldInfo> FieldInfos
        {
            get
            {
                if (null == fieldInfos)
                {
                    System.Type type = this.GetType();
                    // 这里获取到了所有的字段 把他们一个一个的显示出来
                    FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                    fieldInfos = new List<FieldInfo>(fields.Length);
                    fieldInfos.AddRange(fields);
                }
                return fieldInfos;
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        protected List<FieldInfo> fieldInfos = null;
        [Newtonsoft.Json.JsonIgnore]
        protected readonly List<string> brotherNames = new List<string>();
        [Newtonsoft.Json.JsonIgnore]
        protected ReorderableList reorderableFieldList;
        protected ReorderableList RreorderableFieldList
        {
            get
            {
                if (null == reorderableFieldList)
                {
                    reorderableFieldList = new ReorderableList(this.FieldInfos, typeof(FieldInfo))
                    {
                        drawElementCallback = OnDrawFieldElement,
                        drawHeaderCallback = OnDrawFieldHead,
                        onSelectCallback = OnFieldSelectCallback,
                        displayAdd = false,
                        displayRemove = false,
                        draggable = false,
                        footerHeight = 0,
                    };
                }
                return reorderableFieldList;
            }
        }

        private static GameObject bufferGameObject = null;
        [Newtonsoft.Json.JsonIgnore]
        protected Dictionary<int, NodeBase> GlobalNodes
        {
            get
            {
                if (this.IsRoot && null == globalNodes)
                {
                    globalNodes = new Dictionary<int, NodeBase>();
                }
                return globalNodes;
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        private Dictionary<int, NodeBase> globalNodes = null;
        [Newtonsoft.Json.JsonIgnore]
        protected Rect lastedGUILayoutRect = Rect.zero;
        #endregion

        #region Logic
        public NodeBase()
        {


            _isEditing = Utility.NodeDefaultEditing;
        }

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

        protected List<string> CalculateBrotherNames()
        {
            brotherNames.Clear();

            if (null != this._parent)
            {
                List<NodeBase> children = _parent.Children;
                foreach (NodeBase brother in children)
                {
                    brotherNames.Add(brother.Name);
                }
            }

            return brotherNames;
        }

        protected int GetBrotherIndex(string name)
        {
            int index = -1;

            for (int i = 0; i < brotherNames.Count; ++i)
            {
                string n = brotherNames[i];
                if (n == name)
                {
                    index = i; break;
                }
            }

            return index;
        }

        public string[] GetFieldNames(List<System.Type> types)
        {
            List<string> classList = new List<string>();

            foreach (System.Type type in types)
            {
                string v = type.ToString();
                classList.Add(v);
            }

            return classList.ToArray();
        }

        public string[] GetFieldDisplayNames(List<System.Type> types)
        {
            List<string> classList = new List<string>();

            foreach (System.Type type in types)
            {
                string v = type.ToString();
                classList.Add(v.Replace('.', '/'));
            }

            return classList.ToArray();
        }

        public static string FindPropertyType(string[] types, int index)
        {
            if (index < 0 || index >= types.Length)
                return types[0];

            return types[index];
        }

        public static int FindPropertyTypeIndex(string[] types, string v)
        {
            int index = 0;
            for (int i = 0; i < types.Length; ++i)
            {
                if (types[i] == v)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        protected static GameObject GetBufferObject()
        {
            bufferGameObject = new GameObject();
            return bufferGameObject;
        }

        /// <summary>
        /// 检查是否可以保存
        /// 如果有问题则抛出异常
        /// </summary>
        public virtual void CheckCanSave()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                ThrowException("Save Err! The node is null");
            }

            // 首字母不可以是下划线
            if (this.Name[0] == '_')
            {
                ThrowException("Save Err! 首字母不可以是下划线 _");
            }

            // 让子节点自己检查下是否有问题
            foreach (NodeBase child in _children)
            {
                child.CheckCanSave();
            }

            // 检查下是否有同名的
            Dictionary<string, NodeBase> nodes = new Dictionary<string, NodeBase>();
            foreach (NodeBase child in _children)
            {
                if (nodes.ContainsKey(child.Name))
                {
                    ThrowException(string.Format("Save Err! The node name is exits, name is:{0}", child.Name));
                }
                nodes.Add(child.Name, child);
            }
        }

        public bool CheckIsSelectedFromRoot()
        {
            NodeBase node = this;
            while (null != node)
            {
                if (node.IsSelected)
                    return true;

                node = node.Parent;
            }
            return false;
        }

        public bool CheckIsDisabledFromRoot()
        {
            NodeBase node = this;
            while (null != node)
            {
                if (!node.Enabled)
                    return true;

                node = node.Parent;
            }
            return false;
        }

        public void ClearSelected()
        {
            LastedSelectedGlobalIndex = -1;
        }

        /// <summary>
        /// 自动选择一个最近的节点
        /// </summary>
        /// <param name="offset">-1为向上 1为向下 0为自动，如果下有就下 没有就上</param>
        public void AutoSelect(int offset)
        {
            if (offset == 0)
            {
                this.LastedSelectedGlobalIndex = Mathf.Clamp(LastedSelectedGlobalIndex, 0, MaxGlobalIndex - 1);
            }
            else if (offset < 0)
            {
                int srcSelected = LastedSelectedGlobalIndex;
                do
                {
                    int lasted = LastedSelectedGlobalIndex;
                    this.LastedSelectedGlobalIndex = Mathf.Max(0, this.LastedSelectedGlobalIndex - 1);
                    if (lasted == this.LastedSelectedGlobalIndex)
                    {
                        LastedSelectedGlobalIndex = srcSelected;
                        break;
                    }

                    if (null == LastedSelectedNode)
                        break;

                    if (null == LastedSelectedNode.Parent)
                        break;

                    if (LastedSelectedNode.Parent.IsEditingFromRoot)
                        break;

                } while (true);

                if (srcSelected != LastedSelectedGlobalIndex && null != OnGlobalSelectChanged)
                {
                    OnGlobalSelectChanged.Invoke(this, _lastedSelectedGlobalIndex);
                }
            }
            else
            {
                int srcSelected = LastedSelectedGlobalIndex;
                do
                {
                    int lasted = LastedSelectedGlobalIndex;
                    this.LastedSelectedGlobalIndex = Mathf.Min(this.MaxGlobalIndex - 1, this.LastedSelectedGlobalIndex + 1);
                    if (lasted == this.LastedSelectedGlobalIndex)
                    {
                        LastedSelectedGlobalIndex = srcSelected;
                        break;
                    }

                    if (null == LastedSelectedNode)
                        break;

                    if (null == LastedSelectedNode.Parent)
                        break;

                    if (LastedSelectedNode.Parent.IsEditingFromRoot)
                    {
                        break;
                    }
                } while (true);

                if (srcSelected != LastedSelectedGlobalIndex && null != OnGlobalSelectChanged)
                {
                    OnGlobalSelectChanged.Invoke(this, _lastedSelectedGlobalIndex);
                }
            }
        }

        public Rect GetGUILayoutRect()
        {
            return lastedGUILayoutRect;
        }

        public NodeBase GetNodeByGlobalIndex(int index)
        {
            Dictionary<int, NodeBase> globalNodes = this.GlobalNodes;
            globalNodes.TryGetValue(index, out NodeBase ret);

            return ret;
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <returns></returns>
        public string Copy()
        {

            using (TextWriter writer = new StringWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                xmlSerializer.Serialize(writer, this);
                return writer.ToString();
            }
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        /// <param name="data"></param>
        public void Paste(string data)
        {
            try
            {
                using (TextReader reader = new StringReader(data))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                    if (xmlSerializer.Deserialize(reader) is NodeBase node)
                    {
                        node.CloneTo(this);
                    }
                }
            }
            catch
            {
                string err = string.Format("Paste err! The copy buffer is:{0}", data);
                UnityEngine.Debug.LogError(err);
            }
        }

        public delegate void OnProcessChildHandle(NodeBase child);
        public void ForeachChildren(OnProcessChildHandle handle)
        {
            foreach (NodeBase child in _children)
            {
                if (null != child)
                {
                    if (null != handle)
                    {
                        handle.Invoke(child);
                    }

                    child.ForeachChildren(handle);
                }
            }
        }

        #endregion

        #region Clone
        protected virtual void EditorCloneTo<T>(T obj) where T : NodeBase
        {
            obj._isEditing = this.IsEditing;
        }
        #endregion

        #region GUI
        private Color GetNormalBackgroundColor()
        {
            NodeBase root = this.Root;
            float start = null != root && root is Level ? 0.8f : 1f;

            float value = start * (this.Index % 2 == 0 ? 1 : 1);
            return new Color(value, value, value, 1.0f);
        }

        protected virtual Color GetBackgroundColor()
        {
            if (this.IsSelected)
            {
                int c = 2;
                Color color = GetNodeColor(this.GetType());
                return new Color(color.r * c, color.g * c, color.b * c, color.a * 0.5f);
            }

            if (this.CheckIsDisabledFromRoot())
            {
                return Color.white * 0.5f;
            }

            return GetNormalBackgroundColor();
        }

        protected virtual Color GetContentColor()
        {
            return Color.white;
        }

        public void OnGUI(Rect rect, int index, bool selected, bool focused)
        {

        }

        internal static void DrawOutline(Rect rect, float size, Color color)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Color color2 = GUI.color;

                GUI.color = color;
                GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, size), EditorGUIUtility.whiteTexture);
                GUI.DrawTexture(new Rect(rect.x, rect.yMax - size, rect.width, size), EditorGUIUtility.whiteTexture);
                GUI.DrawTexture(new Rect(rect.x, rect.y + 1f, size, rect.height - 2f * size), EditorGUIUtility.whiteTexture);
                GUI.DrawTexture(new Rect(rect.xMax - size, rect.y + 1f, size, rect.height - 2f * size), EditorGUIUtility.whiteTexture);
                GUI.color = color2;
            }
        }

        public void OnGUI()
        {
            using (EditorGUI.ChangeCheckScope cc = new EditorGUI.ChangeCheckScope())
            {
                GUIStyle style = new GUIStyle("FrameBox");

                if (this.CheckIsDisabledFromRoot())
                {
                    Color oldBackgroundColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.white / 2;

                    Color oldContentColor = GUI.contentColor;
                    GUI.contentColor = Color.white / 2;

                    using (EditorGUILayout.VerticalScope v = new EditorGUILayout.VerticalScope(style))
                    {
                        OnGUITitle();

                        if (this.IsEditing)
                        {
                            OnGUIEditing();
                        }

                        lastedGUILayoutRect = v.rect;
                    }

                    GUI.backgroundColor = oldBackgroundColor;
                    GUI.contentColor = oldContentColor;
                }
                else
                {
                    Color oldBackgroundColor = GUI.backgroundColor;
                    GUI.backgroundColor = GetBackgroundColor();

                    using (EditorGUILayout.VerticalScope v = new EditorGUILayout.VerticalScope(style))
                    {
                        GUI.backgroundColor = oldBackgroundColor;

                        OnGUITitle();

                        if (this.IsEditing)
                        {
                            OnGUIEditing();
                        }

                        lastedGUILayoutRect = v.rect;
                        float scale = this.IsSelected ? 1.7f : .85f;
                        DrawOutline(v.rect, 1, GetNodeColor(this.GetType()) * scale);
                    }
                }
                if (cc.changed)
                {
                    this.SetDirty();
                }
            }
        }

        protected virtual GUIStyle GetTitleStyle()
        {
            //string styleName = this.IsEditing ? "AC BoldHeader" : "FrameBox";
            string styleName = "ExposablePopupItem";
            GUIStyle style = new GUIStyle(styleName)
            {
                padding = new RectOffset(5, 5, 0, 0),
                margin = new RectOffset(2, 2, 0, 0),
            };

            //string log = string.Format("name is:{0} margin is:{1}", styleName, style.margin);
            //Debug.LogError(log);

            return style;
        }

        /// <summary>
        /// GUI渲染标题
        /// 标题包含各类信息 横向展示
        /// 开关 索引 类型名 名字 以及标题信息
        /// 接下来还有快捷操作按钮集合
        /// 标题底层覆盖函数 用来实现点击开关
        /// </summary>
        public void OnGUITitle()
        {
            Color color = GUI.backgroundColor;
            GUI.backgroundColor = GetBackgroundColor();

            using (EditorGUILayout.HorizontalScope h = new EditorGUILayout.HorizontalScope(GetTitleStyle()))
            {
                GUI.backgroundColor = color;

                this.OnGUIIsEditing();
                this.OnGUIEnabled();
                this.OnGUIIndex();
                this.OnGUITitleTypeName();
                this.OnGUIName();
                this.OnGUIDescription();
                this.OnGUIParamsPreview();

                GUILayout.FlexibleSpace();

                this.OnGUITitleContent();
                this.OnGUITitleMenus();

                if (GUI.Button(h.rect, GUIContent.none, EditorStyles.whiteLabel))
                {
                    OnClickTitle();
                }
            }
        }

        /// <summary>
        /// 标题信息
        /// </summary>
        protected virtual void OnGUITitleContent()
        {

        }

        protected virtual void OnGUITitleMenus()
        {
            if (GUILayout.Button(Utility.GUIContentD__Menu, Utility.StyleD__MenuIconButton))
            {
                OnClickMenu();
            }
        }

        protected virtual void OnGUIDescription()
        {
            GUIStyle style = new GUIStyle(GUI.skin.textField)
            {
                //fontStyle = FontStyle.Bold
                margin = new RectOffset(3, 3, 0, 5),
            };
            Color color = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 1) * 2f;
            this._description = EditorGUILayout.TextField(this._description, style, GUILayout.Width(200));
            GUI.backgroundColor = color;
        }

        protected virtual void OnGUIName()
        {
            if (this.IsRoot)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    //fontStyle = FontStyle.Bold
                    margin = new RectOffset(3, 3, 0, 5),
                };

                EditorGUILayout.LabelField(_name, style, GUILayout.MaxWidth(160));
            }
            else
            {
                GUIStyle style = new GUIStyle(GUI.skin.textField)
                {
                    //fontStyle = FontStyle.Bold
                    margin = new RectOffset(3, 3, 0, 5),
                };

                _name = EditorGUILayout.TextField(_name, style, GUILayout.Width(160));
            }
        }

        //protected virtual void OnGUITitleContent()
        //{

        //}

        /// <summary>
        /// 渲染参数预览
        /// </summary>
        protected virtual void OnGUIParamsPreview()
        {
            StringBuilder title = new StringBuilder();
            Type type = this.GetType();

            #region 字段
            // 这里获取到了所有的字段 把他们一个一个的显示出来
            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < fields.Length; ++i)
            {
                FieldInfo field = fields[i];
                if (field.IsFamily)
                    continue;

                string fieldName = field.Name;
                title.AppendFormat(" <color=#2AE336FF>{0}</color>=<color=#FFFFFFFF>{1}</color>", fieldName, field.GetValue(this)?.ToString());
            }
            #endregion 字段

            #region 属性子节点
            var children = this._children;
            for (int i = 0; i < children.Count; ++i)
            {
                if (children[i] is PropertyBase property)
                {
                    string propertyName = property.Name.Replace("Property_", "");
                    title.AppendFormat(" <color=#2ACDE2FF>{0}</color>=<color=#FFFFFFFF>{1}</color>", propertyName, property.GetParamsPreviewString());
                }
            }
            #endregion 属性子节点
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                richText = true,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(0, 0, 0, 4)
            };
            string titleString = title.ToString();

            EditorGUILayout.LabelField(titleString, style, GUILayout.MaxWidth(1000));

        }

        protected virtual void OnGUIType(ref Rect rect, int index = 0, bool selected = false, bool focused = false)
        {
            EditorGUILayout.LabelField(string.Format("{0}：", this.TypeName));
        }

        protected virtual void OnGUIIsEditing()
        {
            if (!this.IsRoot)
            {
                GUIStyle style = new GUIStyle(EditorStyles.foldout)
                {
                    margin = new RectOffset(0, 0, 0, 0),
                };
                _isEditing = EditorGUILayout.Toggle("", _isEditing, style, GUILayout.Width(12));
            }
        }

        protected virtual void OnGUIEnabled()
        {
            if (!this.IsRoot)
            {
                this._enabled = EditorGUILayout.Toggle(this._enabled, GUILayout.Width(12));
            }
        }

        protected virtual void OnGUIIndex()
        {
            // 没有父节点的 不会有索引相关的设定
            if (null == this._parent)
                return;

            string title = string.Format("{0}.{1:000}", "Node", _index);
            GUIStyle style = GetGUITitleTypeNameStyle(this.NodeType);
            style.alignment = TextAnchor.MiddleCenter;

            GUILayout.Space(4);
            //{
            //    alignment = TextAnchor.MiddleCenter
            //};

            if (GUILayout.Button(title, style, GUILayout.Width(72), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
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

            GUILayout.Space(4);
        }

        protected virtual GUIStyle GetGUITitleTypeNameStyle(ENodeType nodeType)
        {
            string styleName;
            switch (nodeType)
            {
                case ENodeType.Action:
                {
                    styleName = "flow node 5";
                    break;
                }
                case ENodeType.Trigger:
                {
                    styleName = "flow node 1";
                    break;
                }
                case ENodeType.Level:
                {
                    styleName = "flow node 0";
                    break;
                }
                case ENodeType.Property:
                {
                    styleName = "flow node 2";
                    break;
                }
                default:
                {
                    styleName = "flow node 0";
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
                //fontStyle = FontStyle.Bold,
            };

            return style;
        }

        protected virtual void OnGUITitleTypeName()
        {
            GUIStyle style = GetGUITitleTypeNameStyle(this.NodeType);
            GUILayout.Button(this.LocalTypeName, style, GUILayout.Width(240), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            //EditorGUILayout.LabelField(this.LocalTypeName, style, GUILayout.Width(240));
        }

        protected virtual void OnGUIEditing()
        {
            OnGUIEditingFieldInfo();

            OnGUIChildren();
        }

        protected void OnGUIChildren()
        {
            if (this._children.Count > 0)
            {
                GUIStyle style = new GUIStyle("FrameBox");

                using (EditorGUILayout.VerticalScope v = new EditorGUILayout.VerticalScope(style))
                {
                    OnGUIChildrenTitle();
                    OnGUIChildrenContent();
                }
            }
            else
            {
                OnGUIChildrenTitle();
            }
        }

        protected virtual void OnGUIChildrenTitle()
        {

        }

        protected virtual Color GetNodeColor(Type type)
        {
            if (type.IsSubclassOf(typeof(Trigger)) || type == typeof(Trigger))
            {
                return new Color(56 / 255f, 96 / 255f, 158 / 255f, 1);
            }
            if (type.IsSubclassOf(typeof(ActionBase)) || type == typeof(ActionBase))
            {
                return new Color(195 / 255f, 110 / 255f, 18 / 255f, 1);
            }
            if (type.IsSubclassOf(typeof(PropertyBase)) || type == typeof(PropertyBase))
            {
                return new Color(56 / 255f, 139 / 255f, 119 / 255f, 1);
            }

            return new Color(.5f, .5f, .5f, 1);
            //return new Color(73 / 255f, 75 / 255f, 78 / 255f, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnGUIHorizontalSeparator(int offset, Color backgroundColor, Color frameColor)
        {
            GUIStyle style = new GUIStyle("DD background")
            {
                padding = new RectOffset(offset, offset, offset, offset)
            };

            using (EditorGUILayout.VerticalScope v = new EditorGUILayout.VerticalScope(style))
            {
                EditorGUI.DrawRect(v.rect, backgroundColor);
                if (offset > 1)
                {
                    DrawOutline(v.rect, 1, frameColor);
                }
            }
        }

        protected virtual void OnGUIHorizontalDottedLineSeparator(int offset, int horizontalOffset, int count, Color backgroundColor, Color frameColor, int step)
        {
            GUIStyle style = new GUIStyle("dockarea")
            {
                margin = new RectOffset(horizontalOffset, horizontalOffset, 0, 0),
                padding = new RectOffset(offset, offset, offset, offset),
            };

            Color c = GUI.backgroundColor;
            GUI.backgroundColor = frameColor;

            using (EditorGUILayout.VerticalScope v = new EditorGUILayout.VerticalScope(style))
            {
                GUI.backgroundColor = c;

                count = count * 2 - 1;
                float partWidth = v.rect.width / count;
                float splitWidth = partWidth / 10;

                for (int i = 0; i < count; ++i)
                {
                    if (i % 2 == step)
                    {
                        float x = (v.rect.x + i * partWidth);
                        Rect r = new Rect(x, v.rect.y, partWidth, v.rect.height);
                        EditorGUI.DrawRect(r, backgroundColor);
                    }
                }
            }
        }

        protected virtual void OnGUIChildrenContent()
        {
            Color frameColor = GetNodeColor(this.GetType());
            for (int i = 0; i < this._children.Count; ++i)
            {
                NodeBase node = this._children[i];
                if (null != node)
                {
                    node.OnGUI();

                    if (!node.IsEditing)
                        continue;

                    if (i < this._children.Count - 1)
                    {
                        OnGUIHorizontalDottedLineSeparator(Math.Max(3 - this.LayerCount, 1), 20, 10, frameColor * 1.8f, new Color(0.2f, 0.2f, 0.2f, 0.2f), this.LayerCount % 2);
                        //OnGUIHorizontalSeparator(Math.Max(3 - this.LayerCount, 1), frameColor, Color.black);
                    }
                }
            }
        }

        protected virtual void OnGUIEditingFieldInfo()
        {
            if (RreorderableFieldList.list.Count > 0)
            {
                GUIStyle style = new GUIStyle("FrameBox");
                using (EditorGUILayout.VerticalScope v = new EditorGUILayout.VerticalScope(style))
                {
                    RreorderableFieldList.DoLayoutList();
                }
            }
        }
        #endregion

        #region GUI/Properties
        protected virtual void OnFieldSelectCallback(ReorderableList list)
        {
            if (list.index >= 0 && list.index < fieldInfos.Count)
            {
                FieldInfo field = fieldInfos[list.index];
                OnClickField(field);
            }
        }

        protected void OnDrawFieldHead(Rect rect)
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold
            };

            labelStyle.normal.textColor = new Color(42 / 255f, 227 / 255f, 54 / 255f, 1);
            EditorGUI.LabelField(rect, "Fields", labelStyle);
        }

        protected void OnDrawFieldElement(Rect rect, int index, bool selected, bool focused)
        {
            Rect parentRect = rect;
            float width = rect.width;
            if (index < fieldInfos.Count)
            {
                FieldInfo field = fieldInfos[index];
                System.Type fieldType = field.FieldType;

                GUIStyle labelStyle = new GUIStyle("minibutton")
                {
                    alignment = TextAnchor.MiddleLeft,
                };

                labelStyle.normal.textColor = Color.white;

                //rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.xMax = rect.xMin + width * 0.2f;
                EditorGUI.LabelField(rect, new GUIContent(field.Name, this.GetFieldDesc(field.Name)), labelStyle);

                Rect separationRect = Rect.MinMaxRect(rect.xMax + 2, rect.yMin, rect.xMax + 4, rect.yMax);
                EditorGUI.DrawRect(separationRect, Logic.Utility.kSplitLineColor);

                rect.xMin = rect.xMax + 6;
                rect.xMax = rect.xMin + width * 0.2f;


                string fieldTypeString = field.FieldType.ToString();
                if (!string.IsNullOrEmpty(fieldTypeString))
                {
                    if (fieldTypeString.Contains('.'))
                    {
                        fieldTypeString = fieldTypeString.Substring(fieldTypeString.IndexOf('.') + 1);
                    }
                    EditorGUI.LabelField(rect, new GUIContent(fieldTypeString, field.FieldType.BaseType.ToString()), labelStyle);
                }

                separationRect = Rect.MinMaxRect(rect.xMax + 2, rect.yMin, rect.xMax + 4, rect.yMax);
                EditorGUI.DrawRect(separationRect, Logic.Utility.kSplitLineColor);

                rect.xMin = rect.xMax + 6;
                rect.xMax = parentRect.xMax - 18;
                OnDrawUnderlyingFieldValue(ref rect, field);

                rect.xMin = rect.xMax + 6;
                rect.xMax = parentRect.xMax;

                if (GUI.Button(rect, Utility.GUIContentD__Menu, Utility.StyleD__MenuIconButton))
                {
                    GameObject g = GetBufferObject();
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Copy"), false, (object userData) =>
                    {
                        GUIUtility.systemCopyBuffer = Convert.ToString(field.GetValue(this));
                    }, null);


                    bool isDisable = string.IsNullOrEmpty(GUIUtility.systemCopyBuffer);
                    if (isDisable)
                    {
                        menu.AddDisabledItem(new GUIContent("Paste"));
                    }
                    else
                    {
                        menu.AddItem(new GUIContent("Paste"), false, (object userData) =>
                        {
                            try
                            {
                                field.SetValue(this, Convert.ChangeType(GUIUtility.systemCopyBuffer, field.FieldType));
                            }
                            catch
                            {
                                string err = string.Format("Paste err! The copy buffer is:{0}", GUIUtility.systemCopyBuffer);
                                UnityEngine.Debug.LogError(err);
                            }
                            finally
                            {
                                this.SetDirty();
                            }
                        }, null);
                    }

                    GameObject.DestroyImmediate(g);
                    menu.ShowAsContext();
                }

                Rect lineRect = Rect.MinMaxRect(parentRect.xMin + 4, parentRect.yMax - 2, parentRect.xMax - 4, parentRect.yMax);
                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);
            }
            else
            {
                fieldInfos.RemoveAt(index);
            }
        }

        protected virtual void OnDrawUnderlyingFieldValue(ref Rect rect, FieldInfo field)
        {
            EditorGUI.BeginChangeCheck();

            if (field.FieldType == typeof(int))
            {
                int v = (int)field.GetValue(this);
                v = EditorGUI.IntField(rect, v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(float))
            {
                float v = (float)field.GetValue(this);
                v = EditorGUI.FloatField(rect, v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(bool))
            {
                bool v = (bool)field.GetValue(this);
                v = EditorGUI.Toggle(rect, v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(string))
            {
                string v = (string)field.GetValue(this);
                v = EditorGUI.TextField(rect, v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(FixFactor))
            {
                FixFactor v = (FixFactor)field.GetValue(this);
                Vector2Int vi = new Vector2Int((int)v.nom, (int)v.den);
                vi = EditorGUI.Vector2IntField(rect, new GUIContent(""), vi);
                v = new FixFactor(vi.x, vi.y);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(FixVector2))
            {
                FixVector2 v = (FixVector2)field.GetValue(this);
                Vector2 vi = new Vector2(v.X, v.Y);
                vi = EditorGUI.Vector2Field(rect, new GUIContent(""), vi);
                v = new FixVector2(vi.x, vi.y);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(FixVector3))
            {
                FixVector3 v = (FixVector3)field.GetValue(this);
                Vector3 vi = new Vector3(v.X, v.Y, v.Z);
                vi = EditorGUI.Vector3Field(rect, new GUIContent(""), vi);
                v = new FixVector3(vi.x, vi.y, vi.z);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(Color))
            {
                Color v = (Color)field.GetValue(this);
                v = EditorGUI.ColorField(rect, v);
                field.SetValue(this, v);
            }
            else if (field.FieldType.IsEnum)
            {
                OnDrawEnumFieldValue(ref rect, field);
            }

            if (EditorGUI.EndChangeCheck())
            {
                this.SetDirty();
            }
        }

        protected virtual void OnDrawEnumFieldValue(ref Rect rect, FieldInfo field)
        {
            bool ret = GUIEnumFieldValues.TryGetValue(field.FieldType, out OnDrawEnumFieldValueHandle handle);
            if (ret && null != handle)
            {
                handle.Invoke(ref rect, field);
            }
            else
            {
                System.Enum v = (System.Enum)field.GetValue(this);
                v = EditorGUI.EnumPopup(rect, v);

                field.SetValue(this, v);
            }
        }

        protected virtual void OnGUIPasteToField<T>(GenericMenu menu, string title, FieldInfo field, T o)
        {
            FieldPkg p = new FieldPkg(field, o);
            menu.AddItem(new GUIContent(title), false, (object userData) =>
            {
                FieldPkg pkg = (FieldPkg)userData;
                T v = (T)pkg.obj;
                pkg.field.SetValue(this, v);
            }, p);
        }
        #endregion

        #region GUI/EnumFields
        public delegate void OnDrawEnumFieldValueHandle(ref Rect rect, FieldInfo field);
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        private Dictionary<Type, OnDrawEnumFieldValueHandle> guiEnumFieldValues = null;
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        public virtual Dictionary<Type, OnDrawEnumFieldValueHandle> GUIEnumFieldValues
        {
            get
            {
                if (null == guiEnumFieldValues)
                {
                    guiEnumFieldValues = new Dictionary<Type, OnDrawEnumFieldValueHandle>
                    {
                        { typeof(ECheckOperator), OnDrawFieldValueCheckOperator },
                        { typeof(ENodeType), OnDrawFieldValueNodeType },
                    };
                }
                return guiEnumFieldValues;
            }
        }

        private readonly static string[] checkOperatorNames = new string[]
        {
            "==",
            "!=",
            ">",
            ">=",
            "<",
            "<=",
        };

        protected virtual string GetOperatorStyleName(ECheckOperator op)
        {
            string styleName;
            switch (op)
            {
                case ECheckOperator.NotEqual:
                {
                    styleName = "flow node 6";
                    break;
                }
                case ECheckOperator.LessThan:
                {
                    styleName = "flow node 2";
                    break;
                }
                case ECheckOperator.LessThanOrEqualTo:
                {
                    styleName = "flow node 1";
                    break;
                }
                case ECheckOperator.GreaterThan:
                {
                    styleName = "flow node 4";
                    break;
                }
                case ECheckOperator.GreaterThanOrEqualTo:
                {
                    styleName = "flow node 5";
                    break;
                }
                default:
                {
                    styleName = "flow node 3";
                    break;
                }
            }

            //if (this.IsSelected)
            //{
            //    styleName += " on";
            //}

            return styleName;
        }

        protected virtual void OnDrawFieldValueCheckOperator(ref Rect rect, FieldInfo field)
        {
            ECheckOperator v = (ECheckOperator)field.GetValue(this);
            GUIStyle style = new GUIStyle(GetOperatorStyleName(v))
            {
                alignment = TextAnchor.MiddleCenter,
                contentOffset = new Vector2(0, -14),
                fontSize = 12,
                fontStyle = FontStyle.Bold,
            };
            v = (ECheckOperator)EditorGUI.Popup(rect, (int)v, checkOperatorNames, style);
            field.SetValue(this, v);
        }

        protected virtual void OnDrawFieldValueNodeType(ref Rect rect, FieldInfo field)
        {
            ENodeType v = (ENodeType)field.GetValue(this);
            GUIStyle style = new GUIStyle(GetGUITitleTypeNameStyle(v));
            v = (ENodeType)EditorGUI.EnumPopup(rect, v, style);
            field.SetValue(this, v);
        }
        #endregion

        #region Listener
        protected virtual void OnClickField(FieldInfo field)
        {
            Root.LastedSelectedGlobalIndex = this.GlobalIndex;

            if (null != OnGlobalSelectChanged)
            {
                OnGlobalSelectChanged.Invoke(this, _lastedSelectedGlobalIndex);
            }

            lastedSelectedFieldInfo = field;
        }

        protected virtual void OnClickTitle()
        {
            if (this.IsSelected)
            {
                _isEditing = !_isEditing;
                lastedSelectedFieldInfo = null;
            }
            else
            {
                GUIUtility.keyboardControl = 0;

                Root.LastedSelectedGlobalIndex = this.GlobalIndex;

                if (null != OnGlobalSelectChanged)
                {
                    OnGlobalSelectChanged.Invoke(this, _lastedSelectedGlobalIndex);
                }
            }
        }

        protected virtual void OnClickRemove()
        {
            this.SetParent(null);
            this.Recycle();
        }

        protected virtual void OnClickMenu()
        {
            bool isDisable;

            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Delete"), false, (object userData) =>
            {
                this.OnClickRemove();
            }, null);

            menu.AddSeparator("");
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
        #endregion

        #region Node Display
        protected EAction selectedAction;

        public static string[] ActionDisplayNames
        {
            get
            {
                if (null == actionDisplayNames)
                {
                    List<string> actionNames = new List<string>();

                    EAction[] actionArray = ActionArray;
                    for (int i = 0; i < actionArray.Length; ++i)
                    {
                        string name = actionArray[i].ToString().Replace('_', '/');
                        name += string.Format(" - {0}", LogicHelper.GetActionDesc((int)actionArray[i]));
                        actionNames.Add(name);
                    }

                    actionDisplayNames = actionNames.ToArray();
                }
                return actionDisplayNames;
            }
        }
        private static string[] actionDisplayNames = null;

        public static string[] GetActionDisplayNamesWithoutSelectedDesc(EAction action)
        {
            List<string> actionNames = new List<string>();

            EAction[] actionArray = ActionArray;
            for (int i = 0; i < actionArray.Length; ++i)
            {
                string name = actionArray[i].ToString().Replace('_', '/');
                if (actionArray[i] != action)
                {
                    name += string.Format(" - {0}", LogicHelper.GetActionDesc((int)actionArray[i]));
                }
                actionNames.Add(name);
            }

            return actionNames.ToArray();
        }


        public static EAction[] ActionArray
        {
            get
            {
                if (null == actionArray)
                {
                    List<EAction> actionList = new List<EAction>();
                    foreach (EAction i in System.Enum.GetValues(typeof(EAction)))
                    {
                        actionList.Add(i);
                    }

                    actionArray = actionList.ToArray();
                }
                return actionArray;
            }
        }
        private static EAction[] actionArray = null;

        public static EAction FindAction(int index)
        {
            if (index < 0 || index >= ActionArray.Length)
                return ActionArray[0];

            return ActionArray[index];
        }

        public static EAction FindAction(string localTypeName)
        {
            EAction[] types = ActionArray;

            if (types.Length == 0)
                return default;

            localTypeName = localTypeName.Replace("Actions.", "");
            string standardName = Utility.GetStandardizationEnumName(localTypeName);

            for (int i = 0; i < types.Length; ++i)
            {
                EAction action = types[i];
                if (action.ToString() == standardName)
                    return action;
            }

            return default;
        }

        public static int FindActionIndex(EAction v)
        {
            int index = 0;

            EAction[] types = ActionArray;

            for (int i = 0; i < types.Length; ++i)
            {
                if (types[i] == v)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public static string[] EventDisplayNames
        {
            get
            {
                if (null == eventDisplayNames)
                {
                    List<string> eventNames = new List<string>();

                    EEvent[] eventArray = EventArray;
                    for (int i = 0; i < eventArray.Length; ++i)
                    {
                        string name = eventArray[i].ToString().Replace('_', '/');
                        name += string.Format(" - {0}", LogicHelper.GetEventDesc((int)eventArray[i]));
                        eventNames.Add(name);
                    }

                    eventDisplayNames = eventNames.ToArray();
                }
                return eventDisplayNames;
            }
        }
        private static string[] eventDisplayNames = null;

        public static EEvent[] EventArray
        {
            get
            {
                if (null == eventArray)
                {
                    List<EEvent> eventList = new List<EEvent>();
                    foreach (EEvent i in System.Enum.GetValues(typeof(EEvent)))
                    {
                        eventList.Add(i);
                    }

                    eventArray = eventList.ToArray();
                }
                return eventArray;
            }
        }
        private static EEvent[] eventArray = null;

        public static EEvent FindEvent(int index)
        {
            if (index < 0 || index >= EventArray.Length)
                return EventArray[0];

            return EventArray[index];
        }

        public static int FindEventIndex(EEvent v)
        {
            int index = 0;

            EEvent[] types = EventArray;

            for (int i = 0; i < types.Length; ++i)
            {
                if (types[i] == v)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        #endregion
    }
}
#endif