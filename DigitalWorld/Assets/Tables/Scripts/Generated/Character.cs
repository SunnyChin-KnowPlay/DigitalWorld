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

        public override object GetField(int index)
        {
            throw new System.NotImplementedException();
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
        /// 缩放尺寸
        /// </summary>
        public Dream.FixMath.FixVector3 ScaleSize => scaleSize;

        public override int FieldCount => throw new System.NotImplementedException();

        private Dream.FixMath.FixVector3 scaleSize;

        public CharacterInfo()
        {
        }
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