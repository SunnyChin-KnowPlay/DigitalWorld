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

    public enum ETileType
    {
        None = 0,
        /// <summary>
        /// 出生点
        /// </summary>
        Origin,
        /// <summary>
        /// 赌场
        /// </summary>
        Casino,
        /// <summary>
        /// 宝箱
        /// </summary>
        Chest,
        /// <summary>
        /// 门 就是使用钥匙的
        /// </summary>
        Door,
        /// <summary>
        /// 草地
        /// </summary>
        Grass,
        /// <summary>
        /// 魔晶
        /// </summary>
        MagicStone,
        /// <summary>
        /// 怪物
        /// </summary>
        Monster,
        /// <summary>
        /// 山
        /// </summary>
        Mountion,
        /// <summary>
        /// 商店
        /// </summary>
        Shop,
        /// <summary>
        /// 草地
        /// </summary>
        Spring,
        /// <summary>
        /// 阻挡
        /// </summary>
        Block,
        /// <summary>
        /// 旅行者
        /// </summary>
        Traveller,
    }
}
