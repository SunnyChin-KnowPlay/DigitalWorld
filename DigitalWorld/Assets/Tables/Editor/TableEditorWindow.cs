using DigitalWorld.Utilities;
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
        static TableEditorWindow()
        {
            //PlayerPrefs.DeleteAll();

            //Utilities.Utility.SetDefaultString(Utility.outputCodeKey, Path.Combine(Application.dataPath, Utility.defaultOutPutCodePath));
            //Utilities.Utility.SetDefaultString(Utility.modelKey, Utility.defaultModelPath);
            //Utilities.Utility.SetDefaultString(Utility.excelKey, Utility.defaultExcelPath);
            //Utilities.Utility.SetDefaultString(Utility.configXmlKey, Path.Combine(Application.dataPath, Utility.defaultConfigXml));
            //Utilities.Utility.SetDefaultString(Utility.configDataKey, Path.Combine(Application.dataPath, Utility.defaultConfigData));
        }

        #region MenuItems
        [MenuItem("Table/Generate/Generate Codes")]
        private static void GenerateCodes()
        {
            ExecuteTableGenerate(Utility.GenerateCodesCmd);

            AssetDatabase.Refresh();
        }

        [MenuItem("Table/Generate/Generate Tables")]
        private static void GenerateTablesWithModel()
        {
            ExecuteTableGenerate(Utility.GenerateTablesCmd);
        }

        [MenuItem("Table/Convert/ConvertExcelsToConfig")]
        private static void ConvertExcelsToConfig()
        {
            ExecuteTableGenerate(Utility.ConvertExcelsToConfigCmd);
        }

        [MenuItem("Table/Convert/ConvertConfigsToExcel")]
        private static void ConvertConfigsToExcel()
        {
            ExecuteTableGenerate(Utility.ConvertConfigsToExcelCmd);
        }

        [MenuItem("Table/Convert/ConvertXmlToBytes")]
        private static void ConvertXmlToBytes()
        {
            string bytesDirectory = Utilities.Utility.GetString(Utility.configDataKey, Path.Combine(Utilities.Utility.GetProjectDataPath(), Utility.defaultConfigData));
            if (!string.IsNullOrEmpty(bytesDirectory))
            {
                if (!Directory.Exists(bytesDirectory))
                    Directory.CreateDirectory(bytesDirectory);
                else
                {
                    ClearDirectory(bytesDirectory);
                }
            }

            TableManager m = TableManager.Instance;
            m.DecodeXml();
            m.Encode();

            AssetDatabase.Refresh();
        }

        [MenuItem("Table/CopyXmlFromConfig")]
        private static void CopyXmlFromConfig()
        {
            string targetPath = Utilities.Utility.GetString(Utility.configXmlKey, Path.Combine(Utilities.Utility.GetProjectDataPath(), Utility.defaultConfigXml));
            ClearDirectory(targetPath);

            CopyDirectory(Utilities.Utility.GetString(Utility.configSrcKey, Utility.defaultConfigSrc), targetPath, null);
            AssetDatabase.Refresh();
        }

        [MenuItem("Table/Auto Process")]
        private static void AutoProcess()
        {
            GenerateCodes();

            CopyXmlFromConfig();
            ConvertXmlToBytes();
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
                Utility.ConfigSrcPath,
                Utility.ExcelTablePath,
                Utility.ModelPath,
                Utility.CodeGeneratedPath,
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

