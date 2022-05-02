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
        public void Invoke(float delta)
        {
            this.OnInvoke(delta);
        }

        protected abstract void OnInvoke(float delta);
        #endregion
    }
}
