using Dream.FixMath;
using System;
using System.Runtime.Serialization;

namespace DigitalWorld.Game.Datas
{
    [Serializable]
    public class MapData : DataBase
    {
        #region Params
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
        public GridData[] gridDatas;
        #endregion

        #region Serialization
        public MapData(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.width = (int)info.GetValue("width", typeof(int));
            this.height = (int)info.GetValue("height", typeof(int));
            this.gridDatas = (GridData[])info.GetValue("gridDatas", typeof(GridData[]));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("width", width);
            info.AddValue("height", height);
            info.AddValue("gridDatas", gridDatas);
        }
        #endregion

        #region Construct
        /// <summary>
        /// MapData 构造函数
        /// </summary>
        /// <param name="width">地图宽度</param>
        /// <param name="height">地图高度</param>
        public MapData(int width, int height)
        {
            this.width = width;
            this.height = height;
            gridDatas = new GridData[width * height];

            for (int i = 0; i < width * height; i++)
            {
                int x = i % width;
                int y = i / width;

                FixVector3 position = new FixVector3(x + 0.5f, 0, y + 0.5f); // 根据每个格子的 1x1 大小计算位置，偏移向右上0.5f，格子的位置的是格子的中心点，并考虑地图的中心点
                gridDatas[i] = new GridData(i, position);
            }
        }
        #endregion
    }
}
