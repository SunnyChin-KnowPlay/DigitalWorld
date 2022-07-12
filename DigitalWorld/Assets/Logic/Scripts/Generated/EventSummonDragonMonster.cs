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
        /// CreateSummonDragonMonster
        /// </summary> 
		public static Event CreateSummonDragonMonster(UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = (int)EEvent.SummonDragonMonster,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }

        #endregion
    }
}
