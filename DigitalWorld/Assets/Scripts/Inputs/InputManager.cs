using UnityEngine;
using DreamEngine.Core;
using System.Collections.Generic;

namespace DigitalWorld.Inputs
{
    /// <summary>
    /// 输入控制器
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        #region Params
        /// <summary>
        /// 事件和输入键位映射词典
        /// </summary>
        private Dictionary<EventCode, KeyCode> eventCodes = new Dictionary<EventCode, KeyCode>();
        #endregion
    }
}
