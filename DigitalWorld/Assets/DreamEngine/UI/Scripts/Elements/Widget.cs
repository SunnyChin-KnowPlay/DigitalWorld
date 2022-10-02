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
        public RectTransform RectTransform => rectTransform;
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

        public virtual RectTransform GetRectTransform(string path)
        {
            return GetTransform(path) as RectTransform;
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

        /// <summary>
        /// ÑÓ³Ù¹Ø±Õ
        /// </summary>
        /// <param name="duration">ºÄÊ± Ãë</param>
        public void DelayHide(float duration)
        {
            StopCoroutine(nameof(ApplyDelayHide));
            StartCoroutine(ApplyDelayHide(duration));
        }

        protected virtual IEnumerator ApplyDelayHide(float duration)
        {
            if (duration > 0)
            {
                yield return new WaitForSeconds(duration);
            }

            this.Hide();
        }
        #endregion


    }

}

