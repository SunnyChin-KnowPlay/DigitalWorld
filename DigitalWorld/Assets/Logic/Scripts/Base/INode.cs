namespace DigitalWorld.Logic
{
    public interface INode
    {
        /// <summary>
        /// ID
        /// </summary>
        int Id { get; }

        void Update(float delta);

        /// <summary>
        /// 是否激活的
        /// </summary>
        bool Enabled { get; set; }

#if UNITY_EDITOR
        #region GUI
        void OnGUIDetails();
        #endregion
#endif
    }
}
