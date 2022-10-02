using DigitalWorld.Logic.Events;
using Dream.Core;
using System.Collections.Generic;
using System.Xml;

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
        /// 要求词典 所有的子节点都可以向其注入结果以供查询
        /// </summary>
        protected Dictionary<int, bool> requirements = new Dictionary<int, bool>();
        /// <summary>
        /// 监听的事件
        /// </summary>
        public EEvent ListenerEvent => listenerEvent;
        protected EEvent listenerEvent;

        /// <summary>
        /// 触发的事件
        /// </summary>
        public Events.EventHandler TriggeringEventHandler => triggeringEventHandler;
        protected Events.EventHandler triggeringEventHandler;
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

            this.requirements.Clear();
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
                trigger.listenerEvent = this.listenerEvent;
            }
            return obj;
        }
        #endregion

        #region Logic
        protected void SetRequirement(int index, bool v)
        {
            if (this.requirements.ContainsKey(index))
            {
                this.requirements[index] = v;
            }
            else
            {
                this.requirements.Add(index, v);
            }
        }

        public bool GetRequirement(int index)
        {
            this.requirements.TryGetValue(index, out bool ret);
            return ret;
        }

        public bool GetRequirement(string key)
        {
            NodeBase node = this.FindChild(key);
            if (null != node)
                return GetRequirement(node.Index);
            return false;
        }

        /// <summary>
        /// 遍历所有子节点时，先记录其requirement，以供后续查询
        /// </summary>
        /// <param name="delta"></param>
        protected override void UpdateChildren(int delta)
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                NodeBase child = this._children[i];
                if (child.Enabled)
                {
                    this.SetRequirement(child.Index, child.CheckRequirement());
                    child.Update(delta);
                }
                else
                {
                    this.SetRequirement(child.Index, true);
                }
            }
        }

        /// <summary>
        /// 唤醒
        /// </summary>
        /// <param name="ev"></param>
        public virtual void Invoke(Events.Event ev)
        {
            if (ev.EventId == this.ListenerEvent)
            {
                this.triggeringEventHandler = ObjectPool<EventHandler>.Instance.Allocate();
                this.triggeringEventHandler.Event = ev;

                this.State = EState.Running;
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
                this.State = EState.Ended;
                //for (int i = 0; i < this._children.Count; ++i)
                //{
                //    if (this._children[i] is NodeState child && child.Enabled && child.State == EState.Running)
                //    {
                //        child.State = EState.Ended;
                //    }
                //}
            }
        }
        #endregion

        #region Proto
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSizeEnum(listenerEvent);
        }

        protected override void OnEncode()
        {
            base.OnEncode();

            EncodeEnum(listenerEvent);
        }

        protected override void OnDecode()
        {
            base.OnDecode();

            DecodeEnum(ref listenerEvent);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.EncodeEnum(listenerEvent, "listenerEvent");
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.DecodeEnum(ref listenerEvent, "listenerEvent");
        }


        #endregion
    }
}
