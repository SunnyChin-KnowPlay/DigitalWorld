using Dream.FixMath;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Game
{
    public class ControlMove : ControlLogic
    {
        #region Params
        /// <summary>
        /// 检测是否走到位置的临界值
        /// </summary>
        private const float moveMagnitudeCriticalSqr = 0.1f * 0.1f;

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

        public Vector3 NormalizeXZDir
        {
            get
            {
                Vector3 d = TargetPos - LogicPos;
                FixVector3 dir = new FixVector3(d.x, 0, d.z);
                dir = dir.NormalizeTo(1000);
                return new Vector3(dir.x / FixDefined.floatPrecision, dir.y / FixDefined.floatPrecision, dir.z / FixDefined.floatPrecision);
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
            this.targetPos = Vector3.zero;

            this.isMoving = false;
        }

        protected override void Start()
        {
            base.Start();
        }

        private IEnumerator StartEnum()
        {
            yield return new WaitForSeconds(5.0f);
            ApplyMoveTo(new Vector3(0, 0, 5));
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
        public override void Setup(ControlUnit unit, UnitData data)
        {
            base.Setup(unit, data);

            this.animContrl = this.unit.Animator;
        }

        /// <summary>
        /// 请求移动
        /// </summary>
        /// <param name="target"></param>
        public virtual void ApplyMoveTo(Vector3 target)
        {
            targetPos = target;
            isMoving = true;
            animContrl.SetBool("isMoving", true);
        }

        protected virtual void OnMove(float delta)
        {
            float moveSpeed = this.MoveSpeed.FactorByStand.singleFloat;

            if (CheckIsNearestByTarget())
            {
                OnArrived();
            }
            else
            {
                Vector3 offset = NormalizeXZDir * delta * moveSpeed;
                SetAnimatorSpeed(moveSpeed);

                Vector3 targetPos = TargetPos;
                Vector3 targetXZ = new Vector3(targetPos.x, this.trans.position.y, targetPos.z);
                this.trans.LookAt(targetXZ);

                this.trans.Translate(offset);
                animContrl.SetFloat("z", moveSpeed);
            }
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

            return v3.sqrMagnitude <= moveMagnitudeCriticalSqr;
        }

        /// <summary>
        /// 到达目标点
        /// </summary>
        private void OnArrived()
        {
            animContrl.SetBool("isMoving", false);
            this.isMoving = false;
        }
        #endregion
    }
}
