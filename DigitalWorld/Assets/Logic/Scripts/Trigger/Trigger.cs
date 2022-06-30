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

        public override T CloneTo<T>(T obj)
        {
            Trigger t = base.CloneTo(obj) as Trigger;
            if (null != t)
            {
                t.ListenEventId = this.ListenEventId;
                t.checkLogic = this.checkLogic;

                t.conditions.Clear();
                t.succeedActions.Clear();
                t.failedActions.Clear();
                t.enterActions.Clear();
                t.exitActions.Clear();

                for (int i = 0; i < this.conditions.Count; ++i)
                {
                    t.conditions.Add((BaseCondition)this.conditions[i].Clone());
                }

                for (int i = 0; i < this.succeedActions.Count; ++i)
                {
                    t.succeedActions.Add((BaseAction)this.succeedActions[i].Clone());
                }

                for (int i = 0; i < this.failedActions.Count; ++i)
                {
                    t.failedActions.Add((BaseAction)this.failedActions[i].Clone());
                }

                for (int i = 0; i < this.enterActions.Count; ++i)
                {
                    t.enterActions.Add((BaseAction)this.enterActions[i].Clone());
                }

                for (int i = 0; i < this.exitActions.Count; ++i)
                {
                    t.exitActions.Add((BaseAction)this.exitActions[i].Clone());
                }
            }
            return base.CloneTo(obj);
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
                this.State = EState.Running;
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

        protected override void OnEnter()
        {
            base.OnEnter();
            InvokeActions(this.enterActions);
        }

        protected override void OnExit()
        {
            base.OnExit();
            InvokeActions(this.exitActions);
        }
        #endregion
    }
}
