using System.IO;
using UnityEngine;

namespace DigitalWorld.Table
{
    public class Utility
    {
        #region Params
        public const string outputCodeKey = "Table.OutputCode";
        public const string defaultOutPutCodePath = "Tables/Scripts/Generated";

        public const string modelKey = "Table.Model";
        public const string defaultModelPath = "D:/Projects/DigitalWorld/DigitalWorld-Config/Models/models.xml";

        public const string excelKey = "Table.Excel";
        public const string defaultExcelPath = "D:/Projects/DigitalWorld/DigitalWorld-Config/Excels";

        public const string configSrcKey = "Table.Config.Src";
        public const string defaultConfigSrc = "D:/Projects/DigitalWorld/DigitalWorld-Config/Xmls";

        public const string configXmlKey = "Table.Config.Xml";
        public const string defaultConfigXml = "Assets/Tables/Config/Xml";

        public const string configDataKey = "Table.Config.Data";
        public const string defaultConfigData = "Assets/Res/Config/Datas";

        public const string defaultNamespaceName = "DigitalWorld.Table";
        /// <summary>
        /// 配置的源文件(config中的)路径
        /// </summary>
        public static string ConfigSrcPath
        {
            get { return Utilities.Utility.GetString(Utility.configSrcKey, defaultConfigSrc); }
        }

        /// <summary>
        /// excel文件的路径
        /// </summary>
        public static string ExcelTablePath
        {
            get { return Utilities.Utility.GetString(excelKey, defaultExcelPath); }
        }

        public static string ModelPath
        {
            get { return Utilities.Utility.GetString(modelKey, defaultModelPath); }
        }

        public static string CodeGeneratedPath
        {
            get { return Utilities.Utility.GetString(outputCodeKey, Path.Combine(Application.dataPath, defaultOutPutCodePath)); }
        }
        #endregion
    }
}
