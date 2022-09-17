using UnityEngine;

namespace DigitalWorld.UI
{
    /// <summary>
    /// 界面组件
    /// </summary>
    public partial class Widget : MonoBehaviour
    {
        #region Params
        protected RectTransform rectTransform;
        #endregion

        #region Mono
        protected virtual void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
        }
        #endregion

        #region Widget
        protected virtual Transform GetTransform(string path)
        {
            return rectTransform.Find(path);
        }

        protected virtual T GetWidgetComponent<T>(string path) where T : Component
        {
            Transform t = rectTransform.Find(path);
            if (null == t)
                return default;

            return t.GetComponent<T>();
        }

        protected virtual T GetOrAddWidgetComponent<T>(string path) where T : Component
        {
            Transform t = rectTransform.Find(path);
            if (null == t)
                return default;

            if (!t.TryGetComponent<T>(out var c))
                c = t.gameObject.AddComponent<T>();
            return c;
        }
        #endregion

        #region Logic
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
        #endregion
    }
}
