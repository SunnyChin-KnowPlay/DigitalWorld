namespace DigitalWorld.Logic
{
    public partial class Trigger
    {
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
