using DigitalWorld.Logic;
using DigitalWorld.Logic.Events;
using DigitalWorld.Table;
using Dream.Core;
using UnityEngine.Assertions;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 技能
    /// </summary>
    public class Skill : PooledObject
    {
        #region Params
        /// <summary>
        /// 技能ID
        /// </summary>
        public int SkillId { get { return skillId; } }
        protected int skillId;

        /// <summary>
        /// 插槽ID
        /// </summary>
        public int Slot { get => slot; }
        protected int slot;

        public SkillInfo SkillInfo { get => TableManager.Instance.SkillTable[skillId]; }

        /// <summary>
        /// 触发器
        /// </summary>
        public Trigger Trigger { get; protected set; }

        /// <summary>
        /// 技能控制器
        /// </summary>
        protected ControlSkill ControlSkill { get; private set; }

        /// <summary>
        /// CD的持续时间
        /// </summary>
        public int CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }
        private int cooldownDuration;

        /// <summary>
        /// 是否为正在CD中
        /// </summary>
        public bool IsCoolingDown
        {
            get
            {
                return cooldownDuration > 0;
            }
        }
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();

            this.skillId = 0;
            this.slot = 0;
            this.cooldownDuration = 0;
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            if (null != Trigger)
            {
                Trigger.Recycle();
                Trigger = null;
            }
        }
        #endregion

        #region Logic
        public void Update(int delta)
        {
            if (IsCoolingDown)
            {
                this.cooldownDuration -= delta;

                // 如果CD结束了
                if (!IsCoolingDown)
                {
                    this.OnCooleddown();
                }
            }
        }

        public virtual void Setup(ControlSkill control, int skillId, int slot)
        {
            this.ControlSkill = control;
            this.skillId = skillId;
            this.slot = slot;

            SkillInfo info = SkillInfo;

            Assert.IsTrue(null != info);

            Logic.Trigger trigger = Logic.LogicHelper.AllocateTrigger(info.BehaviourAssetPath);
            this.Trigger = trigger;

        }

        /// <summary>
        /// 执行
        /// </summary>
        public virtual void Spell(Event ev)
        {
            // 如果正在冷却中 直接return
            if (IsCoolingDown)
            {
                return;
            }

            Cooldown(this.SkillInfo.CoolDownTime);

            if (ev.EventId == EEvent.Trigger)
            {
                if (Trigger.Clone() is Trigger trigger)
                {
                    trigger.Invoke(ev);
                    ControlSkill.Unit.Trigger.RunTrigger(trigger);
                }
            }
        }

        public virtual void Cooldown(int duration)
        {
            if (cooldownDuration <= 0)
            {
                OnInCooldown();
            }
            cooldownDuration += this.SkillInfo.CoolDownTime;
        }
        #endregion

        #region Listen
        /// <summary>
        /// 进入CD
        /// </summary>
        protected virtual void OnInCooldown()
        {

        }

        /// <summary>
        /// CD结束
        /// </summary>
        protected virtual void OnCooleddown()
        {

        }
        #endregion
    }
}
