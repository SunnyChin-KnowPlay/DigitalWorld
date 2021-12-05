using Dream.Extension.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Logic
{
    public class UnitControl : LogicControl
    {
        #region Delegate
        public delegate void OnProcessControl(LogicControl c);
        #endregion

        #region Id
        protected uint uid = 0;
        public uint Uid { get { return uid; } }
        #endregion

        #region Params
        protected NavMeshAgent navMeshAgent;
        protected int currentMoveType;
        private const int moveTypeMask = 0x1;

        public float moveSpeed = 1f;

        #endregion

        #region Controls
        protected Dictionary<ELogicControlType, LogicControl> controls = null;

        public AnimatorControl Animator
        {
            get
            {
                return this.controls[ELogicControlType.Animator] as AnimatorControl;
            }
        }
        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();

            this.SetupControls();

            navMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            UpdateMove();
        }
        #endregion

        #region Logic
        private void EachAllControls(OnProcessControl handle)
        {
            if (null != handle)
            {
                foreach (var kvp in this.controls)
                {
                    handle.Invoke(kvp.Value);
                }
            }
        }

        public void Setup(uint uid, UnitInfo info)
        {
            this.uid = uid;
            this.Setup(info);
        }

        public override void Setup(UnitInfo info)
        {
            base.Setup(info);

            EachAllControls(OnSetupControl);
        }

        private void SetupControls()
        {
            if (null == this.controls)
                this.controls = new Dictionary<ELogicControlType, LogicControl>();
            else
                this.controls.Clear();

            this.controls.Add(ELogicControlType.Attribute, this.GetOrAddComponent<AttributeControl>());
            this.controls.Add(ELogicControlType.Animator, this.GetOrAddComponent<AnimatorControl>());
        }

        private void OnSetupControl(LogicControl c)
        {
            c.Setup(this.info);
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

            AnimatorControl ac = this.Animator;

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

