using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Xml;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
	    /// <summary>
    /// 地基
    /// </summary>
    public partial class TilebaseInfo : InfoBase
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
        /// 名字
        /// </summary>
        public string name => _name;
        private string _prefabPath;
        /// <summary>
        /// 地块预制件路径
        /// </summary>
        public string prefabPath => _prefabPath;

        public TilebaseInfo()
        {
        }

#region Encode
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            this.Encode(this._id);
            this.Encode(this._name);
            this.Encode(this._prefabPath);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._id, "id");
            this.Encode(this._name, "name");
            this.Encode(this._prefabPath, "prefabPath");
        }
        #endregion

#region Decode
        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            this.Decode(ref this._id);
            this.Decode(ref this._name);
            this.Decode(ref this._prefabPath);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._id, "id");
            this.Decode(ref this._name, "name");
            this.Decode(ref this._prefabPath, "prefabPath");
        }
#endregion

#region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this._id);
            this.CalculateSize(this._name);
            this.CalculateSize(this._prefabPath);
        }
#endregion
    }


	    /// <summary>
    /// 地基
    /// </summary>
    [TableNameAttibute("tilebase")]
    public partial class TilebaseTable : TableBase<TilebaseInfo>
    {
        public override string TableName => "tilebase";
    }
}