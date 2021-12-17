using DigitalWorld.Logic;
using UnityEngine;

namespace DigitalWorld.TileMap
{
    public class TileGrid : MonoBehaviour
    {
        #region Params
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

        private Transform trans;
        #endregion

        #region Behaviour
        private void Awake()
        {
            trans = transform;
        }
        #endregion

        #region Common
        public void SetTile(ControlTile tile)
        {
            if (null != this.tile)
            {
                tile.Destroy();
                this.tile = null;
            }

            this.tile = tile;
            if (null != this.tile)
            {
                this.tile.GridIndex = this.index;
                Transform tileTransform = this.tile.transform;
                tileTransform.SetParent(trans, false);
                tileTransform.localPosition = Vector3.up;
            }

        }
        #endregion
    }
}
