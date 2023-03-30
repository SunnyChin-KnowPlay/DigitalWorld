using DigitalWorld.Asset;
using DigitalWorld.Logic;
using Dream.Proto;
using Dream.Table;
using Newtonsoft.Json;
using System.Data;
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
        private string GetJsonFilePath(string tableName)
        {
            string folderPath = Utility.defaultConfigJson;
            return string.Format("{0}/{1}.json", folderPath, tableName);
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

        private T ProcessDecodeTableWithJson<T>(string tableName) where T : class 
        {
            T obj = default;
#if UNITY_EDITOR
            string fullPath = GetJsonFilePath(tableName);
            TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(fullPath);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            };

            obj = JsonConvert.DeserializeObject<T>(ta.text, settings);
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
                    formatter.Serialize(stream, table);
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
