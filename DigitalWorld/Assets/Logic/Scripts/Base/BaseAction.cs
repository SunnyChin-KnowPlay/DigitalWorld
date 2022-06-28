namespace DigitalWorld.Logic
{
    public abstract partial class BaseAction : BaseNode
    {
        #region Relation
        public Trigger Trigger
        {
            get
            {
                return this.Parent as Trigger;
            }
        }
        #endregion

        #region Logic
        public void Invoke()
        {
            this.OnInvoke();
        }

        protected abstract void OnInvoke();
        #endregion
    }
}
