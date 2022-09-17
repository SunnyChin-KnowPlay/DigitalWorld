using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DigitalWorld.Extension.Unity;

namespace DigitalWorld.Game
{
    public partial class ControlCharacter : ControlUnit
    {
        #region Params
        /// <summary>
        /// 是否为主控玩家 就是玩家自己
        /// </summary>
        public bool IsHost { get => isHost; set => isHost = value; }
        private bool isHost;

        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();


        }

        protected override void Update()
        {
            base.Update();

        }
        #endregion

        #region Logic
        protected override void SetupControls()
        {
            if (null == this.controls)
            {
                this.controls = new Dictionary<ELogicControlType, ControlLogic>
                {
                    { ELogicControlType.Property, this.GetOrAddComponent<ControlProperty>() },
                    { ELogicControlType.Animator, this.GetOrAddComponent<ControlAnimator>() },
                    { ELogicControlType.Skill, this.GetOrAddComponent<ControlSkill>() },
                    { ELogicControlType.Move, this.GetOrAddComponent<ControlMove>() },
                    { ELogicControlType.Trigger, this.GetOrAddComponent<ControlTrigger>() },
                    { ELogicControlType.Situation, this.GetOrAddComponent<ControlSituation>() },
                    { ELogicControlType.Calculate, this.GetOrAddComponent<ControlCalculate>() },
                    { ELogicControlType.Event, this.GetOrAddComponent<ControlEvent>() },
                };
            }

            this.EachAllControls(OnSetupControl);
        }

        protected override void OnBorn()
        {
            base.OnBorn();
        }

        protected override void OnDead()
        {
            base.OnDead();
        }
        #endregion
    }
}
