/**
 * 该文件通过代码生成器生成
 * 默认模板的回调函数会抛出NotImplementedException异常
 * 在创建对应的行动后，建议第一时间实现函数效果。
 */
using DigitalWorld.Game;
using DigitalWorld.Logic.Events;
using System;

namespace DigitalWorld.Logic.Actions.Game.Unit
{
    public partial class PlayAnimator
    {
        /// <summary>
        /// 当进场时的回调
        /// 如果是延时类行动 则该函数实现"进场"效果 例如给目标对象挂上效果之类的
        /// 如果是瞬时类行动 则将实现直接写入该函数
        /// </summary>
        protected override void OnEnter()
        {
            base.OnEnter();

            Events.EventHandler ev = this.Trigger.TriggeringEventHandler;
            UnitHandle handle = ev.Triggering;
            if (handle)
            {
                handle.Unit.Animator.SetTrigger(this.triggerKey);
            }
        }


    }
}
