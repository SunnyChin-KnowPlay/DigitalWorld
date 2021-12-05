using UnityEngine;

namespace DigitalWorld.Logic
{
    public class AnimatorControl : LogicControl
    {
        protected Animator animator;

        public Animator Animator { get { return animator; } }

        protected override void Awake()
        {
            base.Awake();

            this.animator = this.GetComponent<Animator>();
        }

    }
}
