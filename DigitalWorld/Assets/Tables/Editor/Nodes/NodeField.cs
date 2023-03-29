using System.Xml;
using System;

namespace DigitalWorld.Table.Editor
{
    [Serializable]
    internal class NodeField : NodeBase
    {
        #region Params
        /// <summary>
        /// 字段类型
        /// </summary>
        public System.Type Type { get; set; }
        #endregion

        #region Serialize
        public override void Serialize(XmlElement root)
        {
            base.Serialize(root);

            string typeName = Type.FullName;
            root.SetAttribute("type", typeName);
        }

        public override void Deserialize(XmlElement root)
        {
            base.Deserialize(root);

            string typeName = root.GetAttribute("type");
            Type = Utility.GetType(typeName);
        }
        #endregion


    }
}
