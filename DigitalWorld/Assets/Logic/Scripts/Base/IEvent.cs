namespace DigitalWorld.Logic
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 触发者的单位ID
        /// </summary>
        public uint TriggerUnitUid { get; set; }
    }
}
