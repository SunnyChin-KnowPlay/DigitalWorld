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
        private int coolDownTime;
        /// <summary>
        /// CD时间，单位：毫秒
        /// </summary>
        public int CoolDownTime => coolDownTime;
        private string behaviourAssetPath;
        /// <summary>
        /// 行为资产路径
        /// </summary>
        public string BehaviourAssetPath => behaviourAssetPath;
        private int castRadius;
        /// <summary>
        /// 施法半径，单位：毫米
        /// </summary>
        public int CastRadius => castRadius;

        public SkillInfo()
        {
        }

#region Encode
        protected override void OnEncode()
        {
            base.OnEncode();

            this.Encode(this.id);
            this.Encode(this.name);
            this.Encode(this.coolDownTime);
            this.Encode(this.behaviourAssetPath);
            this.Encode(this.castRadius);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this.id, "id");
            this.Encode(this.name, "name");
            this.Encode(this.coolDownTime, "coolDownTime");
            this.Encode(this.behaviourAssetPath, "behaviourAssetPath");
            this.Encode(this.castRadius, "castRadius");
        }
        #endregion

#region Decode
        protected override void OnDecode()
        {
            base.OnDecode();

            this.Decode(ref this.id);
            this.Decode(ref this.name);
            this.Decode(ref this.coolDownTime);
            this.Decode(ref this.behaviourAssetPath);
            this.Decode(ref this.castRadius);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this.id, "id");
            this.Decode(ref this.name, "name");
            this.Decode(ref this.coolDownTime, "coolDownTime");
            this.Decode(ref this.behaviourAssetPath, "behaviourAssetPath");
            this.Decode(ref this.castRadius, "castRadius");
        }
#endregion

#region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this.id);
            this.CalculateSize(this.name);
            this.CalculateSize(this.coolDownTime);
            this.CalculateSize(this.behaviourAssetPath);
            this.CalculateSize(this.castRadius);
        }
#endregion
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