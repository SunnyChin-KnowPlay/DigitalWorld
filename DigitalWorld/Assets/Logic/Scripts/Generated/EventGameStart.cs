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
        /// CreateGameStart
        /// </summary> 
		public static Event CreateGameStart(UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = (int)EEvent.GameStart,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }

        #endregion
    }
}
