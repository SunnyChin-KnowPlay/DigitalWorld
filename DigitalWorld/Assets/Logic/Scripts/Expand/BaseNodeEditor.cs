namespace DigitalWorld.Logic
{
    public partial class BaseNode
    {

        public virtual void SetDirty()
        {
            if (null != parent)
            {
                parent.SetDirty();
            }
        }
    }
}
