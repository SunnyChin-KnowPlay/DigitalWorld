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
        /// CreateGameInit
        /// </summary> 
		public static Event CreateGameInit(UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = (int)EEvent.GameInit,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }

        #endregion
    }
}
