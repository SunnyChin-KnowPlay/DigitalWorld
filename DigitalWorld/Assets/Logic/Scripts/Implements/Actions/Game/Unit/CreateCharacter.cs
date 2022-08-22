/**
 * 该文件通过代码生成器生成
 * 默认模板的回调函数会抛出NotImplementedException异常
 * 在创建对应的行动后，建议第一时间实现函数效果。
 */
using DigitalWorld.Game;
using System;

namespace DigitalWorld.Logic.Actions.Game.Unit
{
    public partial class CreateCharacter
    {
        /// <summary>
        /// 当进场时的回调
        /// 如果是延时类行动 则该函数实现"进场"效果 例如给目标对象挂上效果之类的
        /// 如果是瞬时类行动 则将实现直接写入该函数
        /// </summary>
        protected override void OnEnter()
        {
            base.OnEnter();

            UnitData unitData = UnitData.CreateFromCharacter(this.cfgId);

            WorldManager wm = WorldManager.Instance;
            ControlUnit unit = wm.RegisterUnit(unitData);
            if (null != unit)
            {
                unit.LogicPosition = Utilities.Convert.ToVector3(this.localPosition);
            }
        }


    }
}
