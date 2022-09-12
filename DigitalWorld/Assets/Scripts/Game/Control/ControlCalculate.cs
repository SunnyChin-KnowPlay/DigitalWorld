using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 计算控制
    /// </summary>
    public class ControlCalculate : ControlLogic
    {
        #region Logic
        public virtual int CalculateDamage(ref ParamInjury param)
        {
            int resultDamage = 0;

            if (null == param.source || null == param.target)
            {
                return resultDamage;
            }

            ControlUnit target = param.target.Unit;
            ControlUnit source = param.source.Unit;

            // 反向伤害特殊处理
            if (param.damageType == EDamagerType.Reverse)
            {

            }
            else
            {
                // 这里的所有伤害都是正向的，即会对目标造成伤害的
                // 首先，判定目标是否免疫伤害
                if (IsImmunityDamage(ref param))
                {
                    // 如果免疫伤害 则直接return 0
                    return 0;
                }


                int totalDamage = param.damage;
                param.damage = totalDamage;
                resultDamage = totalDamage;

                // 这里就把血量减去伤害
                PropertyValue hpValue = target.Property.Hp;
                hpValue -= totalDamage;
            }
            return resultDamage;
        }

        /// <summary>
        /// 是否免疫伤害，譬如包含无敌之类的效果
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static bool IsImmunityDamage(ref ParamInjury param)
        {
            UnitHandle handle = param.target;

            return handle && handle.Unit.IsForbiddenFunction(EUnitFunction.Vincible);
        }
        #endregion
    }
}
