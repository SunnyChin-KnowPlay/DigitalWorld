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
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            root = this.GetTransform("Root") as RectTransform;
        }

        protected virtual void OnEnable()
        {
            this.StartCoroutine(nameof(ApplyEnter));
        }
        #endregion

        #region Switch
        public override void Hide()
        {
            this.StopCoroutine(nameof(ApplyEnter));
            this.StartCoroutine(nameof(ApplyExit));
        }
        #endregion

        #region Animation
        protected override IEnumerator ApplyEnter()
        {
            if (!root.TryGetComponent<CanvasGroup>(out var cg))
                yield break;

            float t = 0;
            cg.alpha = t;
            while (t < 1.0f)
            {
                t += Time.deltaTime * 2;
                cg.alpha = Mathf.Min(t, 1);
                yield return new WaitForEndOfFrame();
            }
        }

        protected override IEnumerator ApplyExit()
        {
            if (!root.TryGetComponent<CanvasGroup>(out var cg))
                yield break;

            float t = 1;
            cg.alpha = t;
            while (t > 0)
            {
                t -= Time.deltaTime * 2;
                cg.alpha = Mathf.Min(t, 1);
                yield return new WaitForEndOfFrame();
            }

            this.gameObject.SetActive(false);
        }
        #endregion
    }
}
