using UnityEngine;

namespace DigitalWorld.Game
{
    [System.Serializable]
    public struct MapData
    {
        /// <summary>
        /// 地图宽度
        /// </summary>
        public int width;

        /// <summary>
        /// 地图高度
        /// </summary>
        public int height;

        /// <summary>
        /// 存储格子数据的一维数组
        /// </summary>
        public GridData[] gridData;

        /// <summary>
        /// MapData 构造函数
        /// </summary>
        /// <param name="width">地图宽度</param>
        /// <param name="height">地图高度</param>
        public MapData(int width, int height)
        {
            this.width = width;
            this.height = height;
            gridData = new GridData[width * height];

            for (int i = 0; i < width * height; i++)
            {
                int x = i % width;
                int y = i / width;

                Vector3 position = new Vector3(x, 0, y); // 根据每个格子的 1x1 大小计算位置
                gridData[i] = new GridData { index = i, position = position };
            }
        }
    }


}
