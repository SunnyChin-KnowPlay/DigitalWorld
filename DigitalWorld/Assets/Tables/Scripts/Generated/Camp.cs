using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Xml;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
	    /// <summary>
    /// 阵营
    /// </summary>
    public partial class CampInfo : InfoBase
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

        public override int FieldCount => throw new System.NotImplementedException();

        private System.String name;

        public CampInfo()
        {
        }
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