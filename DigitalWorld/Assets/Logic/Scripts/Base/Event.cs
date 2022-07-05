﻿using DigitalWorld.Game;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 事件
    /// </summary>
    public partial struct Event
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public int Id
        {
            get; private set;
        }

        /// <summary>
        /// 触发者
        /// </summary>
        public UnitHandle Triggering { get; private set; }

        /// <summary>
        /// 目标
        /// </summary>
        public UnitHandle Target { get; private set; }

        public static Event Create(int id, UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = id,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }
    }
}
