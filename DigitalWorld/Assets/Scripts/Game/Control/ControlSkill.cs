using DigitalWorld.Logic.Events;
using DigitalWorld.Table;
using Dream.Core;
using System.Collections.Generic;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 技能控制器
    /// </summary>
    public class ControlSkill : ControlLogic
    {
        #region Params
        protected Dictionary<int, Skill> skills = new Dictionary<int, Skill>();

        #endregion

        #region Logic
        /// <summary>
        /// 学习技能
        /// </summary>
        /// <param name="info"></param>
        public void Study(SkillInfo info, int slot)
        {
            Skill skill = ObjectPool<Skill>.Instance.Allocate();
            skill.Setup(this, info.Id, slot);

            skills.Add(slot, skill);
        }

        public virtual void Spell(int slot, Event ev)
        {
            bool ret = this.skills.TryGetValue(slot, out Skill skill);
            if (ret)
            {
                skill.Spell(ev);
            }
        }
        #endregion
    }
}
