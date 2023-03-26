using System.Collections.Generic;
using DigitalWorld.Extension.Unity;
using DigitalWorld.Events;
using System;

namespace DigitalWorld.Game
{
    [Serializable]
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

            UnitHandle lastedInjurer = this.Calculate.LastedInjurer;
            if (default != lastedInjurer) // 如果有击杀者
            {
                if (lastedInjurer != this.UnitHandle) // 排除自杀
                {
                    if (lastedInjurer.Unit.IsPlayerControlling) // 如果击杀者是玩家主控的角色的话
                    {
                        string message = string.Format($"{lastedInjurer.Unit.Data.Name}击杀了{this.Data.Name}");
                        EventManager.Instance.Invoke(EEventType.Notice_Board, new EventArgsNotice(message, 5f));
                    }
                }
            }
        }
        #endregion
    }
}
