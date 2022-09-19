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

        protected Widget widget;
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
        }

        public void Hide()
        {
            widget.Hide();
        }
        #endregion
    }
}
