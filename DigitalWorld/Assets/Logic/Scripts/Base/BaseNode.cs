﻿using Dream.Core;
using Dream.Proto;
using System;
using System.Collections.Generic;
using System.Xml;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 逻辑根节点
    /// </summary>
    public abstract partial class BaseNode : ByteBuffer
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

        public BaseNode Parent => _parent;
        protected BaseNode _parent;

       
        /// <summary>
        /// 所有的子节点
        /// </summary>
        public List<BaseNode> Children => _children;
        protected List<BaseNode> _children = new List<BaseNode>();

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
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            this.OnAllocate();
            this._enabled = false;
            this._index = 0;
            this._parent = null;
            this._key = null;
        }

        public override void OnRecycle()
        {
            this.DetachChildren();
            this.SetParent(null);

            this.OnRecycle();
        }

        #endregion

        #region Relation
        protected virtual void AddChild(BaseNode node)
        {
            this._children.Add(node);
        }

        protected virtual void RemoveChild(BaseNode node)
        {
            this._children.Remove(node);
        }

        public virtual void SetParent(BaseNode parent)
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
                    BaseNode child = this._children[i];
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
        }
        #endregion

        #region Logic

        #endregion
    }
}
