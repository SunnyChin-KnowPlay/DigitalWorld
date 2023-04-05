using DreamEngine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalWorld.UI
{
    /// <summary>
    /// 界面控制器
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(WidgetPanel))]
    public partial class PanelControl : Control
    {
        #region Const
        public const string PanelPrefabPath = "Assets/Res/UI/Elements/Panel/Panel.prefab";
        #endregion
        #region Params
        /// <summary>
        /// panel组件
        /// </summary>
        public WidgetPanel Panel => GetWidget<WidgetPanel>();
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
        #endregion

        #region Events
        /// <summary>
        /// 当接收到退出事件时 关闭这个界面
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        protected virtual void OnEscape(Events.EEventType type, System.EventArgs args)
        {
            this.Hide();
        }
        #endregion

    }
}
