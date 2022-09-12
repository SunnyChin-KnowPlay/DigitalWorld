namespace DigitalWorld.Logic
{
    public enum ECheckLogic : int
    {
        And = 0,
        Or = 1,
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
        GreaterThanOrEquipTo = 3,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan = 4,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEquipTo = 5,
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

}
