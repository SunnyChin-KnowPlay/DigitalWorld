using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
	    /// <summary>
    /// 
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

        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            this.Decode(ref this._id);
            this.Decode(ref this._name);
            this.Decode(ref this._attributes);
        }

        
    }


	    /// <summary>
    /// 
    /// </summary>
    [TableNameAttibute("character")]
    public partial class CharacterTable : TableBase<CharacterInfo>
    {
        public override string TableName => "character";
    }
}