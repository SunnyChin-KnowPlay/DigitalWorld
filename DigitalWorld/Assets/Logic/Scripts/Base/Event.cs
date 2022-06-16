using DigitalWorld.Game;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 事件
    /// </summary>
    public struct Event
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 触发者
        /// </summary>
        public UnitHandle Trigger { get; set; }
    }
}
