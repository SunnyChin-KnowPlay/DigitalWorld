using DigitalWorld.Logic;
using UnityEngine;

namespace DigitalWorld.TileMap
{
    public class TileGrid : MonoBehaviour
    {
        /// <summary>
        /// 索引号
        /// </summary>
        public int index;

        /// <summary>
        /// 砖块
        /// </summary>
        private ControlTile tile;
        public ControlTile Tile
        {
            get { return tile; }
        }

        #region Common
        public void SetTile(ControlTile tile)
        {
            if (null != this.tile)
            {
                tile.Destroy();
                this.tile = null;
            }

            this.tile = tile;
            this.tile.GridIndex = this.index;
        }
        #endregion
    }
}
