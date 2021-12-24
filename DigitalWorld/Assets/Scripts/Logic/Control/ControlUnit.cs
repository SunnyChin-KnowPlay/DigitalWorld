using Dream.Extension.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Logic
{
    public abstract class ControlUnit : ControlLogic
    {
        #region Delegate
        public delegate void OnProcessControl(ControlLogic c);
        #endregion

        #region Params
        protected uint uid = 0;
        public uint Uid { get { return uid; } }
        #endregion

        #region Controls
        protected Dictionary<ELogicControlType, ControlLogic> controls = null;

        public ControlAnimator Animator
        {
            get
            {
                return this.controls[ELogicControlType.Animator] as ControlAnimator;
            }
        }
        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();

            this.SetupControls();


        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// 删除对象
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
        #endregion

        #region Controls
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

        protected virtual void SetupControls()
        {
            if (null == this.controls)
                this.controls = new Dictionary<ELogicControlType, ControlLogic>();
            else
                this.controls.Clear();

            this.controls.Add(ELogicControlType.Attribute, this.GetOrAddComponent<ControlAttribute>());
            this.controls.Add(ELogicControlType.Animator, this.GetOrAddComponent<ControlAnimator>());
        }

        protected virtual void OnSetupControl(ControlLogic c)
        {
            c.Setup(this.info);
        }
        #endregion

        #region Logic
        /// <summary>
        /// 当出生时
        /// </summary>
        public abstract void OnBorn();

        /// <summary>
        /// 死亡时
        /// </summary>
        public abstract void OnDead();
        #endregion

    }

}

