using UnityEngine;

namespace DigitalWorld.Game
{
    public class ControlAnimator : ControlLogic
    {

        #region Params
        /// <summary>
        /// 动画控制器
        /// </summary>
        public Animator Animator { get { return animator; } }
        protected Animator animator;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            this.animator = this.GetComponent<Animator>();
        }

        protected virtual void OnEnable()
        {
            this.animator.speed = 1;
        }
        #endregion

        #region Logic
        public void SetSpeed(float speed)
        {
            this.animator.speed = speed;
        }

        public void SetFloat(string name, float value)
        {
            this.animator.SetFloat(name, value);
        }

        public void SetBool(string name, bool v)
        {
            this.animator.SetBool(name, v);
        }
        #endregion
    }
}
