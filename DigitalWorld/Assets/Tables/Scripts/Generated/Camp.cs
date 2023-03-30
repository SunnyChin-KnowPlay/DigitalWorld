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
    /// 阵营
    /// </summary>
    [Serializable]
    public partial class CampInfo : InfoBase
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

        #region Serialization
		public CampInfo()
		{

		}

        public CampInfo(SerializationInfo info, StreamingContext context)
			: base(info, context)
        {
			this.id = (System.Int32)info.GetValue("id", typeof(System.Int32));
			this.name = (System.String)info.GetValue("name", typeof(System.String));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
			info.AddValue("id", this.id);
			info.AddValue("name", this.name);
        }

        public override void SetupRow(DataRow row)
        {
            row["id"] = id;
            row["name"] = name;
        }

        public override void UpdateRow(DataRow row)
        {
            this.id = (System.Int32)row["id"];
            this.name = (System.String)row["name"];
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