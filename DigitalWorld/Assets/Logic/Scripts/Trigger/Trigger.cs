using System.Collections.Generic;

namespace DigitalWorld.Logic
{
    public partial class Trigger : NodeFSE
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
        public int ListenEventId { get; set; }

        private readonly List<BaseAction> actions = new List<BaseAction>();
        private readonly List<BaseCondition> conditions = new List<BaseCondition>();

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
        protected void Invoke(float delta)
        {
            for (int i = 0; i < this.actions.Count; ++i)
            {
                BaseAction ba = this.actions[i];
                if (null != ba)
                {
                    ba.Invoke(delta);
                }
            }
        }

        protected override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);

            switch (this.State)
            {
                case EState.Running:
                {
                    bool ret = this.Check();
                    if (ret)
                    {
                        Invoke(delta);
                    }
                    else
                    {
                        this.State = EState.End;
                    }
                    break;
                }
            }
        }

        protected override void OnStateChanged(EState laststate)
        {
            base.OnStateChanged(laststate);

            switch (this.State)
            {
                case EState.Running:
                {
                    this.Enter();
                    break;
                }
                case EState.End:
                {
                    this.Exit();
                    break;
                }
            }
        }

        protected void Enter()
        {

        }

        protected void Exit()
        {

        }
        #endregion
    }
}
