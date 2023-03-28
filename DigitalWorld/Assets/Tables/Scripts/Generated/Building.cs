using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Xml;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
    /// <summary>
    /// 建筑
    /// </summary>
    public partial class BuildingInfo : InfoBase
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
        /// 名字
        /// </summary>
        public System.String Name => name;
        private System.String name;
        /// <summary>
        /// 占格尺寸
        /// </summary>
        public Dream.FixMath.FixVector3 Size => size;
        private Dream.FixMath.FixVector3 size;
        /// <summary>
        /// 预制件路径
        /// </summary>
        public System.String PrefabPath => prefabPath;
        private System.String prefabPath;

        public BuildingInfo()
        {
        }

        #region Encode
        protected override void OnEncode()
        {
            base.OnEncode();

            this.Encode(this.id);
            this.Encode(this.name);
            this.Encode(this.size);
            this.Encode(this.prefabPath);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this.id, "id");
            this.Encode(this.name, "name");
            this.Encode(this.size, "size");
            this.Encode(this.prefabPath, "prefabPath");
        }
        #endregion

        #region Decode
        protected override void OnDecode()
        {
            base.OnDecode();

            this.Decode(ref this.id);
            this.Decode(ref this.name);
            this.Decode(ref this.size);
            this.Decode(ref this.prefabPath);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this.id, "id");
            this.Decode(ref this.name, "name");
            this.Decode(ref this.size, "size");
            this.Decode(ref this.prefabPath, "prefabPath");
        }
        #endregion

        #region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this.id);
            this.CalculateSize(this.name);
            this.CalculateSize(this.size);
            this.CalculateSize(this.prefabPath);
        }
        #endregion
    }


    /// <summary>
    /// 建筑
    /// </summary>
    [TableNameAttibute("building")]
    public partial class BuildingTable : TableBase<BuildingInfo>
    {
        public override string TableName => "building";
    }
}