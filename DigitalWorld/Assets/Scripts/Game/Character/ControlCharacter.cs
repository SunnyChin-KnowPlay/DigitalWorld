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
                    { ELogicControlType.Behaviour, this.GetOrAddComponent<ControlBehaviour>() },
                    { ELogicControlType.Situation, this.GetOrAddComponent<ControlSituation>() },
                };
            }

            this.EachAllControls(OnSetupControl);
        }

        public override void OnBorn()
        {

        }

        public override void OnDead()
        {

        }
        #endregion
    }
}
