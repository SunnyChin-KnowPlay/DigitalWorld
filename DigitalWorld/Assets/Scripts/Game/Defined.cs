namespace DigitalWorld.Game
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
        Property,
        /// <summary>
        /// 动画
        /// </summary>
        Animator,
        /// <summary>
        /// 技能
        /// </summary>
        Skill,
        /// <summary>
        /// 移动
        /// </summary>
        Move,
        /// <summary>
        /// 行为
        /// </summary>
        Behaviour,
    }

    public enum EUnitStatus
    {
        /// <summary>
        /// 待机
        /// </summary>
        Idle = 0,
        /// <summary>
        /// 活跃中
        /// </summary>
        Running,
        /// <summary>
        /// 已死亡的
        /// </summary>
        Dead,
        /// <summary>
        /// 待回收 待释放的
        /// </summary>
        WaitRecycle,
    }

    public class Defined
    {
        /// <summary>
        /// 死亡存续周期
        /// </summary>
        public const float deadDuration = 5f;
    }

}
