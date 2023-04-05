using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DreamEngine.UI
{
    public class WidgetTitle : Widget
    {
        #region Params

        /// <summary>
        /// 开关动画曲线
        /// </summary>
        public AnimationCurve switchAnimationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        /// <summary>
        /// 开场动画耗时
        /// </summary>
        public float showAniamtionDuration = 0.2f;
        /// <summary>
        /// 退场动画耗时
        /// </summary>
        public float hideAniamtionDuration = 0.2f;

        private Vector2 defaultSizeDelta;

        private RectTransform textRectTransform;
        private TMPro.TMP_Text titleText;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            defaultSizeDelta = rectTransform.sizeDelta;
            textRectTransform = GetRectTransform("TextContainer/Text");
            titleText = textRectTransform.GetComponent<TMPro.TMP_Text>();
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

        #region Logic

        #endregion

        #region Animation
        private void StopSwitchCoroutines()
        {
            this.StopCoroutine(nameof(ApplyEnter));
            this.StopCoroutine(nameof(ApplyExit));
        }

        protected virtual IEnumerator ApplyEnter()
        {

            rectTransform.sizeDelta = defaultSizeDelta;
            yield return new WaitForEndOfFrame();

            Vector2 sizeDelta = textRectTransform.sizeDelta;
            sizeDelta += new Vector2(80, defaultSizeDelta.y);

            float t = 0;
            float speed = 1 / showAniamtionDuration;

            while (t < 1)
            {
                t += Time.deltaTime * speed;

                rectTransform.sizeDelta = Vector2.Lerp(defaultSizeDelta, sizeDelta, t);

                yield return new WaitForEndOfFrame();
            }

        }

        protected virtual IEnumerator ApplyExit()
        {
            Vector2 sizeDelta = textRectTransform.sizeDelta;

            float t = 0;
            float speed = 1 / showAniamtionDuration;

            while (t < 1)
            {
                t += Time.deltaTime * speed;

                rectTransform.sizeDelta = Vector2.Lerp(sizeDelta, defaultSizeDelta, t);

                yield return new WaitForEndOfFrame();
            }

            this.gameObject.SetActive(false);
        }

        public void SetTitle(string title)
        {
            this.titleText.text = title;
        }
        #endregion
    }
}
