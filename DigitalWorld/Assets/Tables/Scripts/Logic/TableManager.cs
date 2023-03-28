using DigitalWorld.Asset;
using DigitalWorld.Logic;
using Dream.Proto;
using Dream.Table;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Table
{
    public partial class TableManager
    {
        private TableManager()
        {
           
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

        #region Process
        private T ProcessDecodeTable<T>(string tableName) where T : class
        {
            string path = GetDataFilePath(tableName);
            ByteAsset asset = AssetManager.LoadAsset<ByteAsset>(path);
            if (null == asset)
                return null;

            using Stream stream = new MemoryStream(asset.bytes);

            BinaryFormatter formatter = new BinaryFormatter();
            T table = formatter.Deserialize(stream) as T;

            return table;
        }

        private T ProcessDecodeTableWithJSON<T>(string tableName) where T : class 
        {
            T obj = default;
#if UNITY_EDITOR
            string fullPath = GetXmlFilePath(tableName);
            TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(fullPath);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            };

            obj = JsonConvert.DeserializeObject<T>(ta.text, settings);

            //Trigger trigger = JsonConvert.DeserializeObject<Trigger>(ta.text, settings);
            //trigger.RelativeFolderPath = System.IO.Path.GetDirectoryName(relativePath);


            //string fullPath = GetXmlFilePath(tableName);
            //TextAsset ta = UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(TextAsset)) as TextAsset;

            //if (null != ta)
            //{
            //    XmlDocument xmlDocument = new XmlDocument();
            //    xmlDocument.LoadXml(ta.text);
            //    XmlElement root = xmlDocument["table"];
            //    if (null != root)
            //    {
            //        table.DecodeXml(root);
            //    }
            //}
#endif
            return obj;
        }

        private void ProcessEncodeTable(object table, string tableName)
        {
#if UNITY_EDITOR
            if (null != table)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    string fullPath = GetDataFilePath(tableName);

                    string directorPath = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(directorPath))
                    {
                        Directory.CreateDirectory(directorPath);
                    }

                    AssetDatabase.DeleteAsset(fullPath);
                    ByteAsset.CreateAsset(stream.GetBuffer(), fullPath);
                }
            }
#endif
        }
        #endregion
    }
}
