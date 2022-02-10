﻿using DigitalWorld.Proto.Game;
using DigitalWorld.TileMap;
using System.Collections;
using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 砖块对象
    /// </summary>
    public abstract partial class ControlTile : ControlUnit
    {
        #region Params
        /// <summary>
        /// 格子索引
        /// </summary>
        protected int gridIndex;
        public int GridIndex
        {
            get { return gridIndex; }
            set { gridIndex = value; }
        }
        public TileGrid TileGrid
        {
            get
            {
                if (null == world)
                    return null;

                TileMapControl map = world.TileMapControl;
                if (null == map)
                    return null;

                return map.GetGrid(this.gridIndex);
            }
        }

        public abstract ETileType TileType { get; }

#if UNITY_EDITOR
        public int level = 0;
        public int objectId = 0;
#endif

        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();
        }
        #endregion

        #region Factory
        public static ControlTile GetOrAddControl(GameObject go, ETileType type)
        {
            if (null == go)
                return null;
            ControlTile tile = go.GetComponent<ControlTile>();
            if (null == tile)
            {
                switch (type)
                {
                    case ETileType.Origin:
                        tile = go.AddComponent<TileOrigin>(); break;
                    case ETileType.Casino:
                        tile = go.AddComponent<TileCasino>(); break;
                    case ETileType.Chest:
                        tile = go.AddComponent<TileChest>(); break;
                    case ETileType.Door:
                        tile = go.AddComponent<TileDoor>(); break;
                    case ETileType.Grass:
                        tile = go.AddComponent<TileGrass>(); break;
                    case ETileType.MagicStone:
                        tile = go.AddComponent<TileMagicStone>(); break;
                    case ETileType.Monster:
                        tile = go.AddComponent<TileMonster>(); break;
                    case ETileType.Mountion:
                        tile = go.AddComponent<TileMountion>(); break;
                    case ETileType.Shop:
                        tile = go.AddComponent<TileShop>(); break;
                    case ETileType.Block:
                        tile = go.AddComponent<TileBlock>(); break;
                    case ETileType.Traveller:
                        tile = go.AddComponent<TileTraveller>(); break;
                }
            }

            return tile;
        }
        #endregion

        #region Setup
        public override void Setup(UnitData data)
        {
            base.Setup(data);
        }
        #endregion

        #region Editor
#if UNITY_EDITOR
        public virtual TileData ExportData()
        {
            TileData data;

            data = new TileData()
            {
                index = this.gridIndex,
                tileBaseId = (int)this.TileType,
                level = this.level,
                objectId = this.objectId,
            };

            return data;
        }
#endif
        #endregion

        #region Logic
        public override void OnBorn()
        {
            base.OnBorn();
        }

        public override void OnDead()
        {
            base.OnDead();

            StartCoroutine(ApplyDead());
        }

        /// <summary>
        /// 访问
        /// </summary>
        public virtual void Visit()
        {
            
        }

        private IEnumerator ApplyDead()
        {
            yield return new WaitForSeconds(Defined.deadDuration);

            this.OnFuneral();
        }

        #endregion
    }
}
