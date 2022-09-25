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
        private readonly static System.Type eventCodeType = typeof(EventCode);
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
        public static bool GetKey(EventCode key)
        {
            return Input.GetKey(Instance.GetKeyCode(key));
        }

        //
        // 摘要:
        //     Returns true while the user holds down the key identified by name.
        //
        // 参数:
        //   name:
        public static bool GetKey(string name)
        {
            return Input.GetKey(Instance.GetKeyString(name));
        }

        //
        // 摘要:
        //     Returns true during the frame the user releases the key identified by the key
        //     KeyCode enum parameter.
        //
        // 参数:
        //   key:
        public static bool GetKeyUp(EventCode key)
        {
            return Input.GetKeyUp(Instance.GetKeyCode(key));
        }

        //
        // 摘要:
        //     Returns true during the frame the user releases the key identified by name.
        //
        // 参数:
        //   name:
        public static bool GetKeyUp(string name)
        {
            return Input.GetKeyUp(Instance.GetKeyString(name));
        }

        //
        // 摘要:
        //     Returns true during the frame the user starts pressing down the key identified
        //     by the key KeyCode enum parameter.
        //
        // 参数:
        //   key:
        public static bool GetKeyDown(EventCode key)
        {
            return Input.GetKeyDown(Instance.GetKeyCode(key));
        }

        //
        // 摘要:
        //     Returns true during the frame the user starts pressing down the key identified
        //     by name.
        //
        // 参数:
        //   name:
        public static bool GetKeyDown(string name)
        {
            return Input.GetKeyDown(Instance.GetKeyString(name));
        }

        public KeyCode GetKeyString(string codeStr)
        {
            bool ret = System.Enum.TryParse<EventCode>(codeStr, out EventCode ec);
            if (!ret)
                return KeyCode.None;
            return GetKeyCode(ec);
        }

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
