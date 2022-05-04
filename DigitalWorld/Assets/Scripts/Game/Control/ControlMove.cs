using UnityEngine;

namespace DigitalWorld.Game
{
    public class ControlMove : ControlLogic
    {
        #region Params
        public PropertyValue MoveSpeed
        {
            get
            {
                return this.unit.Property.MoveSpeed;
            }
        }

        protected ControlAnimator animContrl = null;

        public Vector3 TargetPos
        {
            get { return targetPos; }
        }
        protected Vector3 targetPos = Vector3.zero;

        public Vector3 LogicPos
        {
            get { return this.unit.LogicPosition; }
        }

        /// <summary>
        /// 目标方向
        /// </summary>
        public Vector3 TargetDir
        {
            get
            {
                Vector3 dir = TargetPos - LogicPos;
                return dir;
            }
        }
        /// <summary>
        /// 检测是否走到位置的临界值
        /// </summary>
        private const float moveMagnitudeCriticalSqr = 0.1f * 0.1f;

        private bool isMoving = false;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void OnEnable()
        {
            this.animContrl = this.unit.Animator;
            this.targetPos = Vector3.zero;
            this.isMoving = false;
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
        /// <summary>
        /// 请求移动
        /// </summary>
        /// <param name="target"></param>
        public virtual void ApplyMoveTo(Vector3 target)
        {
            targetPos = target;
        }

        protected virtual void OnMove(float delta)
        {
            float moveSpeed = this.MoveSpeed.FactorByStand.singleFloat;
            SetAnimatorSpeed(moveSpeed);

            Vector3.MoveTowards(LogicPos, targetPos, delta * moveSpeed);
        }

        private void SetAnimatorSpeed(float speed)
        {
            if (null != this.animContrl)
            {
                this.animContrl.SetSpeed(speed);
            }
        }

        /// <summary>
        /// 检查当前的位置是否已经到达和目标的临界值之内了
        /// 比较Target和Current的sqrMagnitude是否<=moveDistanceCriticalSqr
        /// </summary>
        /// <returns></returns>
        private bool CheckIsNearestByTarget()
        {
            Vector3 v3 = (TargetPos - LogicPos);
            return v3.sqrMagnitude < moveMagnitudeCriticalSqr;
        }

        private Vector3 NextMoveStep(float delta)
        {
            return Vector3.one;
        }
        #endregion
    }
}
