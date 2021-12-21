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
        private List<int> _attributes;
        /// <summary>
        /// 属性
        /// </summary>
        public List<int> attributes => _attributes;

        public CharacterInfo()
        {
        }

#region Encode
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            this.Encode(this._id);
            this.Encode(this._name);
            this.Encode(this._attributes);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._id, "id");
            this.Encode(this._name, "name");
            this.Encode(this._attributes, "attributes");
        }
        #endregion

#region Decode
        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            this.Decode(ref this._id);
            this.Decode(ref this._name);
            this.Decode(ref this._attributes);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._id, "id");
            this.Decode(ref this._name, "name");
            this.Decode(ref this._attributes, "attributes");
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