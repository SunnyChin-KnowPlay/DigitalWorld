using Dream.Proto;
using System.IO;
using System.Xml;

namespace DigitalWorld.Table
{
    public partial class TableManager
    {
        public TableManager()
        {
            this.OnDecodeTable = OnProcessDecodeTable;
            this.OnDecodeTableWithXml = OnProcessDecodeTableWithXml;
            this.OnEncodeTable = OnProcessEncodeTable;
        }

        #region Utility
        private string GetXmlFilePath(string tableName)
        {
            return "";
        }
        #endregion

        #region Listen
        /// <summary>
        /// 处理解码表格
        /// 先I/O出数据流并解析到对象
        /// </summary>
        /// <param name="table">表的数据流</param>
        /// <param name="tableName">表名</param>
        private void OnProcessDecodeTable(ByteBuffer table, string tableName)
        {
            if (null != table)
            {
                using (FileStream fs = File.Open(GetXmlFilePath(tableName), FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(fs);
                    XmlElement root = xmlDocument["table"];
                    if (null != root)
                    {
                        table.Decode(root);

                    }
                }
            }
        }

        private void OnProcessDecodeTableWithXml(ByteBuffer table, string tableName)
        {
            if (null != table)
            {

            }
        }

        private void OnProcessEncodeTable(ByteBuffer table, string tableName)
        {
            if (null != table)
            {

            }
        }
        #endregion
    }
}
