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

            this.BindWidgets();
        }
        #endregion

        #region Bind
        protected virtual void BindWidgets()
        {
           
        }
        #endregion

        #region Widget
        protected virtual Transform GetTransform(string path)
        {
            return rectTransform.Find(path);
        }

        protected virtual T GetComponent<T>(string path) where T : Component
        {
            Transform t = rectTransform.Find(path);
            if (null == t)
                return default;

            return t.GetComponent<T>();
        }
        #endregion
    }
}
