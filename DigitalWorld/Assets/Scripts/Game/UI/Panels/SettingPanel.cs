using DigitalWorld.Asset;
using DigitalWorld.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DreamEngine.UI;
using DigitalWorld.Extension.Unity;

namespace DigitalWorld.Game.UI
{
    /// <summary>
    /// 设置界面
    /// </summary>
    public class SettingPanel : PanelControl
    {
        #region Enter
        public const string path = "Assets/Res/UI/Game/Panels/SettingPanel.prefab";
        #endregion

        #region Params
        /// <summary>
        /// 菜单
        /// </summary>
        private RectTransform menuTransform;


        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            menuTransform = this.GetControlComponent<RectTransform>("Root/Menus");

            SetupMenus();
        }

        #endregion

        #region Switch
       
        #endregion

        #region Logic
        private void SetupMenus()
        {
            GameObject assetObject = AssetManager.LoadAsset<GameObject>("Assets/Res/UI/Elements/Button.prefab");

            GameObject go;
            go = CreateMenuButton(assetObject, "图像", OnClickMenuGraphics);
            go.transform.SetParent(menuTransform, false);

            go = CreateMenuButton(assetObject, "控制", OnClickMenuControls);
            go.transform.SetParent(menuTransform, false);

            go = CreateMenuButton(assetObject, "回到登录", OnClickToLogin);
            go.transform.SetParent(menuTransform, false);
        }

        private GameObject CreateMenuButton(GameObject assetObj, string text, UnityAction action)
        {
            GameObject go = GameObject.Instantiate(assetObj);
            go.name = text;

            if (go.TryGetComponent<WidgetButton>(out WidgetButton wb))
            {
                wb.buttonText = text;
                wb.UpdateUI();
            }

            Button button = go.GetOrAddComponent<Button>();
            button.onClick.AddListener(action);

            return go;
        }
        #endregion

      

        #region UI Events
        /// <summary>
        /// 图像
        /// </summary>
        private void OnClickMenuGraphics()
        {

        }

        /// <summary>
        /// 控制
        /// </summary>
        private void OnClickMenuControls()
        {

        }

        /// <summary>
        /// 退回登录
        /// </summary>
        private void OnClickToLogin()
        {
            WorldManager.Instance.Exit();
        }
        #endregion
    }
}
