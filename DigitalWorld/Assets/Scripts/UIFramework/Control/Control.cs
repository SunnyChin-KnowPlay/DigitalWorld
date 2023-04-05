using UnityEngine;
using DreamEngine.UI;

namespace DigitalWorld.UI
{
    /// <summary>
    /// 界面控制器
    /// </summary>
    [RequireComponent(typeof(Widget))]
    public partial class Control : MonoBehaviour
    {
        #region Params
        /// <summary>
        /// 首要的小部件
        /// </summary>
        private Widget firstWidget;

        #endregion

        #region Mono
        protected virtual void Awake()
        {
            firstWidget = GetComponent<Widget>();
        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }
        #endregion

        #region Getter
        /// <summary>
        /// 画布
        /// </summary>
        public Canvas Canvas
        {
            get
            {
                return GetComponentInParent<Canvas>();
            }
        }

        public Widget FirstWidget
        {
            get => firstWidget;
        }

        /// <summary>
        /// 获取部件
        /// </summary>
        /// <typeparam name="TWidget"></typeparam>
        /// <returns></returns>
        public TWidget GetWidget<TWidget>() where TWidget : Widget
        {
            return GetComponent<TWidget>();
        }

        protected virtual T GetControlComponent<T>(string path) where T : Component
        {
            Transform t = this.FirstWidget.GetTransform(path);
            if (null == t)
                return default;

            return t.GetComponent<T>();
        }

        protected virtual bool TryGetControlComponent<T>(string path, out T component) where T : Component
        {
            Transform t = this.FirstWidget.GetTransform(path);
            if (null == t)
            {
                component = null;
                return false;
            }

            return t.TryGetComponent<T>(out component);
        }

        protected virtual T GetOrAddControlComponent<T>(string path) where T : Component
        {
            Transform t = this.FirstWidget.GetTransform(path);
            if (null == t)
                return default;

            if (!t.TryGetComponent<T>(out var c))
                c = t.gameObject.AddComponent<T>();
            return c;
        }
        #endregion

        #region Switch
        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        #endregion
    }
}
