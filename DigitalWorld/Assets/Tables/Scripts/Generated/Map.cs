using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Collections.Generic;
using System.Data;
using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DigitalWorld.Table
{
	    /// <summary>
    /// 地图
    /// </summary>
    [Serializable]
    public partial class MapInfo : InfoBase
    {
        public override int GetID()
        {
            return id;
        }

        /// <summary>
        /// ID
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
        private System.Int32 level;

        #region Serialization
		public MapInfo()
		{

		}

        public MapInfo(SerializationInfo info, StreamingContext context)
			: base(info, context)
        {
			this.id = (System.Int32)info.GetValue("id", typeof(System.Int32));
			this.name = (System.String)info.GetValue("name", typeof(System.String));
			this.assetPath = (System.String)info.GetValue("assetPath", typeof(System.String));
			this.level = (System.Int32)info.GetValue("level", typeof(System.Int32));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
			info.AddValue("id", this.id);
			info.AddValue("name", this.name);
			info.AddValue("assetPath", this.assetPath);
			info.AddValue("level", this.level);
        }

        public override void SetupRow(DataRow row)
        {
            row["id"] = id;
            row["name"] = name;
            row["assetPath"] = assetPath;
            row["level"] = level;
        }
        #endregion
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