using DigitalWorld.Asset;
using DigitalWorld.Inputs;
using DigitalWorld.UI;
using DreamEngine.UI;
using UnityEngine;

namespace DigitalWorld.Game.UI.Settings
{
    public class SettingControl : SettingContent
    {
        #region Params
        private WidgetListView listView;


        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            listView = this.GetControlComponent<WidgetListView>("Scroll View");

            GameObject obj = AssetManager.LoadAsset<GameObject>("Assets/Res/UI/Elements/Button/TitleButton.prefab");

            foreach (EventCode ec in System.Enum.GetValues(typeof(EventCode)))
            {
                GameObject go = GameObject.Instantiate(obj);
                SettingControlTitle sc = go.AddComponent<SettingControlTitle>();
                sc.OnClickTitle += OnClickTitle;
                sc.Setup(ec);
                //listView.AddItemToBottom(go);
            }

            if (this.TryGetControlComponent<WidgetButton>("ResetButton", out WidgetButton resetButton))
            {
                resetButton.onClick.AddListener(OnClickReset);
            }
        }

        #endregion

        #region Listen
        private void OnClickTitle(EventCode ec)
        {
            UIManager uiManager = UIManager.Instance;
            SettingEventCodePanel settingPanel = uiManager.ShowPanel<SettingEventCodePanel>(SettingEventCodePanel.Path);
            if (null != settingPanel)
            {
                settingPanel.Setup(ec);
            }

            Debug.Log("SettingControl:OnClickTitle the eventCodes is:\t" + ec);
        }

        private void OnClickReset()
        {
            InputManager.Instance.SetDefaultEventCodes();
        }
        #endregion
    }
}
