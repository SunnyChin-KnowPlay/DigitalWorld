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
        #endregion

        #region Pooled
        public override void OnAllocate()
        {
            base.OnAllocate();

            ListenEventId = 0;
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


        }
        #endregion
    }
}
