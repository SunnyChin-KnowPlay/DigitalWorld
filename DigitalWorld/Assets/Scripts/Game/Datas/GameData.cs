using Dream.FixMath;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DigitalWorld.Game.Datas
{
    /// <summary>
    /// 游戏数据
    /// </summary>
    [Serializable]
    public class GameData : DataBase
    {
        #region Params
        /// <summary>
        /// 地图数据
        /// </summary>
        public MapData MapData
        {
            get => mapData;
            set => mapData = value;
        }
        private MapData mapData;
        #endregion

        #region Create
        public static GameData CreateTestData()
        {
            GameData gameData = new GameData
            {
                mapData = new MapData(128, 128)
            };
            return gameData;

        }
        #endregion

        #region Construct
        public GameData()
        {

        }
        #endregion

        #region Serialization
        public GameData(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.mapData = (MapData)info.GetValue("mapData", typeof(MapData));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("mapData", this.mapData);
        }
        #endregion

    }
}
