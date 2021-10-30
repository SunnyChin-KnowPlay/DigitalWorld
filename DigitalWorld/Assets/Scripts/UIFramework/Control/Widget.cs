using UnityEngine;

namespace DigitalWorld.UI
{
    /// <summary>
    /// 界面组件
    /// </summary>
    public partial class Widget : MonoBehaviour
    {
        protected RectTransform rectTransform;

        protected virtual void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();

            this.BindWidgets();
        }

        protected virtual void BindWidgets()
        {
           
        }

        #region Widget
        protected virtual Transform GetTransform(string path)
        {
            return rectTransform.Find(path);
        }

        protected virtual T GetComponent<T>(string path) where T : Component
        {
            Transform t = rectTransform.Find(path);
            if (null == t)
                return default(T);

            return t.GetComponent<T>();
        }
        #endregion
    }
}
