using DigitalWorld.Logic;
using DigitalWorld.Logic.Events;
using DigitalWorld.Table;
using Dream.Core;

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
        public int SkillId
        {
            get { return skillId; }
        }
        protected int skillId;

        /// <summary>
        /// 插槽ID
        /// </summary>
        public int Slot
        {
            get => slot;
        }
        protected int slot;

        public SkillInfo SkillInfo { get => TableManager.Instance.SkillTable[skillId]; }

        /// <summary>
        /// 触发器
        /// </summary>
        public Trigger Trigger { get; protected set; }

        protected ControlSkill ControlSkill { get; private set; }
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();

            this.skillId = 0;
            this.slot = 0;
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
        public virtual void Setup(ControlSkill control, int skillId, int slot)
        {
            this.ControlSkill = control;
            this.skillId = skillId;
            this.slot = slot;

            SkillInfo info = SkillInfo;
            if (null != info)
            {
                Logic.Trigger trigger = Logic.LogicHelper.AllocateTrigger(info.BehaviourAssetPath);
                this.Trigger = trigger;
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        public virtual void Spell(Event ev)
        {
            if (ev.EventId == EEvent.Trigger)
            {
                if (Trigger.Clone() is Trigger trigger)
                {
                    trigger.Invoke(ev);
                    ControlSkill.Unit.Trigger.RunTrigger(trigger);
                }
            }
        }
        #endregion
    }
}
