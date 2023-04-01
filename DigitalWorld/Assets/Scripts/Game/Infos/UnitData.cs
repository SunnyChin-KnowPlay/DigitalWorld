using DigitalWorld.Defined.Game;
using DigitalWorld.Table;
using Dream.FixMath;
using UnityEngine;

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
        /// 角色信息
        /// </summary>
        private Table.CharacterInfo CharacterInfo => TableManager.Instance.CharacterTable[configId];

        /// <summary>
        /// 建筑信息
        /// </summary>
        private Table.BuildingInfo BuildingInfo => TableManager.Instance.BuildingTable[configId];

        /// <summary>
        /// 血量
        /// </summary>
        public int Hp
        {
            get
            {
                int hp = 0;

                Table.CharacterInfo info = this.CharacterInfo;
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

                Table.CharacterInfo info = this.CharacterInfo;
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

                Table.CharacterInfo info = this.CharacterInfo;
                if (null != info)
                {
                    moveSpeed = info.MoveSpeed;
                }

                return moveSpeed;
            }
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.unitType)
                {
                    case EUnitType.Character:
                    {
                        return CharacterInfo?.Name;
                    }
                    case EUnitType.Building:
                    {
                        return BuildingInfo?.Name;
                    }
                }

                return string.Empty;
            }
        }

        public FixVector3 ScaleSize
        {
            get
            {
                Table.CharacterInfo info = CharacterInfo;
                if (null == info)
                    return new FixVector3(0, 0, 0);
                return info.ScaleSize;
            }
        }

        public string PrefabPath
        {
            get
            {
                switch (this.unitType)
                {
                    case EUnitType.Character:
                    {
                        return CharacterInfo?.PrefabPath;
                    }
                    case EUnitType.Building:
                    {
                        return BuildingInfo?.PrefabPath;
                    }
                }
                return string.Empty;
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
            };
            return unitData;
        }

        public static UnitData CreateFromBuilding(int configId)
        {
            UnitData unitData = new UnitData
            {
                configId = configId,
                unitType = EUnitType.Building,
            };
            return unitData;
        }
        #endregion
    }
}
