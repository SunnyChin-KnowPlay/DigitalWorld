using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEngine.UI
{
    [ExecuteInEditMode]
    public class Widget : MonoBehaviour
    {
        #region Params
        protected RectTransform rectTransform;
        #endregion

        #region Mono
        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        #endregion

        #region Getter
        public virtual Transform GetTransform(string path)
        {
            return rectTransform.Find(path);
        }
        #endregion

        #region Switch
        public void Show()
        {
            this.OnShow();
        }

        public void Hide()
        {
            this.OnHide();
        }

        protected virtual void OnShow()
        {
            this.gameObject.SetActive(true);
        }

        protected virtual void OnHide()
        {
            this.gameObject.SetActive(false);
        }
        #endregion


    }

}

