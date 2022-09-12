using DigitalWorld.Defined.Game;
using DigitalWorld.Table;

namespace DigitalWorld.Game
{
    public struct UnitData
    {
        #region Params
        /// <summary>
        /// 单位类型
        /// </summary>
        public EUnitType unitType;

        /// <summary>
        /// 配置ID
        /// </summary>
        public int configId;

        /// <summary>
        /// 等级
        /// </summary>
        public int level;

        /// <summary>
        /// 角色信息
        /// </summary>
        public CharacterInfo CharacterInfo
        {
            get { return TableManager.Instance.CharacterTable[configId]; }
        }

        /// <summary>
        /// 血量
        /// </summary>
        public int Hp
        {
            get
            {
                int hp = 0;

                CharacterInfo info = this.CharacterInfo;
                if (null != info)
                {
                    hp = info.Hp;
                }

                return hp;
            }
        }

        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack
        {
            get
            {
                int attack = 0;

                CharacterInfo info = this.CharacterInfo;
                if (null != info)
                {
                    attack = info.Attack;
                }

                return attack;
            }
        }

        public int MoveSpeed
        {
            get
            {
                int moveSpeed = 0;

                CharacterInfo info = this.CharacterInfo;
                if (null != info)
                {
                    moveSpeed = info.MoveSpeed;
                }

                return moveSpeed;
            }
        }
        #endregion

        #region Common
        public static UnitData CreateFromCharacter(int configId)
        {
            UnitData unitData = new UnitData
            {
                configId = configId,
                unitType = EUnitType.Character,
                level = 1
            };
            return unitData;
        }
        #endregion
    }
}
