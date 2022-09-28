using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace DigitalWorld.Table.Editor
{
    internal static class Utility
    {
        #region Params
        public const string GenerateCodesCmd = "GenerateCodesFromModel";
        public const string GenerateExcelsCmd = "GenerateExcelsFromModel";
        public const string ConvertExcelsToXmlsCmd = "ConvertExcelsToXmls";
        public const string ConvertXmlsToExcelsCmd = "ConvertXmlsToExcels";
        public const string ConvertExcelToXmlCmd = "ConvertExcelToXml";
        public const string ConvertXmlToExcelCmd = "ConvertXmlToExcel";
        #endregion

        #region Common

        /*
        * args[0] = "D:/Projects/DigitalWorld/DigitalWorld-Config/Configs";
                   args[1] = "D:/Projects/DigitalWorld/DigitalWorld-Config/Tables";
                   args[2] = "D:/Projects/DigitalWorld/DigitalWorld-Config/Models/models.xml";
                   args[3] = "D:/Projects/DigitalWorld/DigitalWorld/DigitalWorld/Assets/Tables/Scripts/Generated";
        */
        public static void ExecuteTableGenerate(string cmd, string param = null)
        {
            if (string.IsNullOrEmpty(cmd))
            {
                //TODO:找不到命令
                return;
            }

            string arguments;

            List<string> argList = new List<string>(6)
            {
                Table.Utility.ConfigSrcPath,
                Table.Utility.ExcelTablePath,
                Table.Utility.ModelPath,
                Table.Utility.CodeGeneratedPath,
                cmd,
                param
            };

            arguments = string.Join(' ', argList.ToArray());

            string processFilePath = Path.Combine(Application.dataPath, "Tables/Editor/Plugins/TableGenerator.exe");

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
        public static void ClearDirectory(string path, int index = 0)
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
        public static void CopyDirectory(string src, string tar, string subPath)
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
