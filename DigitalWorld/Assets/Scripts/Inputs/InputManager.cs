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
        public Dictionary<EventCode, KeyCode> EventCodes => eventCodes;
        private readonly Dictionary<EventCode, KeyCode> eventCodes = new Dictionary<EventCode, KeyCode>();

        private readonly static System.Type keyCodeType = typeof(KeyCode);
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            InitializeCodes();

            SetDefaultEventCodes();
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(keyCodeType))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        Debug.LogFormat($"InputManager:\"{keyCode}\" is Down");
                    }
                }
            }
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

        #region Key
        public static bool GetKey(EventCode key)
        {
            return Input.GetKey(Instance.GetKeyCode(key));
        }

        public static bool GetKey(string name)
        {
            return Input.GetKey(Instance.GetKeyString(name));
        }

        public static bool GetKeyUp(EventCode key)
        {
            return Input.GetKeyUp(Instance.GetKeyCode(key));
        }

        public static bool GetKeyUp(string name)
        {
            return Input.GetKeyUp(Instance.GetKeyString(name));
        }

        public static bool GetKeyDown(EventCode key)
        {
            return Input.GetKeyDown(Instance.GetKeyCode(key));
        }

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

        public void SetKeyCode(EventCode ec, KeyCode kc)
        {
            if (this.eventCodes.ContainsKey(ec))
                this.eventCodes[ec] = kc;
            else
                this.eventCodes.Add(ec, kc);
        }
        #endregion

        #region Logic

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

        public string GetEventCodeText(EventCode ec)
        {
            return ec switch
            {
                EventCode.Escape => "Esc",
                EventCode.SwitchTargetAuto => "自动切换目标",
                EventCode.SwitchTargetNext => "切换下一个目标",
                EventCode.SwitchTargetPrev => "切换上一个目标",
                EventCode.MoveForward => "前进",
                EventCode.MoveBackward => "后退",
                EventCode.MoveLeft => "左移",
                EventCode.MoveRight => "右移",
                EventCode.ShortcutGroup1_0 => "快捷键1",
                EventCode.ShortcutGroup1_1 => "快捷键2",
                EventCode.ShortcutGroup1_2 => "快捷键3",
                EventCode.ShortcutGroup1_3 => "快捷键4",
                EventCode.ShortcutGroup1_4 => "快捷键5",
                EventCode.ShortcutGroup1_5 => "快捷键6",
                EventCode.ShortcutGroup1_6 => "快捷键7",
                EventCode.ShortcutGroup1_7 => "快捷键8",
                EventCode.ShortcutGroup1_8 => "快捷键9",
                EventCode.ShortcutGroup1_9 => "快捷键10",
                EventCode.ShortcutGroup2_0 => "扩展快捷键1组1",
                EventCode.ShortcutGroup2_1 => "扩展快捷键1组2",
                EventCode.ShortcutGroup2_2 => "扩展快捷键1组3",
                EventCode.ShortcutGroup2_3 => "扩展快捷键1组4",
                EventCode.ShortcutGroup2_4 => "扩展快捷键1组5",
                EventCode.ShortcutGroup2_5 => "扩展快捷键1组6",
                EventCode.ShortcutGroup2_6 => "扩展快捷键1组7",
                EventCode.ShortcutGroup2_7 => "扩展快捷键1组8",
                EventCode.ShortcutGroup2_8 => "扩展快捷键1组9",
                EventCode.ShortcutGroup2_9 => "扩展快捷键1组10",
                _ => "未定义",
            };
            //switch (ec)
            //{
            //    case EventCode.Escape:
            //    {
            //        return "Esc";
            //    }
            //}

        }
        #endregion
    }
}
