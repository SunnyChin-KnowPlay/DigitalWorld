using Dream.Core;

namespace DigitalWorld.Game
{
    public class Map : System.IDisposable
    {
        /// <summary>
        /// 地图宽度
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 地图高度
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// 一维数组存储地图中的格子
        /// </summary>
        private readonly Grid[] grids;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            grids = new Grid[width * height];

            // 初始化地图中的所有格子
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    Grid grid = ObjectPool<Grid>.Instance.Allocate();
                    grid.Setup(this, index, new UnityEngine.Vector2Int(x, y), true);
                    grids[index] = grid;
                }
            }
        }



        // 获取地图上指定坐标的格子
        public Grid GetGridAt(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                int index = y * Width + x;
                return grids[index];
            }

            return null;
        }

        #region IDisposable
        public void Dispose()
        {
            if (null != this.grids)
            {
                for (int i = 0; i < grids.Length; i++)
                {
                    grids[i].Recycle();
                }
            }
        }
        #endregion
    }

}
