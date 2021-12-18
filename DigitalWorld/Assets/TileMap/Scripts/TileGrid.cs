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
        private int index;

        /// <summary>
        /// 砖块
        /// </summary>
        private ControlTile tile;
        public ControlTile Tile
        {
            get { return tile; }
        }

        private Transform trans;

        private TileMapControl mapControl = null;
        #endregion

        #region Behaviour
        private void Awake()
        {
            trans = transform;
        }
        #endregion

        #region Setup
        public void Setup(TileMapControl map, int index)
        {
            this.mapControl = map;
            this.index = index;

            int w1 = mapControl.MapSize.x / 2;
            int h1 = mapControl.MapSize.y / 2;

            int y = index / mapControl.MapSize.x;
            int x = index % mapControl.MapSize.x;

            Vector3 pos = new Vector3((x - w1 + 0.5f) * mapControl.GridSize.x, 0, (y - h1 + 0.5f) * mapControl.GridSize.y);
            if (null == trans)
                trans = this.transform;

            trans.localPosition = pos;
            trans.localScale = new Vector3(mapControl.GridSize.x, trans.localScale.y, mapControl.GridSize.y);
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
