﻿using System.Collections.Generic;

namespace DigitalWorld.Logic
{
    public partial class Trigger : BaseNode
    {
        #region Params
        /// <summary>
        /// 触发的事件
        /// </summary>
        protected IEvent triggeringEvent;

        public IEvent TriggeringEvent { get { return triggeringEvent; } }

        /// <summary>
        /// 监听的事件ID
        /// </summary>
        public uint ListenEventId { get; set; }

        private readonly List<BaseAction> actions = new List<BaseAction>();
        private readonly List<BaseCondition> conditions = new List<BaseCondition>();

        private int runningTime = 0;
        /// <summary>
        /// 运行的时间
        /// </summary>
        public int RunningTime
        {
            get { return runningTime; }
        }

        private int lastedTriggeredTime = 0;
        /// <summary>
        /// 最后一次触发的时间
        /// </summary>
        public int LastedTriggeredTime
        {
            get { return lastedTriggeredTime; }
            protected set { lastedTriggeredTime = value; }
        }

        private ECheckLogic checkLogic;
        public ECheckLogic CheckLogic
        {
            get
            {
                return checkLogic;
            }
            set
            {
                if (checkLogic != value)
                {
                    SetDirty();
                    checkLogic = value;
                }
            }
        }
        #endregion

        #region Pooled
        public override void OnAllocate()
        {
            base.OnAllocate();

            this.ListenEventId = 0;
            this.runningTime = 0;
            this.lastedTriggeredTime = 0;
        }
        #endregion

        #region Relation
        public override void DetachChildren()
        {
            base.DetachChildren();

            this.actions.Clear();
            this.conditions.Clear();
        }

        protected override void AddChild(BaseNode node)
        {
            base.AddChild(node);

            if (node is BaseAction ba)
                this.actions.Add(ba);

            if (node is BaseCondition bc)
                this.conditions.Add(bc);
        }

        protected override void RemoveChild(BaseNode node)
        {
            base.RemoveChild(node);

            if (node is BaseAction ba)
                this.actions.Remove(ba);

            if (node is BaseCondition bc)
                this.conditions.Remove(bc);
        }
        #endregion

        #region Event
        public void DispatchEvent(IEvent ev)
        {
            if (ev.Id == this.ListenEventId)
            {
                this.Process(ev);
            }
        }

        private void Process(IEvent ev)
        {
            this.triggeringEvent = ev;

            bool conf = false;
            ConstructTriggerUnit(ev);
            if (CheckLogic == ECheckLogic.And)
            {
                conf = true;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Enabled) continue;
                    conf = conf && conditions[i].Check();
                    if (!conf) break;
                }
            }
            else
            {
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Enabled) continue;
                    conf = conf || conditions[i].Check();
                    if (conf) break;
                }
            }

            if (conf)
            {
                Invoke();
            }
        }
        #endregion

        #region Logic
        public virtual void OnUpdate(int delta)
        {
            if (this.enabled)
            {
                this.runningTime += delta;
            }
        }

        private void ConstructTriggerUnit(IEvent ev)
        {

        }

        private void Invoke()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (!actions[i].Enabled) continue;
                actions[i].Invoke();
            }
        }
        #endregion
    }
}
