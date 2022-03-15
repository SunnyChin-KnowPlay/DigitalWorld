namespace DigitalWorld.Logic
{
    public partial class BaseNode
    {
#if UNITY_EDITOR

        public virtual void SetDirty()
        {
            if (null != parent)
            {
                parent.SetDirty();
            }
        }
#endif
    }
}
