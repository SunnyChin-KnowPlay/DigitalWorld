using DigitalWorld.Logic.Actions;
using Dream.Core;
using Dream.Table;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 逻辑根节点
    /// </summary>
    [Serializable]
    public abstract partial class NodeBase : IPooledObject, INode, IComparable, ICloneable, ISerializable
    {
        #region Event
        public delegate void OnDirtyChangedHandle(bool dirty);
        public event OnDirtyChangedHandle OnDirtyChanged;

        public delegate void OnGlobalSelectChangedHandle(NodeBase node, int lastedSelected);
        public event OnGlobalSelectChangedHandle OnGlobalSelectChanged;
        #endregion

        #region Params
        /// <summary>
        /// 索引号
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index == value)
                    return;

                _index = value;
                SetDirty();
            }
        }
        protected int _index = 0;

        public int MaxIndex
        {
            get { return _maxIndex; }
            set { _maxIndex = value; }
        }
        protected int _maxIndex = 0;

        public int GlobalIndex
        {
            get
            {
                return this._globalIndex;
            }
            set
            {
                if (this._globalIndex == value)
                    return;

                this._globalIndex = value;
            }
        }
        protected int _globalIndex = 0;

        public int MaxGlobalIndex
        {
            get { return _maxGlobalIndex; }
            set { _maxGlobalIndex = value; }
        }
        protected int _maxGlobalIndex = 0;

        /// <summary>
        /// 最后一次选择的全局索引号
        /// </summary>
        public int LastedSelectedGlobalIndex
        {
            get => _lastedSelectedGlobalIndex;
            set
            {
                if (_lastedSelectedGlobalIndex != value)
                {
                    _lastedSelectedGlobalIndex = value;
                }
#if UNITY_EDITOR
                lastedSelectedFieldInfo = null;
#endif
            }
        }
        private int _lastedSelectedGlobalIndex = -1;

        public NodeBase Parent => _parent;
        protected NodeBase _parent;

        /// <summary>
        /// 获取根节点，如果自己就是根节点则返回自己
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public NodeBase Root
        {
            get
            {
                return GetRoot(this);
            }
        }

        /// <summary>
        /// 自己是否为根节点
        /// </summary>
        public bool IsRoot
        {
            get
            {
                return this._parent == null;
            }
        }

        /// <summary>
        /// 所有的子节点
        /// </summary>
        public List<NodeBase> Children => _children;
        protected List<NodeBase> _children = new List<NodeBase>();

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        protected bool _enabled = true;

        /// <summary>
        /// 类型名
        /// </summary>
        public virtual string TypeName
        {
            get
            {
                return this.GetType().ToString();
            }
        }

        /// <summary>
        /// 抛去命名空间 自我的类型名
        /// </summary>
        public string SelfTypeName
        {
            get
            {
                string typeName = TypeName;
                if (string.IsNullOrEmpty(typeName))
                    return typeName;

                if (!typeName.Contains('.'))
                    return typeName;

                return typeName.Substring(typeName.LastIndexOf('.') + 1);
            }
        }

        /// <summary>
        /// 本地的类型名
        /// </summary>
        public virtual string LocalTypeName
        {
            get
            {
                string typeName = TypeName;
                if (string.IsNullOrEmpty(typeName))
                    return typeName;

                if (!typeName.Contains('.'))
                    return typeName;

                if (!typeName.Contains(Utility.LogicNamespace))
                    return typeName;

                return typeName.Substring(typeName.IndexOf(Utility.LogicNamespace) + Utility.LogicNamespace.Length + 1);
            }
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public abstract ENodeType NodeType { get; }

        /// <summary>
        /// 效果的配置ID
        /// </summary>
        public abstract int Id { get; }


        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        [XmlAttribute]
        protected string _name;

        /// <summary>
        /// 已激活的持续时间
        /// </summary>
        public int EnabledDurationTime
        {
            get { return _enabledDurationTime; }
        }
        protected int _enabledDurationTime = 0;

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get => _description; set => _description = value; }
        protected string _description = string.Empty;
        #endregion

        #region Dirty
        public virtual void SetDirty()
        {
            if (null != _parent)
            {
                _parent.SetDirty();
            }

            if (!this._dirty)
            {
                this._dirty = true;
                OnDirtyChanged?.Invoke(this._dirty);
            }
        }

        /// <summary>
        /// 是否为脏
        /// </summary>
        public virtual bool IsDirty
        {
            get => _dirty;
        }
        protected bool _dirty = false;
        #endregion

        #region Pool
        public virtual void OnAllocate()
        {
            this._enabled = true;
            this._index = 0;
            this._parent = null;
            this._name = null;
            this._enabledDurationTime = 0;
            this._description = string.Empty;
        }

        public virtual void OnRecycle()
        {
            this.DetachChildren();
            this.SetParent(null);
        }

        [Newtonsoft.Json.JsonIgnore]
        protected IObjectPool pool;

        public virtual void Recycle()
        {
            if (null != pool)
            {
                pool.ApplyRecycle(this);
            }
        }

        public virtual void SetPool(IObjectPool pool)
        {
            this.pool = pool;
        }
        #endregion

        #region Cloneable
        public abstract object Clone();

        public virtual T CloneTo<T>(T obj) where T : NodeBase
        {
            obj._enabled = this._enabled;
            obj._name = this._name;

            obj._children.Clear();
            for (int i = 0; i < this._children.Count; ++i)
            {
                if (this._children[i].Clone() is NodeBase node)
                {
                    node.SetParent(obj);
                }
            }

#if UNITY_EDITOR
            this.EditorCloneTo(obj);
#endif

            return obj;
        }
        #endregion

        #region Relation
        protected virtual void AddChild(NodeBase node)
        {
            this._children.Add(node);

            this.SetDirty();
            this.ResetChildrenIndex();
        }

        protected virtual void InsertChild(int index, NodeBase node)
        {
            this._children.Insert(index, node);

            this.SetDirty();
            this.ResetChildrenIndex();
        }

        protected virtual void RemoveChild(NodeBase node)
        {
            this._children.Remove(node);

            this.SetDirty();
            this.ResetChildrenIndex();
        }

        public virtual void SetParent(NodeBase parent, int index = -1)
        {
            if (this._parent == parent)
            {
                return;
            }

            if (null != this._parent)
            {
                this._parent.RemoveChild(this);
            }

            this._parent = parent;

            if (null != this._parent)
            {
                if (string.IsNullOrEmpty(this._name))
                {
                    this.SetDefaultName();
                }

                if (index >= 0)
                {
                    this._parent.InsertChild(index, this);
                }
                else
                {
                    this._parent.AddChild(this);
                }
            }
        }

        /// <summary>
        /// 设置父节点后，如果名字为空，则设置默认名字
        /// 默认递归到int.MaxValue 如果int.MaxValue仍然找不到 则不设置名字了
        /// </summary>
        private void SetDefaultName()
        {
            for (int i = 0; i < int.MaxValue; ++i)
            {
                string name = string.Format("{0}_{1}", this.SelfTypeName, i);
                NodeBase node = _parent.Find(name);
                if (null == node) // 找不到 则说明名字可以用
                {
                    this._name = name;
                    break;
                }
            }
        }

        /// <summary>
        /// 当自己或者父节点更改了
        /// </summary>
        protected virtual void OnChanged()
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                NodeBase node = this._children[i];
                node.OnChanged();
            }
        }

        /// <summary>
        /// 将所有的子节点都脱离关系
        /// 并且回收所有的子节点
        /// </summary>
        public virtual void DetachChildren()
        {
            if (null != this._children && this._children.Count > 0)
            {
                for (int i = this._children.Count - 1; i >= 0; --i)
                {
                    NodeBase child = this._children[i];
                    if (null != child)
                    {
                        child.Recycle();
                    }
                }
                this._children.Clear();
            }
        }

        /// <summary>
        /// 重排整个子节点的索引号
        /// </summary>
        public virtual void ResetChildrenIndex()
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                _children[i].MaxIndex = this._children.Count;
                _children[i].Index = i;

                _children[i].ResetChildrenIndex();
            }

            NodeBase root = this.Root;
            if (null != root)
            {
                root.ResetGlobalIndex();
#if UNITY_EDITOR
                root.AutoSelect(0);
#endif
            }
        }

        private void ResetGlobalIndex()
        {
#if UNITY_EDITOR
            this.GlobalNodes.Clear();
#endif
            int globalIndex = 0;
            this.SetGlobalIndex(ref globalIndex);

            this.MaxGlobalIndex = globalIndex;
        }

        /// <summary>
        /// 获取自己同级的邻居队列
        /// </summary>
        /// <param name="list">队列的引用</param>
        /// <returns>true:成功</returns>
        protected bool GetNeighbours(ref List<NodeBase> list)
        {
            NodeBase parent = this._parent;
            if (null == parent)
                return false;

            List<NodeBase> children = parent.Children;

            if (null == list)
                list = new List<NodeBase>(children.Count - 1);

            for (int i = 0; i < children.Count; ++i)
            {
                if (children[i] != this)
                {
                    list.Add(children[i]);
                }
            }
            return true;
        }

        /// <summary>
        /// 通过递归获取对应的根节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected static NodeBase GetRoot(NodeBase node)
        {
            if (null != node.Parent)
            {
                return GetRoot(node.Parent);
            }

            return node;
        }

        public NodeBase GetChild(int index)
        {
            if (this._children.Count < 1 || index >= this._children.Count)
                return null;

            return this._children[index];
        }

        public NodeBase GetNeighbour(int indexOffset)
        {
            if (null != this.Parent)
            {
                if (indexOffset < 0)
                {
                    if (this.Index > 0)
                    {
                        return this._parent.GetChild(this.Index - 1);
                    }
                }
                else
                {
                    if (this.Index < this._parent.Children.Count - 1)
                    {
                        return this._parent.GetChild(this.Index + 1);
                    }
                }
            }
            return null;
        }

        private void SetGlobalIndex(ref int index)
        {
            this._globalIndex = index++;
            NodeBase root = this.Root;
            if (null != root)
            {
#if UNITY_EDITOR
                root.GlobalNodes.Add(this._globalIndex, this);
#endif
            }

            foreach (NodeBase child in _children)
            {
                child.SetGlobalIndex(ref index);
            }
        }

        /// <summary>
        /// 通过键来获取对应的子节点
        /// </summary>
        /// <param name="name">子节点名</param>
        /// <returns></returns>
        [Obsolete("FindChild has been deprecated. Use Find instead.")]
        public NodeBase FindChild(string name)
        {
            return Find(name);
        }

        /// <summary>
        /// 通过名字来寻找节点
        /// </summary>
        /// <param name="name">节点名</param>
        /// <returns>找不到的话则返回null</returns>
        public NodeBase Find(string name)
        {
            foreach (NodeBase child in _children)
            {
                if (child.Name == name)
                    return child;
            }
            return null;
        }



        /// <summary>
        /// 检查自自己开始往根节点的所有节点中，是否有类型是T的派生类或者为T的
        /// 如果有 则返回这个基点
        /// </summary>
        /// <typeparam name="T">NodeTypeClass</typeparam>
        /// <param name="node">从哪个节点开始往上找</param>
        /// <returns></returns>
        public static T GetNodeToRoot<T>(NodeBase node) where T : NodeBase
        {
            if (null == node)
                return null;

            Type nodeType = node.GetType();
            Type type = typeof(T);
            if (type == nodeType || node.GetType().IsSubclassOf(typeof(T)))
                return node as T;

            return GetNodeToRoot<T>(node.Parent);
        }

        /// <summary>
        /// 层级数，从根节点往下数看是到了第几层
        /// </summary>
        public int LayerCount
        {
            get => GetLayerCount(this, 0);
        }

        protected static int GetLayerCount(NodeBase node, int layerCount)
        {
            if (null == node || null == node.Parent)
                return layerCount;

            return GetLayerCount(node.Parent, layerCount + 1);
        }

        /// <summary>
        /// 打开所有的子节点
        /// </summary>
        public void OpenChildren()
        {
#if UNITY_EDITOR
            foreach (NodeBase child in _children)
            {
                child.IsEditing = true;
            }
#endif
        }

        /// <summary>
        /// 关闭所有的子节点
        /// </summary>
        public void CloseChildren()
        {
#if UNITY_EDITOR
            foreach (NodeBase child in _children)
            {
                child.IsEditing = false;
            }
#endif
        }
        #endregion

        #region Proto
        public virtual void Preprocess()
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                NodeBase node = _children[i];
                node.Preprocess();
            }
        }
        #endregion

        #region Logic
        /// <summary>
        /// 迭代
        /// </summary>
        /// <param name="delta"></param>
        public void Update(int delta)
        {
            _enabledDurationTime += delta;

            OnUpdate(delta);
        }

        /// <summary>
        /// 延迟到帧末时的迭代更新处理
        /// </summary>
        /// <param name="delta"></param>
        public void LateUpdate(int delta)
        {
            OnLateUpdate(delta);
        }

        /// <summary>
        /// 仅当激活时
        /// 才会进入迭代回调
        /// </summary>
        /// <param name="delta"></param>
        protected virtual void OnUpdate(int delta)
        {
            this.UpdateChildren(delta);
        }

        protected virtual void OnLateUpdate(int delta)
        {
            this.LateUpdateChildren(delta);
        }

        protected virtual void UpdateChildren(int delta)
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                NodeBase child = this._children[i];
                if (child.Enabled)
                {
                    child.Update(delta);
                }
            }
        }

        protected virtual void LateUpdateChildren(int delta)
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                NodeBase child = this._children[i];
                if (child.Enabled)
                {
                    child.LateUpdate(delta);
                }
            }
        }

        /// <summary>
        /// 判定子节点的名字是否可使用
        /// </summary>
        /// <param name="name">期望的名字</param>
        /// <returns>true:可使用|false:不可使用</returns>
        protected virtual bool JudgeChildNameCanUse(string name)
        {
            foreach (var child in this._children)
            {
                if (child.Name == name)
                    return false;
            }
            return true;
        }
        #endregion

        #region Comparable
        public virtual int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is NodeBase node)
            {
                return this._index.CompareTo(node._index);
            }

            throw new ArgumentException("Arg_MustBeNodeBase");
        }
        #endregion

        #region Exception
        protected void ThrowException(string message)
        {
            throw new NodeException(this, message);
        }


        #endregion

        #region Serialization
        protected NodeBase(SerializationInfo info, StreamingContext context)
        {
            this._enabled = (bool)info.GetValue("_enabled", typeof(bool));
            this._name = (string)info.GetValue("_name", typeof(string));
            this._description = (string)info.GetValue("_description", typeof(string));
            this._children = new List<NodeBase>();

            List<JToken> jArray = (List<JToken>)info.GetValue("_children", typeof(List<JToken>));

            foreach (JToken token in jArray)
            {
                string typeName = token.Value<string>("_typeName");
                if (!string.IsNullOrEmpty(typeName))
                {
                    if (System.Activator.CreateInstance(Type.GetType(typeName)) is NodeBase child)
                    {
                        child.SetParent(this);
                    }
                }
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_enabled", this._enabled);
            info.AddValue("_name", this._name);
            info.AddValue("_description", this._description);
            info.AddValue("_children", this._children);
            info.AddValue("_typeName", this.TypeName);
        }
        #endregion
    }
}
