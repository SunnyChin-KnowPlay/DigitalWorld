using DigitalWorld.Asset;
using Dream.Proto;
using System.IO;
using System.Xml;
using UnityEngine;

namespace DigitalWorld.Table
{
    public partial class TableManager
    {
        private TableManager()
        {
            this.OnDecodeTable = OnProcessDecodeTable;
            this.OnDecodeTableWithXml = OnProcessDecodeTableWithXml;
            this.OnEncodeTable = OnProcessEncodeTable;
        }

        #region Utility
        private string GetXmlFilePath(string tableName)
        {
            string folderPath = Utility.defaultConfigXml;
            return string.Format("{0}/{1}.xml", folderPath, tableName);
        }

        private string GetDataFilePath(string tableName)
        {
            string folderPath = Utility.defaultConfigData;
            return string.Format("{0}/{1}.asset", folderPath, tableName);
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
                    ByteAsset ta = AssetManager.LoadAsset<ByteAsset>(path);

                    if (null != ta)
                    {
                        table.Decode(ta.bytes, 0);
                    }
                }
            }
        }

        private void OnProcessDecodeTableWithXml(ByteBuffer table, string tableName)
        {
            if (null != table)
            {
#if UNITY_EDITOR
                string fullPath = GetXmlFilePath(tableName);
                TextAsset ta = UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(TextAsset)) as TextAsset;

                if (null != ta)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(ta.text);
                    XmlElement root = xmlDocument["table"];
                    if (null != root)
                    {
                        table.DecodeXml(root);
                    }
                }
#endif


            }
        }

        private void OnProcessEncodeTable(ByteBuffer table, string tableName)
        {
#if UNITY_EDITOR
            if (null != table)
            {
                int size = table.CalculateSize();
                byte[] data = new byte[size];
                table.Encode(data, 0);

                ByteAsset.CreateAsset(data, GetDataFilePath(tableName));


                //string fullPath = Path.Combine(Application.dataPath,);

                //string path = fullPath;
                //if (!string.IsNullOrEmpty(path))
                //{
                //    string directoryPath = Path.GetDirectoryName(path);
                //    if (!Directory.Exists(directoryPath))
                //    {
                //        Directory.CreateDirectory(directoryPath);
                //    }

                //    using FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                //    fs.Write(data, 0, size);
                //    fs.Flush();
                //    fs.Close();
                //}
            }
#endif
        }
        #endregion
    }
}
