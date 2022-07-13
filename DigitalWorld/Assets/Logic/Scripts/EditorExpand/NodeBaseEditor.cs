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


#if UNITY_EDITOR
        #region Params
        /// <summary>
        /// 是否正在编辑中
        /// </summary>
        public bool IsEditing => _isEditing;
        protected bool _isEditing = true;

        /// <summary>
        /// 描述信息
        /// </summary>
        public string _description = string.Empty;
        #endregion

       
        #region GUI
        public virtual void OnGUI()
        {

        }
        #endregion
#endif
    }
}
