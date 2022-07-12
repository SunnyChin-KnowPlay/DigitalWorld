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
        /// CreateGameEnd
        /// </summary> 
		public static Event CreateGameEnd(UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = (int)EEvent.GameEnd,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }

        #endregion
    }
}
