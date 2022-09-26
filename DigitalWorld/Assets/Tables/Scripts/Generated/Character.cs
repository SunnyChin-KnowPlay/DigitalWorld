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
        public int Id => id;
        private int id;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name => name;
        private string name;
        /// <summary>
        /// 基础血量
        /// </summary>
        public int Hp => hp;
        private int hp;
        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack => attack;
        private int attack;
        /// <summary>
        /// 移动速度
        /// </summary>
        public int MoveSpeed => moveSpeed;
        private int moveSpeed;
        /// <summary>
        /// 预制件路径
        /// </summary>
        public string PrefabPath => prefabPath;
        private string prefabPath;

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