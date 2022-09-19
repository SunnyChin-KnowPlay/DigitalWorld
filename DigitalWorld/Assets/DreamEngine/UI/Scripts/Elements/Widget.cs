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
        public virtual void Show()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
        }
        #endregion

        #region Animation
        protected virtual IEnumerator ApplyEnter()
        {
            throw new NotImplementedException();
        }

        protected virtual IEnumerator ApplyExit()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}

