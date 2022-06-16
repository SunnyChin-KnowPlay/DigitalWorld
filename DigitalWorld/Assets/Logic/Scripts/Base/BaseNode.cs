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
    public abstract partial class BaseNode : ByteBuffer
    {
        #region Params
        /// <summary>
        /// 节点唯一ID
        /// </summary>
        protected Guid uid;

        /// <summary>
        /// 节点唯一ID
        /// </summary>
        public Guid Uid { get { return uid; } }

        /// <summary>
        /// 索引号
        /// </summary>
        protected int index = 0;
        public int Index
        {
            get { return index; }
            set
            {
                if (index == value)
                    return;

                index = value;
                SetDirty();
            }
        }

        protected BaseNode parent;

        public BaseNode Parent => parent;

        protected List<BaseNode> children = new List<BaseNode>();
        /// <summary>
        /// 所有的子节点
        /// </summary>
        public List<BaseNode> Children => children;

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        private bool enabled = false;

        public virtual string Name
        {
            get
            {
                return this.GetType().ToString();
            }
        }

        public virtual string Key
        {
            get { return key; }
            set { key = value; }
        }
        protected string key;
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            this.OnAllocate();
            this.uid = Guid.Empty;
            this.enabled = false;
            this.index = 0;
            this.parent = null;
            this.key = null;

            if (null == this.children)
                this.children = new List<BaseNode>();
        }

        public override void OnRecycle()
        {
            this.DetachChildren();
            this.parent = null;

            this.OnRecycle();
        }

        public void NewUID()
        {
            this.uid = Guid.NewGuid();
        }
        #endregion

        #region Relation
        protected virtual void AddChild(BaseNode node)
        {
            this.children.Add(node);
        }

        protected virtual void RemoveChild(BaseNode node)
        {
            this.children.Remove(node);
        }

        public virtual void SetParent(BaseNode parent)
        {
            if (this.parent == parent)
            {
                return;
            }

            if (null != this.parent)
            {
                this.parent.RemoveChild(this);
            }

            this.parent = parent;

            if (null != this.parent)
            {
                this.parent.AddChild(this);
            }
        }

        public virtual void DetachChildren()
        {
            if (null != this.children && this.children.Count > 0)
            {
                for (int i = this.children.Count - 1; i >= 0; --i)
                {
                    BaseNode child = this.children[i];
                    if (null != child)
                    {
                        child.Recycle();
                    }
                }
                this.children.Clear();
            }
        }

        public BaseNode Find(Guid uid)
        {
            for (int i = 0; i < this.children.Count; ++i)
            {
                if (this.children[i].Uid == uid)
                    return this.children[i];
            }
            return null;
        }

        public virtual void ResetChildrenIndex()
        {

        }
        #endregion

        #region Proto
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            this.Encode(this.uid);
            this.Encode(this.enabled);
            this.Encode(this.key);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this.uid, "uid");
            this.Encode(this.enabled, "enabled");
            this.Encode(this.key, "key");
        }

        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            this.Decode(ref this.uid);
            this.Decode(ref this.enabled);
            this.Decode(ref this.key);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this.uid, "uid");
            this.Decode(ref this.enabled, "enabled");
            this.Decode(ref this.key, "key");
        }
        #endregion

        #region Logic

        #endregion
    }
}
