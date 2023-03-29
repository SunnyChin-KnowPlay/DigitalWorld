using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Xml;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
	    /// <summary>
    /// 地图
    /// </summary>
    public partial class MapInfo : InfoBase
    {
        public override int GetID()
        {
            return id;
        }

        public override object GetField(int index)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 唯一ID
        /// </summary>
        public System.Int32 Id => id;
        private System.Int32 id;
        /// <summary>
        /// 地图名字
        /// </summary>
        public System.String Name => name;
        private System.String name;
        /// <summary>
        /// 地图配置资源路径
        /// </summary>
        public System.String AssetPath => assetPath;
        private System.String assetPath;
        /// <summary>
        /// 地图等级
        /// </summary>
        public System.Int32 Level => level;

        public override int FieldCount => throw new System.NotImplementedException();

        private System.Int32 level;

        public MapInfo()
        {
        }


    }


	    /// <summary>
    /// 地图
    /// </summary>
    [TableNameAttibute("map")]
    public partial class MapTable : TableBase<MapInfo>
    {
        public override string TableName => "map";
    }
}