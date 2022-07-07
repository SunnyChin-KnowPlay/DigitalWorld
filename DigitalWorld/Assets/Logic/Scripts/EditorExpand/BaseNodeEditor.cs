namespace DigitalWorld.Logic
{
    public partial class NodeBase
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
