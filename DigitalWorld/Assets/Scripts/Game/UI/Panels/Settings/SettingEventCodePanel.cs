using DigitalWorld.Inputs;
using DigitalWorld.UI;
using DreamEngine.UI;
using UnityEngine;

namespace DigitalWorld.Game.UI.Settings
{
    public class SettingEventCodePanel : PanelControl
    {
        #region Enter
        public const string Path = "Assets/Res/UI/Game/Panels/SettingEventCodePanel.prefab";
        #endregion

        #region Params
        private TMPro.TMP_Text titleText;
        private TMPro.TMP_Text keyCodeText;

        private EventCode eventCode;
        private KeyCode lastKeyCode;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            titleText = this.GetControlComponent<TMPro.TMP_Text>("Root/Title");
            keyCodeText = this.GetControlComponent<TMPro.TMP_Text>("Root/KeyCode");

            if (this.TryGetControlComponent<WidgetButton>("Root/ConfirmButton", out WidgetButton confirmButton))
            {
                confirmButton.onClick.AddListener(OnClickConfirm);
            }

            if (this.TryGetControlComponent<WidgetButton>("Root/CancelButton", out WidgetButton cancelButton))
            {
                cancelButton.onClick.AddListener(OnClickCancel);
            }
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode) && !(keyCode >= KeyCode.Mouse0 && keyCode <= KeyCode.Mouse2))
                    {
                        lastKeyCode = keyCode;
                        keyCodeText.text = lastKeyCode.ToString();
                        break;
                    }
                }
            }
        }
        #endregion

        #region Logic
        public void Setup(EventCode ec)
        {
            this.eventCode = ec;
            titleText.text = string.Format("设置{0}按键", ec);

            KeyCode kc = InputManager.Instance.GetKeyCode(ec);
            keyCodeText.text = kc.ToString();
        }
        #endregion

        #region Listen
        private void OnClickConfirm()
        {
            InputManager.Instance.SetKeyCode(this.eventCode, this.lastKeyCode);
            this.Hide();
        }

        private void OnClickCancel()
        {
            this.Hide();
        }
        #endregion
    }
}
