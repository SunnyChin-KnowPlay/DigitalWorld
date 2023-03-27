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
        /// 触发器
        /// </summary>
        Trigger,
        /// <summary>
        /// 战场态势
        /// </summary>
        Situation,
        /// <summary>
        /// 计算
        /// </summary>
        Calculate,
        /// <summary>
        /// 测试控制器
        /// </summary>
        Test,
        /// <summary>
        /// 事件控制器
        /// </summary>
        Event,
    }

    /// <summary>
    /// 单位状态
    /// </summary>
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

    /// <summary>
    /// 伤害类型
    /// </summary>
    public enum EDamagerType
    {
        /// <summary>
        /// 物理伤害
        /// </summary>
        Physics = 0,
        /// <summary>
        /// 魔法伤害
        /// </summary>
        Magic,
        /// <summary>
        /// 真实伤害
        /// </summary>
        Truth,
        /// <summary>
        /// 反向伤害 - 治疗
        /// </summary>
        Reverse,
    }

    /// <summary>
    /// 伤害目标类型枚举
    /// </summary>
    public enum EDamageTargetType
    {
        /// <summary>
        /// 无目标的
        /// </summary>
        None = 0,
        /// <summary>
        /// 主目标
        /// </summary>
        Main = 1 << 0,
        /// <summary>
        /// AOE的附带目标
        /// </summary>
        Incidental = 1 << 1,
        /// <summary>
        /// 主目标和附带目标
        /// </summary>
        Total = Main | Incidental,
    }

    /// <summary>
    /// 单元功能锁
    /// 一旦上锁后，该功能即失效
    /// </summary>
    public enum EUnitFunction : int
    {
        /// <summary>
        /// 移动
        /// </summary>
        Move = 0,
        /// <summary>
        /// 可攻击的
        /// </summary>
        Vincible,
        /// <summary>
        /// 旋转
        /// </summary>
        Rotate,
        /// <summary>
        /// 被约束
        /// </summary>
        Constrained,
        Max,
    }

    /// <summary>
    /// 单位类型
    /// </summary>
    public enum EUnitType
    {
        /// <summary>
        /// 角色
        /// </summary>
        Character = 1,
        /// <summary>
        /// 建筑
        /// </summary>
        Building = 2,
    }

    public class Defined
    {
        /// <summary>
        /// 死亡存续周期
        /// </summary>
        public const float deadDuration = 5f;
    }

}
