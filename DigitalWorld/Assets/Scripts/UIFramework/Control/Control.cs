using UnityEngine;

namespace DigitalWorld.UI
{
    /// <summary>
    /// 逻辑控制器
    /// </summary>
    public partial class Control : MonoBehaviour
    {
        /// <summary>
        /// 容器
        /// </summary>
        protected Container container;

        protected virtual void Awake()
        {
            this.container = this.GetComponent<Container>();
            if (null != container)
            {
                this.BindWidgets();
            }
        }

        protected virtual void BindWidgets()
        {

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
