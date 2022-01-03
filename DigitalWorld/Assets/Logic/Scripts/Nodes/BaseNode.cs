using Dream.Core;
using Dream.Proto;
using System;
using System.Xml;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 逻辑根节点
    /// </summary>
    public abstract partial class BaseNode : DataBuffer
    {
        /// <summary>
        /// 节点唯一ID
        /// </summary>
        protected Guid uid;

        /// <summary>
        /// 节点唯一ID
        /// </summary>
        public Guid Uid { get { return uid; } }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        protected bool enabled = false;

        #region Pool
        public override void OnAllocate()
        {
            this.OnAllocate();
            this.uid = Guid.Empty;
            this.enabled = false;
        }

        public override void OnRecycle()
        {
            this.OnRecycle();
        }
        #endregion

        #region Proto
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            this.Encode(this.uid);
            this.Encode(this.enabled);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this.uid, "uid");
            this.Encode(this.enabled, "enabled");
        }
        #endregion
    }
}
