

namespace DigitalWorld.Game
{
    /// <summary>
    /// 受伤参数结构
    /// </summary>
    public struct ParamInjury
    {
        /// <summary>
        /// 来源
        /// </summary>
        public UnitHandle source;
        /// <summary>
        /// 目标
        /// </summary>
        public UnitHandle target;

        /// <summary>
        /// 技能索引号
        /// </summary>
        public int skillIndex;

        /// <summary>
        /// 技能配置ID
        /// </summary>
        public int skillId;
    }

}
