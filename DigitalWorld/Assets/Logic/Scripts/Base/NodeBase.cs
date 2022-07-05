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

        public NodeBase Parent { get => _parent; set => _parent = value; }
        protected NodeBase _parent;


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
        private bool _enabled = false;

        public virtual string Name
        {
            get
            {
                return this.GetType().ToString();
            }
        }

        public virtual string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        protected string _key;

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
            this.OnAllocate();
            this._enabled = false;
            this._index = 0;
            this._parent = null;
            this._key = null;
            this._enabledDurationTime = 0;
        }

        public override void OnRecycle()
        {
            this.DetachChildren();
            this.SetParent(null);

            this.OnRecycle();
        }

        public abstract object Clone();

        public virtual T CloneTo<T>(T obj) where T : NodeBase
        {
            obj._enabled = this._enabled;
            obj._key = this._key;


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

        public virtual void ResetChildrenIndex()
        {

        }
        #endregion

        #region Proto
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            this.Encode(this._enabled);
            this.Encode(this._key);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._enabled, "enabled");
            this.Encode(this._key, "key");
        }

        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            this.Decode(ref this._enabled);
            this.Decode(ref this._key);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._enabled, "enabled");
            this.Decode(ref this._key, "key");

            _children.Clear();
            XmlElement childrenEle = element["children"];
            if (null != childrenEle)
            {
                foreach (var node in childrenEle.ChildNodes)
                {
                    XmlElement childEle = node as XmlElement;
                    System.Type type = System.Type.GetType(childEle.GetAttribute("class"));

                    NodeBase child = System.Activator.CreateInstance(type) as NodeBase;
                    if (null != child)
                    {
                        child.Parent = this;
                        child.Decode(childEle);
                        _children.Add(child);
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

        protected void UpdateChildren(float delta)
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
        #endregion
    }
}
