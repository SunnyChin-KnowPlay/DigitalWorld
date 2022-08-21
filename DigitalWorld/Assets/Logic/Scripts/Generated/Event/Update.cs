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
        /// 迭代事件
        /// </summary> 
		public static Event CreateUpdate(UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = (int)EEvent.Update,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }

        #endregion
    }
}
