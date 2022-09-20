using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DreamEngine.UI
{
    public class WidgetPanel : Widget
    {
        #region Params
        private RectTransform root;

        public float showAniamtionDuration = 0.3f;
        public float hideAniamtionDuration = 0.3f;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            root = this.GetTransform("Root") as RectTransform;
        }

        protected virtual void OnEnable()
        {
            StopSwitchCoroutines();

            this.StartCoroutine(nameof(ApplyEnter));
        }
        #endregion

        #region Switch
        protected override void OnHide()
        {
            StopSwitchCoroutines();

            this.StartCoroutine(nameof(ApplyExit));
        }
        #endregion

        #region Animation
        private void StopSwitchCoroutines()
        {
            this.StopCoroutine(nameof(ApplyEnter));
            this.StopCoroutine(nameof(ApplyExit));
        }

        protected virtual IEnumerator ApplyEnter()
        {
            if (!root.TryGetComponent<CanvasGroup>(out var cg))
                yield break;

            float t = 0;
            cg.alpha = t;
            while (t < showAniamtionDuration)
            {
                t += Time.deltaTime;
                float a = t / showAniamtionDuration;
                cg.alpha = Mathf.Min(a, 1);
                yield return new WaitForEndOfFrame();
            }
        }

        protected virtual IEnumerator ApplyExit()
        {
            if (!root.TryGetComponent<CanvasGroup>(out var cg))
                yield break;

            float t = showAniamtionDuration;
            
            while (t > 0)
            {
                t -= Time.deltaTime;
                float a = t / showAniamtionDuration;
                cg.alpha = Mathf.Min(a, 1);
                yield return new WaitForEndOfFrame();
            }

            this.gameObject.SetActive(false);
        }
        #endregion
    }
}
