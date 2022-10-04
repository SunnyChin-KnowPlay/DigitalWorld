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
        /// 基础血量
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
        /// 角色的尺寸
        /// </summary>
        public Dream.FixMath.FixVector3 ScaleSize => scaleSize;
        private Dream.FixMath.FixVector3 scaleSize;

        public CharacterInfo()
        {
        }

#region Encode
        protected override void OnEncode()
        {
            base.OnEncode();

            this.Encode(this.id);
            this.Encode(this.name);
            this.Encode(this.hp);
            this.Encode(this.attack);
            this.Encode(this.moveSpeed);
            this.Encode(this.prefabPath);
            this.Encode(this.scaleSize);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this.id, "id");
            this.Encode(this.name, "name");
            this.Encode(this.hp, "hp");
            this.Encode(this.attack, "attack");
            this.Encode(this.moveSpeed, "moveSpeed");
            this.Encode(this.prefabPath, "prefabPath");
            this.Encode(this.scaleSize, "scaleSize");
        }
        #endregion

#region Decode
        protected override void OnDecode()
        {
            base.OnDecode();

            this.Decode(ref this.id);
            this.Decode(ref this.name);
            this.Decode(ref this.hp);
            this.Decode(ref this.attack);
            this.Decode(ref this.moveSpeed);
            this.Decode(ref this.prefabPath);
            this.Decode(ref this.scaleSize);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this.id, "id");
            this.Decode(ref this.name, "name");
            this.Decode(ref this.hp, "hp");
            this.Decode(ref this.attack, "attack");
            this.Decode(ref this.moveSpeed, "moveSpeed");
            this.Decode(ref this.prefabPath, "prefabPath");
            this.Decode(ref this.scaleSize, "scaleSize");
        }
#endregion

#region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this.id);
            this.CalculateSize(this.name);
            this.CalculateSize(this.hp);
            this.CalculateSize(this.attack);
            this.CalculateSize(this.moveSpeed);
            this.CalculateSize(this.prefabPath);
            this.CalculateSize(this.scaleSize);
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