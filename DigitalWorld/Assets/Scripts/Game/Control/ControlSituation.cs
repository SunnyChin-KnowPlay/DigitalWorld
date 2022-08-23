namespace DigitalWorld.Game
{
    /// <summary>
    /// 战场态势控件
    /// </summary>
    public class ControlSituation : ControlLogic
    {
        /// <summary>
        /// 当前锁定目标
        /// </summary>
        public UnitHandle Target => target;
        protected UnitHandle target;


    }
}
