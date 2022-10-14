using UnityEngine;
using DreamEngine.Core;
using System.Collections.Generic;
using System.Xml;

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

            DeserializeKeyCodes();
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
            {
                if (this.eventCodes[ec] != kc)
                {
                    this.eventCodes[ec] = kc;
                    this.SerializeKeyCodes();
                }
            }
            else
            {
                this.eventCodes.Add(ec, kc);
                this.SerializeKeyCodes();
            }
        }
        #endregion

        #region Logic
        private KeyCode GetDefaultKeyCode(EventCode ec)
        {
            return ec switch
            {
                EventCode.Escape => KeyCode.Escape,
                EventCode.SwitchTargetAuto => KeyCode.Tab,
                EventCode.SwitchTargetNext => KeyCode.None,
                EventCode.SwitchTargetPrev => KeyCode.None,
                EventCode.MoveForward => KeyCode.W,
                EventCode.MoveBackward => KeyCode.S,
                EventCode.MoveLeft => KeyCode.A,
                EventCode.MoveRight => KeyCode.D,
                EventCode.ShortcutGroup1_0 => KeyCode.Alpha1,
                EventCode.ShortcutGroup1_1 => KeyCode.Alpha2,
                EventCode.ShortcutGroup1_2 => KeyCode.Alpha3,
                EventCode.ShortcutGroup1_3 => KeyCode.Alpha4,
                EventCode.ShortcutGroup1_4 => KeyCode.Alpha5,
                EventCode.ShortcutGroup1_5 => KeyCode.Alpha6,
                EventCode.ShortcutGroup1_6 => KeyCode.Alpha7,
                EventCode.ShortcutGroup1_7 => KeyCode.Alpha8,
                EventCode.ShortcutGroup1_8 => KeyCode.Alpha9,
                EventCode.ShortcutGroup1_9 => KeyCode.Alpha0,

                _ => KeyCode.None,
            };
        }

        public void SetDefaultEventCodes()
        {
            foreach (EventCode ec in System.Enum.GetValues(typeof(EventCode)))
            {
                eventCodes[ec] = GetDefaultKeyCode(ec);
            }
            SerializeKeyCodes();
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
                EventCode.ShortcutGroup1_0 => "快捷键1组1",
                EventCode.ShortcutGroup1_1 => "快捷键1组2",
                EventCode.ShortcutGroup1_2 => "快捷键1组3",
                EventCode.ShortcutGroup1_3 => "快捷键1组4",
                EventCode.ShortcutGroup1_4 => "快捷键1组5",
                EventCode.ShortcutGroup1_5 => "快捷键1组6",
                EventCode.ShortcutGroup1_6 => "快捷键1组7",
                EventCode.ShortcutGroup1_7 => "快捷键1组8",
                EventCode.ShortcutGroup1_8 => "快捷键1组9",
                EventCode.ShortcutGroup1_9 => "快捷键1组10",
                EventCode.ShortcutGroup2_0 => "快捷键2组1",
                EventCode.ShortcutGroup2_1 => "快捷键2组2",
                EventCode.ShortcutGroup2_2 => "快捷键2组3",
                EventCode.ShortcutGroup2_3 => "快捷键2组4",
                EventCode.ShortcutGroup2_4 => "快捷键2组5",
                EventCode.ShortcutGroup2_5 => "快捷键2组6",
                EventCode.ShortcutGroup2_6 => "快捷键2组7",
                EventCode.ShortcutGroup2_7 => "快捷键2组8",
                EventCode.ShortcutGroup2_8 => "快捷键2组9",
                EventCode.ShortcutGroup2_9 => "快捷键2组10",
                _ => "未定义",
            };

        }
        #endregion

        #region Serialization
        public void SerializeKeyCodes()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("customEventCodes");
            doc.AppendChild(root);

            foreach (EventCode ec in System.Enum.GetValues(typeof(EventCode)))
            {
                XmlElement ele = doc.CreateElement("event");
                ele.SetAttribute("eventCode", ec.ToString());
                ele.SetAttribute("keyCode", GetKeyCode(ec).ToString());
                root.AppendChild(ele);
            }

            System.IO.StringWriter sw = new System.IO.StringWriter();
            doc.Save(sw);

            string ret = sw.ToString();
            Utilities.Utility.SetString("Input.KeyCodes", ret);
        }

        public void DeserializeKeyCodes()
        {
            string ret = Utilities.Utility.GetString("Input.KeyCodes", string.Empty);
            if (string.IsNullOrEmpty(ret))
            {
                SetDefaultEventCodes();
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(ret);

                XmlElement root = doc["customEventCodes"];
                if (null != root)
                {
                    foreach (var node in root.ChildNodes)
                    {
                        XmlElement childEle = node as XmlElement;

                        EventCode ec = (EventCode)System.Enum.Parse(typeof(EventCode), childEle.GetAttribute("eventCode"));
                        KeyCode kc = (KeyCode)System.Enum.Parse(typeof(KeyCode), childEle.GetAttribute("keyCode"));

                        SetKeyCode(ec, kc);
                    }
                }
            }
        }


        #endregion
    }
}
