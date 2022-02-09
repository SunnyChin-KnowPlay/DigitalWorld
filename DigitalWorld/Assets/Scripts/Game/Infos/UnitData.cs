﻿using DigitalWorld.Proto.Game;
using DigitalWorld.Table;

namespace DigitalWorld.Game
{
    public struct UnitData
    {
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
            get { return TableManager.instance.CharacterTable[configId]; }
        }

        /// <summary>
        /// 地块信息
        /// </summary>
        public TileInfo TileInfo
        {
            get { return TableManager.instance.TileTable[configId]; }
        }

        /// <summary>
        /// 血量
        /// </summary>
        public int Hp
        {
            get
            {
                int hp = 0;

                switch (this.unitType)
                {
                    case EUnitType.Character:
                    {
                        CharacterInfo info = this.CharacterInfo;
                        if (null != info)
                        {
                            hp = info.hp;
                        }
                        break;
                    }

                    case EUnitType.Tile:
                    {
                        TileInfo info = this.TileInfo;
                        if (null != info)
                            hp = info.hp;
                        break;
                    }
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

                switch (this.unitType)
                {
                    case EUnitType.Character:
                    {
                        CharacterInfo info = this.CharacterInfo;
                        if (null != info)
                        {
                            attack = info.attack;
                        }
                        break;
                    }

                    case EUnitType.Tile:
                    {
                        TileInfo info = this.TileInfo;
                        if (null != info)
                            attack = info.attack;
                        break;
                    }
                }

                return attack;
            }
        }
    }
}
