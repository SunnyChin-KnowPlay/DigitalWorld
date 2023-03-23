using UnityEngine;

namespace DigitalWorld.Game
{
    [System.Serializable]
    public struct MapData
    {
        public int width; // 地图宽度
        public int height; // 地图高度
        public GridData[] gridData; // 存储格子数据的一维数组
    }




}
