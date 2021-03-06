using System.IO;
using UnityEngine;

namespace DigitalWorld.Table
{
    public class Utility
    {
        public const string outputCodeKey = "Table.OutputCode";
        public const string defaultOutPutCodePath = "Tables/Scripts/Generated";

        public const string modelKey = "Table.Model";
        public const string defaultModelPath = "D:/Projects/DigitalWorld/DigitalWorld-Config/Models/models.xml";

        public const string excelKey = "Table.Excel";
        public const string defaultExcelPath = "D:/Projects/DigitalWorld/DigitalWorld-Config/Tables";

        public const string configSrcKey = "Table.Config.Src";
        public const string defaultConfigSrc = "D:/Projects/DigitalWorld/DigitalWorld-Config/Configs";

        public const string configXmlKey = "Table.Config.Xml";
        public const string defaultConfigXml = "Tables/Config/Xml";

        public const string configDataKey = "Table.Config.Data";
        public const string defaultConfigData = "Res/Config/Datas";

        public const string GenerateCodesCmd = "GenerateCodesWithModel";
        public const string GenerateTablesCmd = "GenerateTablesWithModel";
        public const string ConvertExcelsToConfigCmd = "ConvertExcelsToConfig";
        public const string ConvertConfigsToExcelCmd = "ConvertConfigsToExcel";

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
    }
}
