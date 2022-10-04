using DreamEngine.UI;
using UnityEngine;

namespace DigitalWorld.UI
{
    /// <summary>
    /// 界面控制器
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public partial class PanelControl : Control
    {
        #region Params
        /// <summary>
        /// 画布 界面控制器必须拥有画布
        /// </summary>
        protected Canvas canvas;

        /// <summary>
        /// panel组件
        /// </summary>
        public WidgetPanel Panel => widget as WidgetPanel;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            canvas = this.GetComponent<Canvas>();
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
          
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
