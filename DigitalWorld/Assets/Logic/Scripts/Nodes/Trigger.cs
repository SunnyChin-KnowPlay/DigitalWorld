using System.Collections.Generic;

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
        #endregion

        #region Pooled
        public override void OnAllocate()
        {
            base.OnAllocate();

            ListenEventId = 0;
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
    }
}
