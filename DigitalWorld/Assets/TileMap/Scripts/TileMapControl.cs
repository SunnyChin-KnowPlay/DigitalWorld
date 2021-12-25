using DigitalWorld.Asset;
using DigitalWorld.Logic;
using Dream.FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DigitalWorld.TileMap
{
    /// <summary>
    /// 瓦片地图控制器
    /// </summary>
    public class TileMapControl : MonoBehaviour
    {
        #region Params
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

        /// <summary>
        /// 当前选中的瓦片
        /// </summary>
        public static GameObject currentEditTileGo = null;

        private GameObject currentSelectedTileGo = null;
        public GameObject CurrentSelectedTileGo { get { return currentSelectedTileGo; } }
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
        #endregion

        #region Setup
        public void Setup()
        {
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

        #region Edit Func
#if UNITY_EDITOR
        public void StartEdit()
        {
            this.isEditing = true;

            //GameObject go = AssetManager.Instance.LoadAsset<GameObject>("Tile/TileOrigin.prefab");
            //currentEditTileGo = GameObject.Instantiate(go);
        }

        public void StopEdit()
        {
            DestroyEditTileGo();
            this.currentSelectedTileGo = null;
            this.isEditing = false;
        }

        public void SelectTileGo(GameObject go)
        {
            this.currentSelectedTileGo = go;

            DestroyEditTileGo();

            if (null != currentSelectedTileGo)
            {
                GameObject g = GameObject.Instantiate(go) as GameObject;
                currentEditTileGo = g;
            }
        }

        private void DestroyEditTileGo()
        {
            if (null != currentEditTileGo)
            {
                GameObject.DestroyImmediate(currentEditTileGo);
                currentEditTileGo = null;
            }
        }
#endif
        #endregion
    }

}


