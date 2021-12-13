using DigitalWorld.TileMap;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 砖块对象
    /// </summary>
    public class ControlTile : ControlLogic
    {
        #region Params
        protected WorldManager world;

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

        protected NavMeshObstacle obstacle = null;
        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();

            world = WorldManager.Instance;

            this.obstacle = this.GetComponent<NavMeshObstacle>();
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        public virtual void Destroy()
        {
            GameObject.Destroy(this.gameObject);
        }
        #endregion
    }
}
