/**
 * 该文件通过代码生成器生成
 * 默认模板的回调函数会抛出NotImplementedException异常
 * 在创建对应的行动后，建议第一时间实现函数效果。
 */
using System;

namespace DigitalWorld.Logic.Actions.Game.Unit
{
    public partial class ClearTargets
    {

        /// <summary>
        /// 当退场时的回调
        /// 如果是延时类行动 则该函数实现"退场"效果 例如给目标对象移除效果之类的
        /// 如果是瞬时类行动 则可将实现逻辑写到此函数中
        /// </summary>
        protected override void OnExit()
        {
            base.OnExit();

            Events.EventHandler ev = this.Trigger.TriggeringEventHandler;

            if (null != ev)
            {
                ev.Targets.Clear();
            }
        }


    }
}
