using Dream.Core;
using Dream.Proto;
using System;

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
    }
}
