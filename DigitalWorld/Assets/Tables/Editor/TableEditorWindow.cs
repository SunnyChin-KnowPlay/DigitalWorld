using DigitalWorld.Utilities.Editor;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Table.Editor
{
    [InitializeOnLoad]
    public class TableEditorWindow : EditorWindow
    {
        /*
         *  if (cmd == "GenerateCodesWithModel")
            {
                Generator.GenerateCodesWithModel(modelPath, codePath);
            }
            else if (cmd == "GenerateTablesWithModel")
            {
                helper.GenerateTablesWithModel(modelPath, tablePath);
            }
            else if (cmd == "ConvertExcelsToConfig")
            {
                helper.ConvertExcelsToConfig(tablePath, configPath);
            }
            else if (cmd == "ConvertConfigsToExcel")
            {
                helper.ConvertConfigsToExcel(configPath, tablePath);
            }
         */


        private const string outputCodeKey = "Table.OutputCode";
        private const string defaultOutPutCodePath = "Tables/Scripts/Generated";

        private const string modelKey = "Table.Model";
        private const string defaultModelPath = "D:/Projects/DigitalWorld/DigitalWorld-Config/Models/models.xml";

        private const string excelKey = "Table.Excel";
        private const string defaultExcelPath = "D:/Projects/DigitalWorld/DigitalWorld-Config/Tables";

        private const string configSrcKey = "Table.Config.Src";
        private const string defaultConfigSrc = "D:/Projects/DigitalWorld/DigitalWorld-Config/Configs";

        private const string configXmlKey = "Table.Config.Xml";
        private const string defaultConfigXml = "Tables/Config/Xml";

        private const string configDataKey = "Table.Config.Data";
        private const string defaultConfigData = "Tables/Config/Data";

        private const string GenerateCodesCmd = "GenerateCodesWithModel";
        private const string GenerateTablesCmd = "GenerateTablesWithModel";
        private const string ConvertExcelsToConfigCmd = "ConvertExcelsToConfig";
        private const string ConvertConfigsToExcelCmd = "ConvertConfigsToExcel";

        /// <summary>
        /// 配置的源文件(config中的)路径
        /// </summary>
        public static string ConfigSrcPath
        {
            get { return Utility.GetString(configSrcKey, defaultConfigSrc); }
        }

        /// <summary>
        /// excel文件的路径
        /// </summary>
        public static string ExcelTablePath
        {
            get { return Utility.GetString(excelKey, defaultExcelPath); }
        }

        public static string ModelPath
        {
            get { return Utility.GetString(modelKey, defaultModelPath); }
        }

        public static string CodeGeneratedPath
        {
            get { return Utility.GetString(outputCodeKey, defaultOutPutCodePath); }
        }

        static TableEditorWindow()
        {
            Utility.SetDefaultString(outputCodeKey, defaultOutPutCodePath);
            Utility.SetDefaultString(modelKey, defaultModelPath);
            Utility.SetDefaultString(excelKey, defaultExcelPath);
        }

        #region MenuItems
        [MenuItem("Table/Generate Codes")]
        private static void GenerateCodes()
        {
            ExecuteTableGenerate(GenerateCodesCmd);
            //string codePath = Path.Combine(Application.dataPath, Utility.GetString(outputCodeKey));
            //string modelPath = Utility.GetString(modelKey);

            //Generator.GenerateCodesWithModel(modelPath, codePath);

            AssetDatabase.Refresh();
        }

        [MenuItem("Table/Generate Tables")]
        private static void GenerateTablesWithModel()
        {
            ExecuteTableGenerate(GenerateTablesCmd);
        }
        #endregion

        #region Common

        /*
        * args[0] = "D:/Projects/DigitalWorld/DigitalWorld-Config/Configs";
                   args[1] = "D:/Projects/DigitalWorld/DigitalWorld-Config/Tables";
                   args[2] = "D:/Projects/DigitalWorld/DigitalWorld-Config/Models/models.xml";
                   args[3] = "D:/Projects/DigitalWorld/DigitalWorld/DigitalWorld/Assets/Tables/Scripts/Generated";
        */
        private static void ExecuteTableGenerate(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
            {
                //TODO:找不到命令
                return;
            }

            string arguments;

            List<string> argList = new List<string>(5)
            {
                ConfigSrcPath,
                ExcelTablePath,
                ModelPath,
                CodeGeneratedPath,
                cmd
            };

            arguments = string.Join(' ', argList.ToArray());

            string processFilePath = Path.Combine(Application.dataPath, "Tables/Editor/TableGenerator.exe");

            Process process = new Process();
            ProcessStartInfo processStartInfo = new ProcessStartInfo(processFilePath)
            {
                Arguments = arguments,
                UseShellExecute = true
            };
            process.StartInfo = processStartInfo;

            try
            {
                _ = process.Start();
                process.WaitForExit();
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
            }
            finally
            {
                process.Close();
            }



        }
        #endregion


    }

}

