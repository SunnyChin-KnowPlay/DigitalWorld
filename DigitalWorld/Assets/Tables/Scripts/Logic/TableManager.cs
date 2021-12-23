using Dream.Proto;
using System.IO;
using System.Xml;
using UnityEngine;

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
            string folderPath = Utilities.Utility.GetString(Utility.configXmlKey, Path.Combine(Application.dataPath, Utility.defaultConfigXml));
            return string.Format("{0}/{1}.xml", folderPath, tableName);
        }

        private string GetDataFilePath(string tableName)
        {
            string folderPath = Utilities.Utility.GetString(Utility.configDataKey, Path.Combine(Application.dataPath, Utility.defaultConfigData));
            return string.Format("{0}/{1}.bytes", folderPath, tableName);
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
                string path = GetDataFilePath(tableName);
                if (!string.IsNullOrEmpty(path))
                {
                    if (File.Exists(path))
                    {
                        using FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                        int size = (int)fs.Length;
                        if (size > 0)
                        {
                            byte[] buffer = new byte[size];
                            size = fs.Read(buffer, 0, size);

                            table.Decode(buffer, 0);
                        }
                    }
                }
            }
        }

        private void OnProcessDecodeTableWithXml(ByteBuffer table, string tableName)
        {
            if (null != table)
            {
                using FileStream fs = File.Open(GetXmlFilePath(tableName), FileMode.Open, FileAccess.Read, FileShare.Read);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fs);
                XmlElement root = xmlDocument["table"];
                if (null != root)
                {
                    table.Decode(root);
                }
            }
        }

        private void OnProcessEncodeTable(ByteBuffer table, string tableName)
        {
            if (null != table)
            {
                int size = table.CalculateSize();
                byte[] data = new byte[size];
                table.Encode(data, 0);

                string path = GetDataFilePath(tableName);
                if (!string.IsNullOrEmpty(path))
                {
                    string directoryPath = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fs.Write(data, 0, size);
                }
            }
        }
        #endregion
    }
}
