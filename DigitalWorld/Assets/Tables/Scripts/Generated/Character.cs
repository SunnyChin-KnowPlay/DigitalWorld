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
    /// 角色
    /// </summary>
    [Serializable]
    public partial class CharacterInfo : InfoBase
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
        /// 血量
        /// </summary>
        public System.Int32 Hp => hp;
        private System.Int32 hp;
        /// <summary>
        /// 攻击力
        /// </summary>
        public System.Int32 Attack => attack;
        private System.Int32 attack;
        /// <summary>
        /// 移动速度
        /// </summary>
        public System.Int32 MoveSpeed => moveSpeed;
        private System.Int32 moveSpeed;
        /// <summary>
        /// 预制件路径
        /// </summary>
        public System.String PrefabPath => prefabPath;
        private System.String prefabPath;
        /// <summary>
        /// 缩放尺寸
        /// </summary>
        public Dream.FixMath.FixVector3 ScaleSize => scaleSize;
        private Dream.FixMath.FixVector3 scaleSize;

        #region Serialization
		public CharacterInfo()
		{

		}

        public CharacterInfo(SerializationInfo info, StreamingContext context)
			: base(info, context)
        {
			this.id = (System.Int32)info.GetValue("id", typeof(System.Int32));
			this.name = (System.String)info.GetValue("name", typeof(System.String));
			this.hp = (System.Int32)info.GetValue("hp", typeof(System.Int32));
			this.attack = (System.Int32)info.GetValue("attack", typeof(System.Int32));
			this.moveSpeed = (System.Int32)info.GetValue("moveSpeed", typeof(System.Int32));
			this.prefabPath = (System.String)info.GetValue("prefabPath", typeof(System.String));
			this.scaleSize = (Dream.FixMath.FixVector3)info.GetValue("scaleSize", typeof(Dream.FixMath.FixVector3));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
			info.AddValue("id", this.id);
			info.AddValue("name", this.name);
			info.AddValue("hp", this.hp);
			info.AddValue("attack", this.attack);
			info.AddValue("moveSpeed", this.moveSpeed);
			info.AddValue("prefabPath", this.prefabPath);
			info.AddValue("scaleSize", this.scaleSize);
        }

        public override void SetupRow(DataRow row)
        {
            row["id"] = id;
            row["name"] = name;
            row["hp"] = hp;
            row["attack"] = attack;
            row["moveSpeed"] = moveSpeed;
            row["prefabPath"] = prefabPath;
            row["scaleSize"] = scaleSize;
        }

        public override void UpdateRow(DataRow row)
        {
            var vvv = row["id"];

            this.id = (System.Int32)row["id"];
            this.name = (System.String)row["name"];
            this.hp = (System.Int32)row["hp"];
            this.attack = (System.Int32)row["attack"];
            this.moveSpeed = (System.Int32)row["moveSpeed"];
            this.prefabPath = (System.String)row["prefabPath"];
            this.scaleSize = (Dream.FixMath.FixVector3)row["scaleSize"];
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