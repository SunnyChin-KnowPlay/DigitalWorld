namespace DigitalWorld.Logic
{
    public abstract partial class BaseCondition : BaseNode
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
        public virtual bool Check()
        {
            return true;
        }
        #endregion
    }
}
