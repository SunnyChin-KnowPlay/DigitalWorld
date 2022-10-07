using DigitalWorld.Inputs;
using DreamEngine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalWorld.Game.UI.Settings
{
    public class SettingControlTitle : MonoBehaviour
    {
        #region Event
        public delegate void OnClickTitleHandle(EventCode ec);
        public event OnClickTitleHandle OnClickTitle;
        #endregion

        #region Params
        private EventCode eventCode;
        private WidgetButton button = null;
        private KeyCode lastKeyCode = KeyCode.None;
        #endregion

        #region Mono
        private void Awake()
        {
            button = GetComponent<WidgetButton>();
            button.onClick.AddListener(OnClick);
        }

        private void Update()
        {
            KeyCode kc = InputManager.Instance.GetKeyCode(eventCode);
            if(lastKeyCode != kc)
            {
                lastKeyCode = kc;
                button.buttonText = kc.ToString();
                button.UpdateUI();
            }
          
        }
        #endregion

        #region Logic
        public void Setup(EventCode ec)
        {
            eventCode = ec;
            button.title = ec.ToString();


            KeyCode kc = InputManager.Instance.GetKeyCode(eventCode);
            lastKeyCode = kc;
            button.buttonText = kc.ToString();
          
            button.UpdateUI();

        }
        #endregion

        #region Listen
        private void OnClick()
        {
            if (null != OnClickTitle)
            {
                OnClickTitle.Invoke(this.eventCode);
            }
        }
        #endregion
    }
}
