﻿using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 测试控制器
    /// 早期开发过程中临时测试用的模块
    /// 后续加入Debug和UnitTest后移除
    /// </summary>
    public class ControlTest : ControlLogic
    {
#if UNITY_EDITOR
        #region Params

        #endregion

        #region Mono
        protected override void Update()
        {
            base.Update();

            // 先用R键来测试技能
            if (Input.GetKeyUp(KeyCode.R))
            {
                // 必须有目标才能用技能
                if (this.Unit.Situation.Target)
                {
                    Logic.Events.Event ev = Logic.Events.Event.CreateTrigger(this.UnitHandle, this.Unit.Situation.Target);
                    this.Unit.Skill.Spell(0, ev);
                }

            }
        }
        #endregion
#endif
    }
}
