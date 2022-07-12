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
        /// CreateCharacterActivate
        /// </summary> 
		public static Event CreateCharacterActivate(UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = (int)EEvent.CharacterActivate,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }

        #endregion
    }
}
