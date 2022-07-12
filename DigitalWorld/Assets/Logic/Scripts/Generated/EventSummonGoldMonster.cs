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
        /// CreateSummonGoldMonster
        /// </summary> 
		public static Event CreateSummonGoldMonster(UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = (int)EEvent.SummonGoldMonster,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }

        #endregion
    }
}
