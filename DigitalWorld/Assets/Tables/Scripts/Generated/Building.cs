using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Collections.Generic;
using System.Data;
using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DigitalWorld.Table
{
	    /// <summary>
    /// 建筑
    /// </summary>
    [Serializable]
    public partial class BuildingInfo : InfoBase
    {
        public override int GetID()
        {
            return id;
        }

        /// <summary>
        /// ID
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
        /// <summary>
        /// 层，每个建筑都有层数概念，一个格子，在同一层内不能覆盖。
        /// </summary>
        public System.Int32 Layer => layer;
        private System.Int32 layer;

        #region Serialization
		public BuildingInfo()
		{

		}

        public BuildingInfo(SerializationInfo info, StreamingContext context)
			: base(info, context)
        {
            try
            {
                this.id = (System.Int32)info.GetValue("id", typeof(System.Int32));
                this.name = (System.String)info.GetValue("name", typeof(System.String));
                this.size = (Dream.FixMath.FixVector3)info.GetValue("size", typeof(Dream.FixMath.FixVector3));
                this.prefabPath = (System.String)info.GetValue("prefabPath", typeof(System.String));
                this.layer = (System.Int32)info.GetValue("layer", typeof(System.Int32));
            }
            catch (Exception ex)
            {

            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
			info.AddValue("id", this.id);
			info.AddValue("name", this.name);
			info.AddValue("size", this.size);
			info.AddValue("prefabPath", this.prefabPath);
			info.AddValue("layer", this.layer);
        }

        public override void ToDataRow(DataRow row)
        {
            row["id"] = id;
            row["name"] = name;
            row["size"] = size;
            row["prefabPath"] = prefabPath;
            row["layer"] = layer;
        }

        public override void FromDataRow(DataRow row)
        {
            this.id = (System.Int32)ParseDataField(row, "id", typeof(System.Int32));
            this.name = (System.String)ParseDataField(row, "name", typeof(System.String));
            this.size = (Dream.FixMath.FixVector3)ParseDataField(row, "size", typeof(Dream.FixMath.FixVector3));
            this.prefabPath = (System.String)ParseDataField(row, "prefabPath", typeof(System.String));
            this.layer = (System.Int32)ParseDataField(row, "layer", typeof(System.Int32));
        }
        #endregion
    }


	    /// <summary>
    /// 建筑
    /// </summary>
    [TableNameAttibute("building")]
    [Serializable]
    public partial class BuildingTable : TableBase<BuildingInfo>
    {
        public override string TableName => "building";

        public BuildingTable()
        {

        }

        public BuildingTable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}