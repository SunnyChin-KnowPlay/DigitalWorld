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
        /// CreateSummonEnergyMonster
        /// </summary> 
		public static Event CreateSummonEnergyMonster(UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = (int)EEvent.SummonEnergyMonster,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }

        #endregion
    }
}
