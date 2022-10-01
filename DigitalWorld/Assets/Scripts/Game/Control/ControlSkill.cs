using DigitalWorld.Logic.Events;
using DigitalWorld.Table;
using Dream.Core;
using System.Collections.Generic;
using UnityEngine;

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
        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (KeyValuePair<int, Skill> kvp in skills)
            {
                kvp.Value.Recycle();
            }
            skills.Clear();
        }

        protected override void Update()
        {
            base.Update();

            int delta = Dream.FixMath.Utility.GetMillisecond(Time.deltaTime);
            this.OnUpdate(delta);
        }

        protected virtual void OnUpdate(int delta)
        {
            foreach (KeyValuePair<int, Skill> kvp in skills)
            {
                kvp.Value.Update(delta);
            }
        }

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

        public virtual void Spell(int slot, Logic.Events.Event ev)
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
