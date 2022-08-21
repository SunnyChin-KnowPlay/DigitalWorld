using Dream.Core;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 用于逻辑检测用的要求
    /// 记录的是是否满足的情况
    /// </summary>
    public partial class Requirement : PooledObject
    {
        /// <summary>
        /// 节点名
        /// </summary>
        public string nodeName;

        /// <summary>
        /// 为真还是为假
        /// </summary>
        public bool isRequirement;
    }
}
