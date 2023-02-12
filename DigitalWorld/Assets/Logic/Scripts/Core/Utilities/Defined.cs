namespace DigitalWorld.Logic
{
    public enum ECheckLogic : int
    {
        And = 0,
        Or = 1,
    }

    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum EState
    {
        /// <summary>
        /// 待机中
        /// </summary>
        Idle = 1 << 0,
        /// <summary>
        /// 运行中
        /// </summary>
        Running = 1 << 1,
        /// <summary>
        /// 已结束 - 成功
        /// </summary>
        Succeeded = 1 << 2,
        /// <summary>
        /// 已结束 - 失败
        /// </summary>
        Failed = 1 << 3,
    }

    /// <summary>
    /// 定位方式
    /// </summary>
    public enum ELocationMode
    {
        /// <summary>
        /// 名字的方式来定位
        /// </summary>
        Name,
        /// <summary>
        /// 上一个
        /// </summary>
        Previous,
        /// <summary>
        /// 下一个
        /// </summary>
        Next,
    }

    public enum ECheckOperator : int
    {
        /// <summary>
        /// 相等
        /// </summary>
        Equal = 0,
        /// <summary>
        /// 不等
        /// </summary>
        NotEqual = 1,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan = 2,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqualTo = 3,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan = 4,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqualTo = 5,
    }

    public enum ENodeType : int
    {
        /// <summary>
        /// 空 没有的
        /// </summary>
        None = 0,
        /// <summary>
        /// 属性
        /// </summary>
        Property = 1 << 0,
        /// <summary>
        /// 行动
        /// </summary>
        Action = 1 << 1,
        /// <summary>
        /// 触发器
        /// </summary>
        Trigger = 1 << 2,
        /// <summary>
        /// 关卡
        /// </summary>
        Level = 1 << 3,
    }

    /// <summary>
    /// 运行模式
    /// </summary>
    public enum EMotionMode
    {
        /// <summary>
        /// 单次
        /// </summary>
        Once = 0,
        /// <summary>
        /// 重复发生，指到达结束时，下一帧再次回到Running再次运行。
        /// </summary>
        Repeat = 1,
        /// <summary>
        /// 循环，指到达结束时，下一帧回到Idle。
        /// </summary>
        Loop = 2,
        /// <summary>
        /// 副本运行，自身状态不变化，生成副本执行。
        /// </summary>
        Duplicate = 4,
    }

    /// <summary>
    /// 类型代码
    /// </summary>
    public enum ETypeCode
    {
        Empty = 0,
        Enum,
        Boolean,
        Char,
        SByte,
        Byte,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        Single,
        Double,
        Decimal,
        DateTime,
        String,
        FixVector2,
        FixVector3,
        FixFactor,
    }

    /// <summary>
    /// 计量单位
    /// 绝对值还是千分比
    /// </summary>
    public enum EMeasureUnit
    {
        /// <summary>
        /// 绝对值
        /// </summary>
        Absolute,
        /// <summary>
        /// 千分比
        /// </summary>
        Permillage
    }

}
