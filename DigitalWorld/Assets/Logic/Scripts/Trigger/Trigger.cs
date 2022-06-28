using System.Collections.Generic;

namespace DigitalWorld.Logic
{
    public partial class Trigger : NodeFSE
    {
        #region Params
        /// <summary>
        /// 触发的事件
        /// </summary>
        protected Event triggeringEvent;

        public Event TriggeringEvent { get { return triggeringEvent; } }

        /// <summary>
        /// 监听的事件ID
        /// </summary>
        public int ListenEventId { get; set; }

        protected List<BaseCondition> conditions = new List<BaseCondition>();
        protected List<BaseAction> succeedActions = new List<BaseAction>();
        protected List<BaseAction> failedActions = new List<BaseAction>();
        protected List<BaseAction> enterActions = new List<BaseAction>();
        protected List<BaseAction> exitActions = new List<BaseAction>();
        /// <summary>
        /// 多重处理的触发器副本队列
        /// </summary>
        protected List<Trigger> multiples = new List<Trigger>();
        public List<Trigger> Multiples { get { return multiples; } }

        protected static List<BaseCondition> processConditions = new List<BaseCondition>();
        protected static List<BaseAction> processActions = new List<BaseAction>();

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

        }
        #endregion

        #region Relation
        public override void DetachChildren()
        {
            base.DetachChildren();

            this.conditions.Clear();
            this.succeedActions.Clear();
            this.failedActions.Clear();
            this.enterActions.Clear();
            this.exitActions.Clear();

            this.multiples.Clear();

        }

        protected override void AddChild(BaseNode node)
        {
            base.AddChild(node);
        }

        protected override void RemoveChild(BaseNode node)
        {
            base.RemoveChild(node);
        }
        #endregion

        #region Event
        public void DispatchEvent(Event ev)
        {
            if (this.State != EState.Idle)
                return;

            if (ev.Id == this.ListenEventId)
            {
                this.triggeringEvent = ev;
                this.Start();
            }
        }

        private bool Check()
        {
            bool ret = false;

            if (CheckLogic == ECheckLogic.And)
            {
                ret = true;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Enabled) continue;
                    ret = ret && conditions[i].Check();
                    if (!ret) break;
                }
            }
            else
            {
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Enabled) continue;
                    ret = ret || conditions[i].Check();
                    if (ret) break;
                }
            }

            return ret;
        }


        #endregion

        #region Logic
        protected virtual void InvokeActions(List<BaseAction> actions)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (!actions[i].Enabled) continue;
                actions[i].Invoke();
            }
        }

        protected override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);

            if (State == EState.Running)
            {
                bool ret = this.Check();
                if (ret)
                {
                    InvokeActions(this.succeedActions);
                }
                else
                {
                    InvokeActions(this.failedActions);
                }
            }

            if (this.State == EState.End)
            {
                this.OnExit();
                this.State = EState.Idle;
            }
        }

        protected override void OnStateChanged(EState laststate)
        {
            base.OnStateChanged(laststate);

            switch (this.State)
            {
                case EState.Running:
                {
                    this.OnEnter();
                    break;
                }
                case EState.End:
                {
                    this.OnExit();
                    break;
                }
            }
        }

        protected virtual void OnEnter()
        {
            InvokeActions(this.enterActions);
        }

        protected virtual void OnExit()
        {
            InvokeActions(this.exitActions);
        }
        #endregion
    }
}
