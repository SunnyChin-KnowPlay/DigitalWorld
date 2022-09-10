using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Xml;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
	    /// <summary>
    /// 阵营
    /// </summary>
    public partial class CampInfo : InfoBase
    {
        public override int GetID()
        {
            return id;
        }

        private int id;
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int Id => id;
        private string name;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name => name;

        public CampInfo()
        {
        }

#region Encode
        protected override void OnEncode()
        {
            base.OnEncode();

            this.Encode(this.id);
            this.Encode(this.name);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this.id, "id");
            this.Encode(this.name, "name");
        }
        #endregion

#region Decode
        protected override void OnDecode()
        {
            base.OnDecode();

            this.Decode(ref this.id);
            this.Decode(ref this.name);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this.id, "id");
            this.Decode(ref this.name, "name");
        }
#endregion

#region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this.id);
            this.CalculateSize(this.name);
        }
#endregion
    }


	    /// <summary>
    /// 阵营
    /// </summary>
    [TableNameAttibute("camp")]
    public partial class CampTable : TableBase<CampInfo>
    {
        public override string TableName => "camp";
    }
}