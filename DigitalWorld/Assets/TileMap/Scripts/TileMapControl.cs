using Dream.FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.TileMap
{
    /// <summary>
    /// 瓦片地图控制器
    /// </summary>
    public class TileMapControl : MonoBehaviour
    {
        public FixVector2 size;
        public TileGrid[] grids = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Common
        public void CalculateGrids()
        {
            if (null == grids || grids.Length <= 0)
                return;

            int w1 = size.x / 2;
            int h1 = size.y / 2;

            for (int i = 0; i < grids.Length; ++i)
            {
                TileGrid grid = grids[i];
                grid.index = i;
                int y = i / size.x;
                int x = i % size.x;

                Vector3 pos = new Vector3(x - w1 + 0.5f, 0, y - h1 + 0.5f);
                Transform t = grid.transform;
                t.localPosition = pos;
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
    }

}


