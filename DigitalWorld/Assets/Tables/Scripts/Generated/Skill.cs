using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Xml;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
	    /// <summary>
    /// 技能
    /// </summary>
    public partial class SkillInfo : InfoBase
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

        public SkillInfo()
        {
        }

    }


	    /// <summary>
    /// 技能
    /// </summary>
    [TableNameAttibute("skill")]
    public partial class SkillTable : TableBase<SkillInfo>
    {
        public override string TableName => "skill";
    }
}