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
        private readonly Dictionary<EventCode, KeyCode> eventCodes = new Dictionary<EventCode, KeyCode>();
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            InitializeCodes();

            SetDefaultEventCodes();
        }

        private void InitializeCodes()
        {
            eventCodes.Clear();

            foreach (EventCode ec in System.Enum.GetValues(typeof(EventCode)))
            {
                eventCodes.Add(ec, KeyCode.None);
            }
        }
        #endregion

        #region Logic
        public KeyCode GetKeyCode(EventCode ec)
        {
            bool ret = eventCodes.TryGetValue(ec, out KeyCode code);
            if (!ret)
                return KeyCode.None;
            return code;
        }

        public void SetDefaultEventCodes()
        {
            eventCodes[EventCode.Escape] = KeyCode.Escape;
            eventCodes[EventCode.SwitchTargetAuto] = KeyCode.Tab;

            eventCodes[EventCode.MoveForward] = KeyCode.W;
            eventCodes[EventCode.MoveBackward] = KeyCode.S;
            eventCodes[EventCode.MoveLeft] = KeyCode.A;
            eventCodes[EventCode.MoveRight] = KeyCode.D;

            eventCodes[EventCode.ShortcutGroup1_0] = KeyCode.Alpha0;
            eventCodes[EventCode.ShortcutGroup1_1] = KeyCode.Alpha1;
            eventCodes[EventCode.ShortcutGroup1_2] = KeyCode.Alpha2;
            eventCodes[EventCode.ShortcutGroup1_3] = KeyCode.Alpha3;
            eventCodes[EventCode.ShortcutGroup1_4] = KeyCode.Alpha4;
            eventCodes[EventCode.ShortcutGroup1_5] = KeyCode.Alpha5;
            eventCodes[EventCode.ShortcutGroup1_6] = KeyCode.Alpha6;
            eventCodes[EventCode.ShortcutGroup1_7] = KeyCode.Alpha7;
            eventCodes[EventCode.ShortcutGroup1_8] = KeyCode.Alpha8;
            eventCodes[EventCode.ShortcutGroup1_9] = KeyCode.Alpha9;
        }
        #endregion
    }
}
