using UnityEngine;

namespace DigitalWorld.UI
{
    /// <summary>
    /// 界面控制器
    /// </summary>
    public partial class PanelControl : Widget
    {
        /// <summary>
        /// 画布 界面控制器必须拥有画布
        /// </summary>
        protected Canvas canvas;

        protected override void Awake()
        {
            base.Awake();

            canvas = this.GetComponent<Canvas>();
        }

        public void Show()
        {
            if (null == this.gameObject)
                return;

            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (null == this.gameObject)
                return;

            this.gameObject.SetActive(false);
        }

        #region Bind
        protected override void BindWidgets()
        {
            base.BindWidgets();
        }
        #endregion

        #region Enable & Disable
        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }
        #endregion

        
    }
}
