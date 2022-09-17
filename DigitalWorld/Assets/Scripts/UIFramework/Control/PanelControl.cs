using UnityEngine;

namespace DigitalWorld.UI
{
    /// <summary>
    /// 界面控制器
    /// </summary>
    public partial class PanelControl : Widget
    {
        #region Params
        /// <summary>
        /// 画布 界面控制器必须拥有画布
        /// </summary>
        protected Canvas canvas;
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

       

    }
}
