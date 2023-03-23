using DigitalWorld.Game;
using Dream.Core;
using System;
using System.Collections.Generic;

namespace DigitalWorld.Logic.Events
{
    /// <summary>
    /// 事件操作者
    /// </summary>
    [Serializable]
    public partial class EventHandler : PooledObject
    {
        #region Params
        /// <summary>
        /// 事件
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// 触发者
        /// </summary>
        public UnitHandle Triggering { get => Event.Triggering; }

        /// <summary>
        /// 主目标
        /// </summary>
        public UnitHandle MainTarget { get => Event.Target; }

        /// <summary>
        /// 目标队列
        /// </summary>
        public List<UnitHandle> Targets => targets;
        protected readonly List<UnitHandle> targets = new List<UnitHandle>();
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();

            this.Event = default;
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            this.targets.Clear();
        }
        #endregion
    }
}
