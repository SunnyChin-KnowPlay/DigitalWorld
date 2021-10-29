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
        }


    }
}
