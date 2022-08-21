/**
 * 该文件通过代码生成器生成
 * 请不要修改这些代码
 * 当然，修改了也没什么用，如果你有兴趣你可以试试。
 */
using DigitalWorld.Game;
using UnityEngine;

namespace DigitalWorld.Logic.Events
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
