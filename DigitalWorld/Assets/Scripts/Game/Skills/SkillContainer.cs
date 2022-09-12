using Dream.Core;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 技能容器
    /// </summary>
    public class SkillContainer : PooledObject
    {
        #region Params
        /// <summary>
        /// 槽位号
        /// </summary>
        public int SlotIndex { get => slotIndex; protected set => slotIndex = value; }
        protected int slotIndex = 0;
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();
            slotIndex = 0;
        }
        #endregion
    }
}
