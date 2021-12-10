using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Logic
{
    public class ControlCharacter : ControlUnit
    {
        #region Params
        protected NavMeshAgent navMeshAgent;
        protected int currentMoveType;
        private const int moveTypeMask = 0x1;
        public float moveSpeed = 1f;
        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();

            navMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        protected override void Update()
        {
            base.Update();
            UpdateMove();
        }
        #endregion

        #region Move
        public void AddMoveDir(EMoveType t)
        {
            this.currentMoveType |= moveTypeMask << (int)t;
        }

        public void RemoveMoveDir(EMoveType t)
        {
            this.currentMoveType &= ~(moveTypeMask << (int)t);
        }

        private bool CheckMoveType(EMoveType t)
        {
            return ((this.currentMoveType >> (int)t) & moveTypeMask) == moveTypeMask;
        }

        private void UpdateMove()
        {
            Vector3 v = Vector3.zero;

            ControlAnimator ac = this.Animator;

            if (this.CheckMoveType(EMoveType.Forward))
            {
                v += Vector3.forward;
            }
            if (this.CheckMoveType(EMoveType.Right))
            {
                v += Vector3.right;
            }
            if (this.CheckMoveType(EMoveType.Back))
            {
                v += Vector3.back;
            }
            if (this.CheckMoveType(EMoveType.Left))
            {
                v += Vector3.left;
            }

            if (v != Vector3.zero)
            {
                v.Normalize();
                v = v * Time.deltaTime * moveSpeed;

                this.Move(v);

                if (null != ac && null != ac.Animator)
                    ac.Animator.SetBool("isRunning", true);
            }
            else
            {
                if (null != ac && null != ac.Animator)
                    ac.Animator.SetBool("isRunning", false);
            }
        }

        public virtual void Move(Vector3 offset)
        {
            if (null != navMeshAgent)
            {
                Vector3 worldPos = trans.position;
                Vector3 targetPos = worldPos + offset;
                this.transform.LookAt(targetPos);

                navMeshAgent.Move(offset);
            }
        }
        #endregion
    }
}
