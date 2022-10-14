using DigitalWorld.Asset;
using DigitalWorld.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DreamEngine.UI;
using DigitalWorld.Extension.Unity;
using System.Collections.Generic;

namespace DigitalWorld.Game.UI.Settings
{
    /// <summary>
    /// 设置界面
    /// </summary>
    public class SettingPanel : PanelControl
    {
        #region Defined
        private enum SettingNode
        {
            /// <summary>
            /// 图像
            /// </summary>
            Graphics = 0,
            Control,
            ToLogin,
        }
        #endregion

        #region Enter
        public const string path = "Assets/Res/UI/Game/Panels/SettingPanel.prefab";
        #endregion

        #region Params
        /// <summary>
        /// 菜单
        /// </summary>
        private RectTransform menuTransform;

        private RectTransform graphicsTransform;
        private RectTransform controlTransform;

        private readonly Dictionary<SettingNode, RectTransform> contents = new Dictionary<SettingNode, RectTransform>();
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            menuTransform = this.GetControlComponent<RectTransform>("Root/Menus");

            graphicsTransform = this.Widget.GetRectTransform("Root/View/GraphicsContent");
            controlTransform = this.Widget.GetRectTransform("Root/View/ControlContent");

            graphicsTransform.gameObject.AddComponent<SettingGraphics>();
            controlTransform.gameObject.AddComponent<SettingControl>();

            if (this.TryGetControlComponent<WidgetButton>("Root/CloseButton", out WidgetButton closeButton))
            {
                closeButton.onClick.AddListener(OnClickClose);
            }

            SetupMenus();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Events.EventManager.Instance.RegisterListener(Events.EEventType.Escape, OnEscape);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Events.EventManager.Instance?.UnregisterListener(Events.EEventType.Escape, OnEscape);
        }
        #endregion

        #region Logic
        private void SetupMenus()
        {
            GameObject assetObject = AssetManager.LoadAsset<GameObject>("Assets/Res/UI/Elements/Button/MainButton.prefab");

            GameObject go;
            go = CreateMenuButton(assetObject, "Graphics", "图像", OnClickMenuGraphics);
            go.transform.SetParent(menuTransform, false);

            go = CreateMenuButton(assetObject, "Controls", "控制", OnClickMenuControls);
            go.transform.SetParent(menuTransform, false);

            go = CreateMenuButton(assetObject, "ReturnToLogin", "<color=#CD2A2DFF>回到登录</color>", OnClickToLogin);
            go.transform.SetParent(menuTransform, false);

            contents.Add(SettingNode.Graphics, graphicsTransform);
            contents.Add(SettingNode.Control, controlTransform);

        }

        private GameObject CreateMenuButton(GameObject assetObj, string name, string text, UnityAction action)
        {
            GameObject go = GameObject.Instantiate(assetObj);
            go.name = name;

            if (go.TryGetComponent<WidgetButton>(out WidgetButton wb))
            {
                wb.buttonText = text;
                wb.UpdateUI();
            }

            Button button = go.GetOrAddComponent<Button>();
            button.onClick.AddListener(action);

            return go;
        }

        private void ShowContent(SettingNode node)
        {
            foreach (var kvp in contents)
            {
                kvp.Value.gameObject.SetActive(kvp.Key == node);
            }
        }
        #endregion



        #region UI Events
        /// <summary>
        /// 图像
        /// </summary>
        private void OnClickMenuGraphics()
        {
            ShowContent(SettingNode.Graphics);
        }

        /// <summary>
        /// 控制
        /// </summary>
        private void OnClickMenuControls()
        {
            ShowContent(SettingNode.Control);
        }

        /// <summary>
        /// 退回登录
        /// </summary>
        private void OnClickToLogin()
        {
            WorldManager.Instance.Exit();
        }

        private void OnClickClose()
        {
            this.Hide();
        }
        #endregion
    }
}
