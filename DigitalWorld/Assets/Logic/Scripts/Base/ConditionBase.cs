namespace DigitalWorld.Logic
{
    public abstract partial class ConditionBase : NodeBase
    {
        #region Logic
        public virtual bool Check(Event ev)
        {
            return true;
        }
        #endregion
    }
}
