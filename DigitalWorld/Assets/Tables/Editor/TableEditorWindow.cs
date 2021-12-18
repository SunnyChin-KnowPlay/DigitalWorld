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
        private const string defaultConfigData = "Res/Config/Datas";

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
            Utility.SetDefaultString(configXmlKey, Path.Combine(Application.dataPath, defaultConfigXml));
            Utility.SetDefaultString(configDataKey, Path.Combine(Application.dataPath, defaultConfigData));
        }

        #region MenuItems
        [MenuItem("Table/Generate/Generate Codes")]
        private static void GenerateCodes()
        {
            ExecuteTableGenerate(GenerateCodesCmd);
           
            AssetDatabase.Refresh();
        }

        [MenuItem("Table/Generate/Generate Tables")]
        private static void GenerateTablesWithModel()
        {
            ExecuteTableGenerate(GenerateTablesCmd);
        }

        [MenuItem("Table/Convert/ConvertExcelsToConfig")]
        private static void ConvertExcelsToConfig()
        {
            ExecuteTableGenerate(ConvertExcelsToConfigCmd);
        }

        [MenuItem("Table/Convert/ConvertConfigsToExcel")]
        private static void ConvertConfigsToExcel()
        {
            ExecuteTableGenerate(ConvertConfigsToExcelCmd);
        }

        [MenuItem("Table/CopyXmlFromConfig")]
        private static void CopyXmlFromConfig()
        {
            string targetPath = Utility.GetString(configXmlKey, Path.Combine(Application.dataPath, defaultConfigXml));
            ClearDirectory(targetPath);

            CopyDirectory(Utility.GetString(configSrcKey, defaultConfigSrc), targetPath, null);
            AssetDatabase.Refresh();
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

        /// <summary>
        /// 清空文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="index"></param>
        private static void ClearDirectory(string path, int index = 0)
        {
            if (!Directory.Exists(path))
                return;

            string[] directories = Directory.GetDirectories(path);
            for (int i = 0; i < directories.Length; ++i)
            {
                ClearDirectory(directories[i], index + 1);
            }

            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; ++i)
            {
                File.Delete(files[i]);
            }

            if (index > 0)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path);
                }
            }
        }

        /// <summary>
        /// 深度拷贝目录
        /// </summary>
        /// <param name="src"></param>
        /// <param name="tar"></param>
        /// <param name="subPath"></param>
        private static void CopyDirectory(string src, string tar, string subPath)
        {
            if (string.IsNullOrEmpty(src) || string.IsNullOrEmpty(tar))
            {
                UnityEngine.Debug.LogError("TableErr: CopyDirectory path is err");
                return;
            }

            if (!Directory.Exists(src))
            {
                UnityEngine.Debug.LogError("TableErr: CopyDirectory src is not found.\t" + src);
                return;
            }

            string[] directories = Directory.GetDirectories(src);
            for (int i = 0; i < directories.Length; ++i)
            {
                string dir = directories[i];

                string sPath = dir.Replace(src, "");
                CopyDirectory(src, tar, sPath);
            }

            string[] files = Directory.GetFiles(src);
            for (int i = 0; i < files.Length; ++i)
            {
                string fullTarPath;
                if (string.IsNullOrEmpty(subPath))
                    fullTarPath = tar;
                else
                    fullTarPath = Path.Combine(tar, subPath);

                fullTarPath = Path.Combine(fullTarPath, Path.GetFileName(files[i]));

                File.Copy(files[i], fullTarPath);
            }

        }
        #endregion


    }

}

