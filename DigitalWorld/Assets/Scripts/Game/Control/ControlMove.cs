using Dream.FixMath;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Game
{
    public class ControlMove : ControlLogic
    {
        #region Params
        public PropertyValue MoveSpeed
        {
            get
            {
                return this.Unit.Property.MoveSpeed;
            }
        }

        protected ControlAnimator animControl = null;

        public Vector3 LogicPos
        {
            get { return this.Unit.LogicPosition; }
        }

        public Vector3 MovingDir => movingDir;
        private Vector3 movingDir = Vector3.zero;

        public Vector3 MovingNormalizeDir
        {
            get
            {
                return movingDir.normalized;
            }
        }

        public bool IsMoving
        {
            get => isMoving;
            set
            {
                if (value != isMoving)
                {
                    isMoving = value;
                    animControl.SetBool("isMoving", isMoving);
                }
            }
        }
        private bool isMoving = false;

        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

        }

        protected virtual void OnEnable()
        {
            this.isMoving = false;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if (isMoving)
            {
                this.OnMove(Time.deltaTime);
            }
        }
        #endregion

        #region Logic
        public override void Setup(UnitControl unit, UnitData data)
        {
            base.Setup(unit, data);

            this.animControl = this.Unit.Animator;
        }

        /// <summary>
        /// 请求移动
        /// </summary>
        /// <param name="target"></param>
        public virtual void ApplyMove(Vector3 dir)
        {
            movingDir = dir;
            IsMoving = dir != Vector3.zero;
        }

        protected virtual void OnMove(float delta)
        {
            float moveSpeed = this.MoveSpeed.FactorByStand.SingleFloat;

            Vector3 normalizeDir = MovingNormalizeDir;
            Vector3 offset = delta * moveSpeed * normalizeDir;
            
            this.trans.Translate(offset);
            animControl.SetFloat("z", normalizeDir.z);
        }

        private void SetAnimatorSpeed(float speed)
        {
            if (null != this.animControl)
            {
                this.animControl.SetSpeed(speed);
            }
        }


        #endregion
    }
}
