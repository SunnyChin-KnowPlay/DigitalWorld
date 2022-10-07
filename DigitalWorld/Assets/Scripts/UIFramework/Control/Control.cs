using UnityEngine;
using DreamEngine.UI;

namespace DigitalWorld.UI
{
    /// <summary>
    /// 界面组件
    /// </summary>
    [RequireComponent(typeof(Widget))]
    public partial class Control : MonoBehaviour
    {
        #region Params
        /// <summary>
        /// 组件
        /// </summary>
        public Widget Widget => widget;
        protected Widget widget;

        public Canvas Canvas
        {
            get
            {
                return GetComponentInParent<Canvas>();
            }
        }
        #endregion

        #region Mono
        protected virtual void Awake()
        {
            this.widget = this.GetComponent<Widget>();
        }
        #endregion

        #region Getter
        protected virtual T GetControlComponent<T>(string path) where T : Component
        {
            Transform t = widget.GetTransform(path);
            if (null == t)
                return default;

            return t.GetComponent<T>();
        }

        protected virtual bool TryGetControlComponent<T>(string path, out T component) where T : Component
        {
            Transform t = widget.GetTransform(path);
            if (null == t)
            {
                component = null;
                return false;
            }


            return t.TryGetComponent<T>(out component);
        }

        protected virtual T GetOrAddControlComponent<T>(string path) where T : Component
        {
            Transform t = widget.GetTransform(path);
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
            widget.Show();
            this.OnShow();
        }

        public void Hide()
        {
            this.OnHide();
            widget.Hide();
        }

        protected virtual void OnShow()
        {

        }

        protected virtual void OnHide()
        {

        }
        #endregion
    }
}
