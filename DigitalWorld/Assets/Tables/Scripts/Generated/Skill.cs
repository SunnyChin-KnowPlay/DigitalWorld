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
    /// 技能
    /// </summary>
    [Serializable]
    public partial class SkillInfo : InfoBase
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
        /// CD时间，单位：毫秒
        /// </summary>
        public System.Int32 CoolDownTime => coolDownTime;
        private System.Int32 coolDownTime;
        /// <summary>
        /// 行为资产路径
        /// </summary>
        public System.String BehaviourAssetPath => behaviourAssetPath;
        private System.String behaviourAssetPath;
        /// <summary>
        /// 施法半径，单位：毫米
        /// </summary>
        public System.Int32 CastRadius => castRadius;
        private System.Int32 castRadius;

        #region Serialization
		public SkillInfo()
		{

		}

        public SkillInfo(SerializationInfo info, StreamingContext context)
			: base(info, context)
        {
			this.id = (System.Int32)info.GetValue("id", typeof(System.Int32));
			this.name = (System.String)info.GetValue("name", typeof(System.String));
			this.coolDownTime = (System.Int32)info.GetValue("coolDownTime", typeof(System.Int32));
			this.behaviourAssetPath = (System.String)info.GetValue("behaviourAssetPath", typeof(System.String));
			this.castRadius = (System.Int32)info.GetValue("castRadius", typeof(System.Int32));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
			info.AddValue("id", this.id);
			info.AddValue("name", this.name);
			info.AddValue("coolDownTime", this.coolDownTime);
			info.AddValue("behaviourAssetPath", this.behaviourAssetPath);
			info.AddValue("castRadius", this.castRadius);
        }

        public override void ToDataRow(DataRow row)
        {
            row["id"] = id;
            row["name"] = name;
            row["coolDownTime"] = coolDownTime;
            row["behaviourAssetPath"] = behaviourAssetPath;
            row["castRadius"] = castRadius;
        }

        public override void FromDataRow(DataRow row)
        {
            this.id = (System.Int32)ParseDataField(row, "id", typeof(System.Int32));
            this.name = (System.String)ParseDataField(row, "name", typeof(System.String));
            this.coolDownTime = (System.Int32)ParseDataField(row, "coolDownTime", typeof(System.Int32));
            this.behaviourAssetPath = (System.String)ParseDataField(row, "behaviourAssetPath", typeof(System.String));
            this.castRadius = (System.Int32)ParseDataField(row, "castRadius", typeof(System.Int32));
        }
        #endregion
    }


	    /// <summary>
    /// 技能
    /// </summary>
    [TableNameAttibute("skill")]
    [Serializable]
    public partial class SkillTable : TableBase<SkillInfo>
    {
        public override string TableName => "skill";

        public SkillTable()
        {

        }

        public SkillTable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}