using DigitalWorld.Extension.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Game
{
    [Serializable]
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

        /// <summary>
        /// �Ƿ�Ϊ������صĵ�λ
        /// </summary>
        public bool IsPlayerControlling { get => isPlayerControlling; set => isPlayerControlling = value; }
        protected bool isPlayerControlling;


        /// <summary>
        /// ������
        /// ������Ĺ��ܾͻᱻ��ֹʹ��
        /// </summary>
        protected int[] forbiddenFunctions = new int[(int)EUnitFunction.Max];
        #endregion

        #region Controls
        protected Dictionary<ELogicControlType, ControlLogic> controls = null;

        public ControlAnimator Animator => this.controls[ELogicControlType.Animator] as ControlAnimator;
        public ControlProperty Property => this.controls[ELogicControlType.Property] as ControlProperty;
        public ControlSkill Skill => this.controls[ELogicControlType.Skill] as ControlSkill;
        public ControlMove Move => this.controls[ELogicControlType.Move] as ControlMove;
        public ControlTrigger Trigger => this.controls[ELogicControlType.Trigger] as ControlTrigger;
        public ControlCalculate Calculate => this.controls[ELogicControlType.Calculate] as ControlCalculate;
        public ControlSituation Situation => this.controls[ELogicControlType.Situation] as ControlSituation;
        public ControlTest Test => this.controls[ELogicControlType.Test] as ControlTest;
        public ControlEvent Event => this.controls[ELogicControlType.Event] as ControlEvent;
        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();
            this.world = WorldManager.Instance;
            this.status = EUnitStatus.Idle;
        }

        protected virtual void OnEnable()
        {
            OnBorn();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        public void Destroy()
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

        public virtual void AddControl(ELogicControlType type, ControlLogic control)
        {
            if (this.controls.ContainsKey(type))
            {
                //TODO:�Ѿ����������ˣ��Ⱥ���...
                return;
            }

            control.Setup(this, this.Data);
            this.controls.Add(type, control);
        }

        public virtual void RemoveControl(ELogicControlType type)
        {
            this.controls.Remove(type);
        }

        protected abstract void SetupControls();

        protected virtual void OnSetupControl(ControlLogic c)
        {
            c.Setup(this, this.data);
        }
        #endregion

        #region Logic
        /// <summary>
        /// ��������
        /// </summary>
        public void ApplyDead()
        {
            if (status == EUnitStatus.Running)
            {
                this.OnDead();
            }
        }

        public IEnumerator ApplyFuneral(float duration)
        {
            yield return new WaitForSeconds(duration);

            this.OnFuneral();
        }

        /// <summary>
        /// ������ʱ
        /// </summary>
        protected virtual void OnBorn()
        {
            status = EUnitStatus.Running;
        }

        /// <summary>
        /// ����ʱ
        /// </summary>
        protected virtual void OnDead()
        {
            this.Animator.SetTrigger("dead");

            this.status = EUnitStatus.Dead;

            StartCoroutine(ApplyFuneral(5));
        }

        /// <summary>
        /// ��...�������ڳ��׽���...
        /// </summary>
        protected virtual void OnFuneral()
        {
            this.status = EUnitStatus.WaitRecycle;
        }
        #endregion

        #region Damage
        /// <summary>
        /// �����˺�
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual int ProcessDamage(ref ParamInjury param)
        {
            int result = Calculate.CalculateInjury(ref param);

            return result;
        }
        #endregion

        #region ForbiddenFunction
        /// <summary>
        /// �Ƿ��ֹ��ĳ���
        /// </summary>
        /// <param name="function">����ö��</param>
        /// <returns>true:��ֹ��|false:����</returns>
        public bool IsForbiddenFunction(EUnitFunction function)
        {
            return forbiddenFunctions[(int)function] > 0;
        }

        /// <summary>
        /// ��ֹ/��������
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public int ForbidFunction(EUnitFunction function)
        {
            forbiddenFunctions[(int)function]++;

            return forbiddenFunctions[(int)function];
        }

        /// <summary>
        /// ����/��������
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public int PermitFunction(EUnitFunction function)
        {
            forbiddenFunctions[(int)function] = Mathf.Max(forbiddenFunctions[(int)function] - 1, 0);
            return forbiddenFunctions[(int)function];
        }

        /// <summary>
        /// ��ս����ж�
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public int ClearForbiddenFunction(EUnitFunction function)
        {
            forbiddenFunctions[(int)function] = 0;
            return 0;
        }
        #endregion
    }

}

