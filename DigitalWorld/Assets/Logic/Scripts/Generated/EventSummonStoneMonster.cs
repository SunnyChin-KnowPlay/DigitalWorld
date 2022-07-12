using DigitalWorld.Game;
using UnityEngine;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 事件
    /// </summary>
    public partial struct Event
    {
        #region Construction

		/// <summary>
        /// CreateSummonStoneMonster
        /// </summary> 
		public static Event CreateSummonStoneMonster(UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = (int)EEvent.SummonStoneMonster,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }

        #endregion
    }
}
