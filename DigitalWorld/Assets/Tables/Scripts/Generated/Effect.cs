using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Xml;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
	    /// <summary>
    /// 效果
    /// </summary>
    public partial class EffectInfo : InfoBase
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
        private string assetPath;
        /// <summary>
        /// 效果资产路径
        /// </summary>
        public string AssetPath => assetPath;

        public EffectInfo()
        {
        }

#region Encode
        protected override void OnEncode()
        {
            base.OnEncode();

            this.Encode(this.id);
            this.Encode(this.name);
            this.Encode(this.assetPath);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this.id, "id");
            this.Encode(this.name, "name");
            this.Encode(this.assetPath, "assetPath");
        }
        #endregion

#region Decode
        protected override void OnDecode()
        {
            base.OnDecode();

            this.Decode(ref this.id);
            this.Decode(ref this.name);
            this.Decode(ref this.assetPath);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this.id, "id");
            this.Decode(ref this.name, "name");
            this.Decode(ref this.assetPath, "assetPath");
        }
#endregion

#region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this.id);
            this.CalculateSize(this.name);
            this.CalculateSize(this.assetPath);
        }
#endregion
    }


	    /// <summary>
    /// 效果
    /// </summary>
    [TableNameAttibute("effect")]
    public partial class EffectTable : TableBase<EffectInfo>
    {
        public override string TableName => "effect";
    }
}