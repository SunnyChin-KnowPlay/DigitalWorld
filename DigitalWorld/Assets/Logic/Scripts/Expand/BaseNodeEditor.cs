namespace DigitalWorld.Logic
{
    public partial class BaseNode
    {

        public virtual void SetDirty()
        {
            if (null != _parent)
            {
                _parent.SetDirty();
            }
        }
    }
}
