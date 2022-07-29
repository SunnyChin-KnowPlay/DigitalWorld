using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Xml;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
	    /// <summary>
    /// 角色
    /// </summary>
    public partial class CharacterInfo : InfoBase
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
        private int _hp;
        /// <summary>
        /// 基础血量
        /// </summary>
        public int hp => _hp;
        private int _attack;
        /// <summary>
        /// 攻击力
        /// </summary>
        public int attack => _attack;
        private int _moveSpeed;
        /// <summary>
        /// 移动速度
        /// </summary>
        public int moveSpeed => _moveSpeed;
        private string _prefabPath;
        /// <summary>
        /// 预制件路径
        /// </summary>
        public string prefabPath => _prefabPath;

        public CharacterInfo()
        {
        }

#region Encode
        protected override void OnEncode()
        {
            base.OnEncode();

            this.Encode(this._id);
            this.Encode(this._name);
            this.Encode(this._hp);
            this.Encode(this._attack);
            this.Encode(this._moveSpeed);
            this.Encode(this._prefabPath);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._id, "id");
            this.Encode(this._name, "name");
            this.Encode(this._hp, "hp");
            this.Encode(this._attack, "attack");
            this.Encode(this._moveSpeed, "moveSpeed");
            this.Encode(this._prefabPath, "prefabPath");
        }
        #endregion

#region Decode
        protected override void OnDecode()
        {
            base.OnDecode();

            this.Decode(ref this._id);
            this.Decode(ref this._name);
            this.Decode(ref this._hp);
            this.Decode(ref this._attack);
            this.Decode(ref this._moveSpeed);
            this.Decode(ref this._prefabPath);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._id, "id");
            this.Decode(ref this._name, "name");
            this.Decode(ref this._hp, "hp");
            this.Decode(ref this._attack, "attack");
            this.Decode(ref this._moveSpeed, "moveSpeed");
            this.Decode(ref this._prefabPath, "prefabPath");
        }
#endregion

#region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this._id);
            this.CalculateSize(this._name);
            this.CalculateSize(this._hp);
            this.CalculateSize(this._attack);
            this.CalculateSize(this._moveSpeed);
            this.CalculateSize(this._prefabPath);
        }
#endregion
    }


	    /// <summary>
    /// 角色
    /// </summary>
    [TableNameAttibute("character")]
    public partial class CharacterTable : TableBase<CharacterInfo>
    {
        public override string TableName => "character";
    }
}