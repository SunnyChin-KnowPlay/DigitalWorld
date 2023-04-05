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
    /// <summary>
    /// 面板开关动画类型
    /// </summary>
    public enum EPanelSwitchAnimationFunction
    {
        /// <summary>
        /// 空
        /// </summary>
        None = 0,
        /// <summary>
        /// 透明度
        /// </summary>
        Alpha = 1 << 0,
        /// <summary>
        /// 模糊
        /// </summary>
        Blur = 1 << 1,
    }

    public class WidgetPanel : Widget
    {
        #region Params
        public RectTransform Root => root;
        private RectTransform root;

        /// <summary>
        /// 开关动画曲线
        /// </summary>
        public AnimationCurve alphaAnimationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        /// <summary>
        /// 开关动画曲线
        /// </summary>
        public AnimationCurve blurAnimationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 4));

        /// <summary>
        /// 开场动画耗时
        /// </summary>
        public float showAniamtionDuration = 0.3f;
        /// <summary>
        /// 退场动画耗时
        /// </summary>
        public float hideAniamtionDuration = 0.3f;

        /// <summary>
        /// 动画功能枚举
        /// </summary>
        [SerializeField]
        [HideInInspector]
        public EPanelSwitchAnimationFunction animationFunction;

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

        #region Logic
        /// <summary>
        /// 检查是否包含动画功能
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        private bool CheckHasAnimationFunction(EPanelSwitchAnimationFunction function)
        {
            return (animationFunction & function) == function;
        }

        /// <summary>
        /// 检查是否不存在任何动画功能
        /// </summary>
        /// <returns></returns>
        private bool CheckAnimationFunctionsIsEmpty()
        {
            return animationFunction == EPanelSwitchAnimationFunction.None;
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
            if (!CheckAnimationFunctionsIsEmpty())
            {
                bool hasFunctionAlpha = CheckHasAnimationFunction(EPanelSwitchAnimationFunction.Alpha);
                hasFunctionAlpha &= root.TryGetComponent<CanvasGroup>(out CanvasGroup cg);

                bool hasFunctionBlur = CheckHasAnimationFunction(EPanelSwitchAnimationFunction.Blur);
                hasFunctionBlur &= root.TryGetComponent<Image>(out Image image);

                float t = 0;
                float speed = 1 / showAniamtionDuration;

                if (hasFunctionAlpha && null != cg)
                {
                    cg.alpha = alphaAnimationCurve.Evaluate(t);
                }

                if (hasFunctionBlur && null != image)
                {
                    image.material.SetFloat("_Size", blurAnimationCurve.Evaluate(t));
                }

                while (t < 1)
                {
                    t += Time.deltaTime * speed;
                    if (hasFunctionAlpha)
                    {
                        cg.alpha = Mathf.Min(alphaAnimationCurve.Evaluate(t), 1);
                    }

                    if (hasFunctionBlur)
                    {
                        image.material.SetFloat("_Size", blurAnimationCurve.Evaluate(t));
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        protected virtual IEnumerator ApplyExit()
        {
            if (!CheckAnimationFunctionsIsEmpty())
            {
                bool hasFunctionAlpha = CheckHasAnimationFunction(EPanelSwitchAnimationFunction.Alpha);
                hasFunctionAlpha &= root.TryGetComponent<CanvasGroup>(out CanvasGroup cg);

                bool hasFunctionBlur = CheckHasAnimationFunction(EPanelSwitchAnimationFunction.Blur);
                hasFunctionBlur &= root.TryGetComponent<Image>(out Image image);

                float t = 1;
                float speed = 1 / hideAniamtionDuration;

                while (t > 0)
                {
                    t -= Time.deltaTime * speed;

                    if (hasFunctionAlpha)
                    {
                        cg.alpha = Mathf.Min(alphaAnimationCurve.Evaluate(t), 1);
                    }

                    if (hasFunctionBlur)
                    {
                        image.material.SetFloat("_Size", blurAnimationCurve.Evaluate(t));
                    }

                    yield return new WaitForEndOfFrame();
                }
            }

            this.gameObject.SetActive(false);
        }
        #endregion
    }
}
