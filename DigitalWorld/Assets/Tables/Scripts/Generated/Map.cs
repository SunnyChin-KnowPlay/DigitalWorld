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
            return _id;
        }

        private int _id;
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int id => _id;
        private string _name;
        /// <summary>
        /// 地图名字
        /// </summary>
        public string name => _name;
        private string _assetPath;
        /// <summary>
        /// 地图配置资源路径
        /// </summary>
        public string assetPath => _assetPath;
        private int _level;
        /// <summary>
        /// 地图等级
        /// </summary>
        public int level => _level;

        public MapInfo()
        {
        }

#region Encode
        protected override void OnEncode()
        {
            base.OnEncode();

            this.Encode(this._id);
            this.Encode(this._name);
            this.Encode(this._assetPath);
            this.Encode(this._level);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._id, "id");
            this.Encode(this._name, "name");
            this.Encode(this._assetPath, "assetPath");
            this.Encode(this._level, "level");
        }
        #endregion

#region Decode
        protected override void OnDecode()
        {
            base.OnDecode();

            this.Decode(ref this._id);
            this.Decode(ref this._name);
            this.Decode(ref this._assetPath);
            this.Decode(ref this._level);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._id, "id");
            this.Decode(ref this._name, "name");
            this.Decode(ref this._assetPath, "assetPath");
            this.Decode(ref this._level, "level");
        }
#endregion

#region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this._id);
            this.CalculateSize(this._name);
            this.CalculateSize(this._assetPath);
            this.CalculateSize(this._level);
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