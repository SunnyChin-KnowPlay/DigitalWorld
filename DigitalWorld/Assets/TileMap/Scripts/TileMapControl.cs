using DigitalWorld.Asset;
using DigitalWorld.Game;
using DigitalWorld.Proto.Game;
using DigitalWorld.Table;
using Dream.Extension.Unity;
using Dream.FixMath;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.TileMap
{
    /// <summary>
    /// 瓦片地图控制器
    /// </summary>
    public class TileMapControl : MonoBehaviour
    {
        #region Params
        public const string defaultMapDataPath = "Res/Config/Maps";

        public Vector2 gridSize = Vector2.one;
        /// <summary>
        /// 格子尺寸
        /// </summary>
        public Vector2 GridSize { get { return gridSize; } }
        public FixVector2 mapSize;
        /// <summary>
        /// 地图尺寸
        /// </summary>
        public FixVector2 MapSize { get { return mapSize; } }
        public TileGrid[] grids = null;

        private Dictionary<int, ControlTile> tiles = new Dictionary<int, ControlTile>();

        /// <summary>
        /// 地图等级
        /// </summary>
        public int level;

        /// <summary>
        /// 当前地图资源
        /// </summary>
        private TextAsset currentMapAsset;
        /// <summary>
        /// 当前操作的地图资源
        /// </summary>
        public TextAsset CurrentMapAsset { get { return currentMapAsset; } }

        /// <summary>
        /// 当前地图数据
        /// </summary>
        private MapData mapData = null;
        public MapData MapData
        {
            get { return mapData; }
        }

        private int mapId = 0;
        public int MapId
        { get { return mapId; } }

        #endregion

        #region Edit Params
#if UNITY_EDITOR
        /// <summary>
        /// 是否正在编辑中
        /// </summary>
        private bool isEditing = false;
        public bool IsEditing
        {
            get { return isEditing; }
        }

        public ETileType currentType = ETileType.None;
#endif
        #endregion

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Grids
        public void CalculateGrids()
        {
            if (null == grids || grids.Length <= 0)
                return;

            for (int i = 0; i < grids.Length; ++i)
            {
                TileGrid grid = grids[i];
                grid.Setup(this, i);
            }
        }

        /// <summary>
        /// 通过索引获取格子
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TileGrid GetGrid(int index)
        {
            if (index < 0 || index >= this.grids.Length)
                return null;

            return this.grids[index];
        }


        /// <summary>
        /// 重置所有地块
        /// </summary>
        public void ResetGrids()
        {
            if (null == grids || grids.Length <= 0)
                return;

            for (int i = 0; i < grids.Length; ++i)
            {
                TileGrid grid = grids[i];
                grid.Reset();
            }
        }
        #endregion

        #region Setup
        public void Setup(MapData data)
        {
            this.mapData = data;
            this.mapId = null != mapData ? mapData.mapId : 0;

            this.SetupTiles();
        }

        public void Clear()
        {
            this.ClearTiles();
        }
        #endregion

        #region Tiles
        /// <summary>
        /// 配置砖块
        /// </summary>
        private void SetupTiles()
        {
            if (null != this.tiles)
                this.ClearTiles();
            else
                this.tiles = new Dictionary<int, ControlTile>();

            if (null != this.mapData)
            {
                if (null != this.mapData.tiles)
                {
                    foreach (TileData tile in this.mapData.tiles)
                    {
                        ControlTile tileC = WorldManager.Instance.CreateTile(tile);
                        if (null != tileC)
                        {
                            TileGrid grid = GetGrid(tile.index);
                            if (null != grid)
                            {
                                grid.SetTile(tileC);

                                if (this.tiles.ContainsKey(tile.index))
                                    this.tiles.Remove(tile.index);
                                this.tiles.Add(tile.index, tileC);
                            }
                            else
                            {
                                tileC.Destroy();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 清空砖块
        /// </summary>
        public void ClearTiles()
        {
            foreach (var kvp in this.tiles)
            {
                kvp.Value.Destroy();
            }
            this.tiles.Clear();
        }

       
        public bool SetTile(ControlTile tile, int index)
        {
            TileGrid grid = this.GetGrid(index);
            if (null == grid)
                return false;

            grid.SetTile(tile);
            if (this.tiles.ContainsKey(index))
                this.tiles.Remove(index);
            if (null != tile)
                this.tiles.Add(index, tile);

            return true;
        }
        #endregion

        #region Edit Func
#if UNITY_EDITOR
        public void StartEdit()
        {
            this.isEditing = true;

            if (null != this.currentMapAsset)
            {
                MapData mapData = new MapData();
                mapData.Decode(this.currentMapAsset.bytes, 0);

                this.Setup(mapData);
            }

            this.currentType = ETileType.None;
            //GameObject go = AssetManager.Instance.LoadAsset<GameObject>("Tile/TileOrigin.prefab");
            //currentEditTileGo = GameObject.Instantiate(go);
        }

        public void StopEdit()
        {
            this.Clear();

            this.currentMapAsset = null;
            this.currentType = ETileType.None;
            this.isEditing = false;
        }


        public void OpenMap(TextAsset mapAsset)
        {
            if (null != this.mapData)
            {
                this.Clear();
                mapData = null;
            }

            this.currentMapAsset = mapAsset;
        }

        public MapData SaveMap()
        {
            this.mapData = new MapData();
            this.mapData.mapId = this.mapId;

            List<TileData> tiles = new List<TileData>();
            foreach (KeyValuePair<int, ControlTile> kvp in this.tiles)
            {
                tiles.Add(kvp.Value.ExportData());
            }
            mapData.tiles = tiles;

            return mapData;
        }
#endif
        #endregion
    }

}


