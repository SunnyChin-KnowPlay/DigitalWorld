using Dream.Core;
using Dream.Proto;
using Dream.Table;
using System.Xml;
using System.Collections.Generic;

namespace DigitalWorld.Table
{
    /// <summary>
    /// 建筑
    /// </summary>
    public partial class BuildingInfo : InfoBase
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
        /// 占格尺寸
        /// </summary>
        public Dream.FixMath.FixVector3 Size => size;
        private Dream.FixMath.FixVector3 size;
        /// <summary>
        /// 预制件路径
        /// </summary>
        public System.String PrefabPath => prefabPath;
        private System.String prefabPath;

        public BuildingInfo()
        {
        }

     
    }


    /// <summary>
    /// 建筑
    /// </summary>
    [TableNameAttibute("building")]
    public partial class BuildingTable : TableBase<BuildingInfo>
    {
        public override string TableName => "building";
    }
}