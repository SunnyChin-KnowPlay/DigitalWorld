using Dream.Core;
using Dream.Proto;
using System;
using System.Collections.Generic;
using System.Xml;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 逻辑根节点
    /// </summary>
    public abstract partial class NodeBase : ByteBuffer
    {
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

        public NodeBase Parent => _parent;
        protected NodeBase _parent;

        /// <summary>
        /// 获取根节点，如果自己就是根节点则返回自己
        /// </summary>
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
        protected bool _enabled = false;

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

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        protected string _name;

        /// <summary>
        /// 已激活的持续时间
        /// </summary>
        public float EnabledDurationTime
        {
            get { return _enabledDurationTime; }
        }
        protected float _enabledDurationTime = 0;
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();
            this._enabled = false;
            this._index = 0;
            this._parent = null;
            this._name = null;
            this._enabledDurationTime = 0;
        }

        public override void OnRecycle()
        {
            this.DetachChildren();
            this.SetParent(null);

            base.OnRecycle();
        }

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

            return obj;
        }
        #endregion

        #region Relation
        protected virtual void AddChild(NodeBase node)
        {
            this._children.Add(node);
        }

        protected virtual void RemoveChild(NodeBase node)
        {
            this._children.Remove(node);
        }

        public virtual void SetParent(NodeBase parent)
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
                this._parent.AddChild(this);
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
            if (this._children.Count < 1 || this._children.Count >= index)
                throw new ArgumentOutOfRangeException();
            return this._children[index];
        }

        /// <summary>
        /// 通过键来获取对应的子节点
        /// </summary>
        /// <param name="name">子节点名</param>
        /// <returns></returns>
        public NodeBase FindChild(string name)
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                if (this._children[i]._name == name)
                    return this._children[i];
            }
            return null;
        }
        #endregion

        #region Proto
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            this.Encode(this._enabled);
            this.Encode(this._name);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            XmlDocument doc = element.OwnerDocument;

            this.Encode(this._enabled, "_enabled");
            this.Encode(this._name, "_name");
            this.Encode(this.TypeName, "_typeName");

            XmlElement childrenEle = doc.CreateElement("_children");
            for (int i = 0; i < _children.Count; ++i)
            {
                XmlElement childEle = doc.CreateElement("child");
                _children[i].Encode(childEle);
                childrenEle.AppendChild(childEle);
            }
            element.AppendChild(childrenEle);
        }

        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            this.Decode(ref this._enabled);
            this.Decode(ref this._name);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._enabled, "_enabled");
            this.Decode(ref this._name, "_name");

            _children.Clear();
            XmlElement childrenEle = element["_children"];
            if (null != childrenEle)
            {
                foreach (var node in childrenEle.ChildNodes)
                {
                    XmlElement childEle = node as XmlElement;
                    System.Type type = System.Type.GetType(childEle.GetAttribute("_typeName"));

                    if (System.Activator.CreateInstance(type) is NodeBase child)
                    {
                        child.Decode(childEle);
                        child.SetParent(this);
                    }
                }
            }
        }
        #endregion

        #region Logic
        /// <summary>
        /// 迭代
        /// </summary>
        /// <param name="delta"></param>
        public void Update(float delta)
        {
            //当未激活时 不执行任何逻辑
            if (!this.Enabled)
                return;

            _enabledDurationTime += delta;

            OnUpdate(delta);
        }

        /// <summary>
        /// 仅当激活时
        /// 才会进入迭代回调
        /// </summary>
        /// <param name="delta"></param>
        protected virtual void OnUpdate(float delta)
        {
            UpdateChildren(delta);
        }

        protected virtual void UpdateChildren(float delta)
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

        /// <summary>
        /// 检测要求条件
        /// </summary>
        /// <returns>true:可以执行</returns>
        public virtual bool CheckRequirement()
        {

            return true;
        }
        #endregion
    }
}
