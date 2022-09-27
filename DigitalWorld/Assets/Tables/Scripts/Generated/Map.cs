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
        private System.Int32 level;

        public MapInfo()
        {
        }

#region Encode
        protected override void OnEncode()
        {
            base.OnEncode();

            this.Encode(this.id);
            this.Encode(this.name);
            this.Encode(this.assetPath);
            this.Encode(this.level);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this.id, "id");
            this.Encode(this.name, "name");
            this.Encode(this.assetPath, "assetPath");
            this.Encode(this.level, "level");
        }
        #endregion

#region Decode
        protected override void OnDecode()
        {
            base.OnDecode();

            this.Decode(ref this.id);
            this.Decode(ref this.name);
            this.Decode(ref this.assetPath);
            this.Decode(ref this.level);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this.id, "id");
            this.Decode(ref this.name, "name");
            this.Decode(ref this.assetPath, "assetPath");
            this.Decode(ref this.level, "level");
        }
#endregion

#region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this.id);
            this.CalculateSize(this.name);
            this.CalculateSize(this.assetPath);
            this.CalculateSize(this.level);
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