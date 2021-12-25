using DigitalWorld.Proto.Game;
using DigitalWorld.TileMap;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 砖块对象
    /// </summary>
    public abstract partial class ControlTile : ControlUnit
    {
        #region Params
        private WorldManager world;

        public WorldManager World
        {
            get { return world; }
            private set { world = value; }
        }

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

        protected NavMeshObstacle obstacle = null;

        protected TileData data;
        public TileData Data { get { return data; } }

        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();

            this.world = WorldManager.Instance;
            this.obstacle = this.GetComponent<NavMeshObstacle>();
        }


        #endregion

        #region Setup
        public virtual void Setup(TileData data)
        {
            this.data = data;
        }

        public virtual TileData ExportData()
        {
            this.data = new TileData();

            data.index = this.gridIndex;
            data.tileBaseId = (int)this.TileType;
            data.level = this.level;

            return data;
        }


        #endregion

        #region Logic
        public override void OnBorn()
        {

        }

        public override void OnDead()
        {

        }
        #endregion
    }
}
