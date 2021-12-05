namespace DigitalWorld.Logic
{
    public enum EMoveType
    {
        None = 0,
        Forward = 1 << 0,
        Right = 1 << 1,
        Back = 1 << 2,
        Left = 1 << 3,
    }


    /// <summary>
    /// 逻辑控制器类型
    /// </summary>
    public enum ELogicControlType
    {
        /// <summary>
        /// 属性
        /// </summary>
        Attribute,
        /// <summary>
        /// 动画
        /// </summary>
        Animator,
    }
}
