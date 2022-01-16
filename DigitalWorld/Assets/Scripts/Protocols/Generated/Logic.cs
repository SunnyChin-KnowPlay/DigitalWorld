using Dream.Core;
using Dream.Proto;
using System.Collections.Generic;
using System.Xml;

namespace DigitalWorld.Proto.Logic
{
        	/// <summary>
    /// 指令类型
    /// </summary>
    public enum EEventType : int
    {
  
        /// <summary>
        /// 游戏开始
        /// </summary>
        GameStart = 1,
  
        /// <summary>
        /// 玩家出生
        /// </summary>
        PlayerBorn = 2,
  
        /// <summary>
        /// 单位出生
        /// </summary>
        UnitBorn = 3,
  
        /// <summary>
        /// 接触前
        /// </summary>
        TouchBefore = 101,
  
        /// <summary>
        /// 接触
        /// </summary>
        Touching = 102,
  
        /// <summary>
        /// 接触后
        /// </summary>
        TouchAfter = 103,
    }
}