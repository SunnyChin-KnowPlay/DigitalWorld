using Dream.Extension.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Game
{
    public abstract class ControlUnit : ControlLogic
    {
        #region Delegate
        public delegate void OnProcessControl(ControlLogic c);
        #endregion

        #region Params
        protected uint uid = 0;
        /// <summary>
        /// ��λ��ΨһID
        /// </summary>
        public uint Uid { get { return uid; } }

        /// <summary>
        /// ����״̬
        /// </summary>
        protected EUnitStatus status;
        public EUnitStatus Status { get { return status; } }

        /// <summary>
        /// �Ƿ�Ϊ��Ծ�е�
        /// </summary>
        public bool IsRunning
        {
            get { return status == EUnitStatus.Running; }
        }

        /// <summary>
        /// ��ǰ���߼�λ��
        /// </summary>
        public Vector3 LogicPosition
        {
            get { return this.trans.position; }
            set { this.trans.position = value; }
        }
        protected WorldManager world;

        #endregion

        #region Controls
        protected Dictionary<ELogicControlType, ControlLogic> controls = null;

        public ControlAnimator Animator => this.controls[ELogicControlType.Animator] as ControlAnimator;
       
        public ControlProperty Property => this.controls[ELogicControlType.Property] as ControlProperty;

        public ControlSkill Skill => this.controls[ELogicControlType.Skill] as ControlSkill;

        public ControlMove Move => this.controls[ELogicControlType.Move] as ControlMove;
        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();
            this.world = WorldManager.Instance;
            this.status = EUnitStatus.Idle;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        public override void Destroy()
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                GameObject.DestroyImmediate(this.gameObject);
            }
        }
        #endregion

        #region Setup
        public void Setup(uint uid, UnitData data)
        {
            this.uid = uid;
            this.Setup(this, data);
        }

        public override void Setup(ControlUnit unit, UnitData data)
        {
            base.Setup(unit, data);

            this.SetupControls();

        }
        #endregion

        #region Controls
        protected void EachAllControls(OnProcessControl handle)
        {
            if (null != handle)
            {
                foreach (var kvp in this.controls)
                {
                    handle.Invoke(kvp.Value);
                }
            }
        }

        protected abstract void SetupControls();

        protected virtual void OnSetupControl(ControlLogic c)
        {
            c.Setup(this, this.data);
        }
        #endregion

        #region Logic
        /// <summary>
        /// ������ʱ
        /// </summary>
        public virtual void OnBorn()
        {
            status = EUnitStatus.Running;
        }

        /// <summary>
        /// ����ʱ
        /// </summary>
        public virtual void OnDead()
        {
            this.status = EUnitStatus.Dead;
            this.OnFuneral();
        }

        /// <summary>
        /// ��...�������ڳ��׽���...
        /// </summary>
        protected virtual void OnFuneral()
        {
            this.status = EUnitStatus.WaitRecycle;
        }
        #endregion

    }

}

