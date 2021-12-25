using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Xml;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
    /// <summary>
    /// 地块
    /// </summary>
    public partial class TileInfo : InfoBase
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
        private int _baseType;
        /// <summary>
        /// 地基类型 tilebase的ID
        /// </summary>
        public int baseType => _baseType;
        private string _prefabPath;
        /// <summary>
        /// 角色预制件路径
        /// </summary>
        public string prefabPath => _prefabPath;
        private int _hp;
        /// <summary>
        /// 血量
        /// </summary>
        public int hp => _hp;
        private int _attack;
        /// <summary>
        /// 攻击力
        /// </summary>
        public int attack => _attack;

        public TileInfo()
        {
        }

        #region Encode
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            this.Encode(this._id);
            this.Encode(this._name);
            this.Encode(this._baseType);
            this.Encode(this._prefabPath);
            this.Encode(this._hp);
            this.Encode(this._attack);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._id, "id");
            this.Encode(this._name, "name");
            this.Encode(this._baseType, "baseType");
            this.Encode(this._prefabPath, "prefabPath");
            this.Encode(this._hp, "hp");
            this.Encode(this._attack, "attack");
        }
        #endregion

        #region Decode
        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            this.Decode(ref this._id);
            this.Decode(ref this._name);
            this.Decode(ref this._baseType);
            this.Decode(ref this._prefabPath);
            this.Decode(ref this._hp);
            this.Decode(ref this._attack);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._id, "id");
            this.Decode(ref this._name, "name");
            this.Decode(ref this._baseType, "baseType");
            this.Decode(ref this._prefabPath, "prefabPath");
            this.Decode(ref this._hp, "hp");
            this.Decode(ref this._attack, "attack");
        }
        #endregion

        #region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this._id);
            this.CalculateSize(this._name);
            this.CalculateSize(this._baseType);
            this.CalculateSize(this._prefabPath);
            this.CalculateSize(this._hp);
            this.CalculateSize(this._attack);
        }
        #endregion
    }


    /// <summary>
    /// 地块
    /// </summary>
    [TableNameAttibute("tile")]
    public partial class TileTable : TableBase<TileInfo>
    {
        public override string TableName => "tile";
    }
}