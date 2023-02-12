using Dream.Core;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using DigitalWorld.Logic.Events;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 触发器类
    /// </summary>
    public partial class Trigger : NodeState
    {
        #region Params
        public override int Id
        {
            get => 0;
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public override ENodeType NodeType => ENodeType.Trigger;

        /// <summary>
        /// 监听的事件
        /// </summary>
        public EEvent ListenerEvent => _listenerEvent;
        protected EEvent _listenerEvent;

        /// <summary>
        /// 触发的事件
        /// </summary>
        public Events.EventHandler TriggeringEventHandler => triggeringEventHandler;
        protected Events.EventHandler triggeringEventHandler;

        /// <summary>
        /// 副本节点队列
        /// 如果MotionMode为Multiple 则将节点启动并加入队列。
        /// </summary>
        protected readonly List<NodeState> _duplicates = new List<NodeState>();
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            if (null != triggeringEventHandler)
            {
                triggeringEventHandler.Recycle();
                triggeringEventHandler = null;
            }

            foreach (NodeState node in _duplicates)
            {
                node.Recycle();
            }
            _duplicates.Clear();
        }

        public override object Clone()
        {
            Trigger trigger = ObjectPool<Trigger>.Instance.Allocate();
            this.CloneTo(trigger);
            return trigger;
        }

        public override T CloneTo<T>(T obj)
        {
            if (base.CloneTo(obj) is Trigger trigger)
            {
                trigger._listenerEvent = this._listenerEvent;
            }
            return obj;
        }
        #endregion

        #region Logic
        protected override void OnUpdate(int delta)
        {
            for (int i = 0; i < this._duplicates.Count; ++i)
            {
                NodeState duplicate = _duplicates[i];
                if (null != duplicate)
                {
                    duplicate.Update(delta);

                    // 如果副本执行结束了 则回收
                    if (duplicate.IsEnded)
                    {
                        this._duplicates.RemoveAt(--i);
                        duplicate.Recycle();
                    }
                }
            }

            base.OnUpdate(delta);
        }

        /// <summary>
        /// 唤醒
        /// 如果MotionMode为Duplicate的话，则直接生成副本运行。
        /// </summary>
        /// <param name="ev"></param>
        public virtual void Invoke(Events.Event ev)
        {
            if (this.CheckRequirement())
            {
                if (ev.EventId == this.ListenerEvent)
                {
                    if (this.MotionMode == EMotionMode.Duplicate)
                    {
                        if (this.Clone() is Trigger obj)
                        {
                            obj.triggeringEventHandler = ObjectPool<EventHandler>.Instance.Allocate();
                            obj.triggeringEventHandler.Event = ev;

                            obj.State = EState.Running;
                            this._duplicates.Add(obj);
                        }
                    }
                    else
                    {
                        if (this.State == EState.Idle)
                        {
                            this.triggeringEventHandler = ObjectPool<EventHandler>.Instance.Allocate();
                            this.triggeringEventHandler.Event = ev;

                            this.State = EState.Running;
                        }
                    }
                }

            }

        }

        protected override void OnExit()
        {
            base.OnExit();

            if (null != triggeringEventHandler)
            {
                triggeringEventHandler.Recycle();
                triggeringEventHandler = null;
            }

            foreach (NodeState node in _duplicates)
            {
                node.Recycle();
            }
            _duplicates.Clear();
        }

        /// <summary>
        /// 打断触发器
        /// 如果触发器正在运行中的话
        /// 则让其停止 并且遍历所有的子节点行动，如果有运行中的一并停止
        /// </summary>
        public virtual void Break()
        {
            if (this.State == EState.Running)
            {
                this.State = EState.Failed;
            }
        }

        public override int TotalTime
        {
            get
            {
                int time = 0;
                foreach (NodeState nodeState in _children.Cast<NodeState>())
                {
                    if (null != nodeState)
                    {
                        time += nodeState.TotalTime;
                    }
                }
                return time;
            }
        }
        #endregion

        #region Proto
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSizeEnum(_listenerEvent);
        }

        protected override void OnEncode()
        {
            base.OnEncode();

            EncodeEnum(_listenerEvent);
        }

        protected override void OnDecode()
        {
            base.OnDecode();

            DecodeEnum(ref _listenerEvent);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.EncodeEnum(_listenerEvent, "_listenerEvent");
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.DecodeEnum(ref _listenerEvent, "_listenerEvent");
        }


        #endregion
    }
}
