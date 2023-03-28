using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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

        public static System.Type GetType(string typeName)
        {
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in assemblies)
            {
                System.Type[] types = asm.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.FullName == typeName)
                        return type;
                }
            }

            return null;
        }
        #endregion

        #region Process
        /// <summary>
        /// Find out what process(es) have a lock on the specified file.
        /// </summary>
        /// <param name="path">Path of the file.</param>
        /// <returns>Processes locking the file</returns>
        /// <remarks>See also:
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa373661(v=vs.85).aspx
        /// http://wyupdate.googlecode.com/svn-history/r401/trunk/frmFilesInUse.cs (no copyright in code at time of viewing)
        /// </remarks>
        public static List<Process> GetProcessesLockingFile(string path)
        {
            uint handle;
            string key = Guid.NewGuid().ToString();
            int res = RmStartSession(out handle, 0, key);

            if (res != 0) throw new Exception("Could not begin restart session.  Unable to determine file locker.");

            try
            {
                const int MORE_DATA = 234;
                uint pnProcInfoNeeded, pnProcInfo = 0, lpdwRebootReasons = RmRebootReasonNone;

                string[] resources = { path }; // Just checking on one resource.

                res = RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);

                if (res != 0) throw new Exception("Could not register resource.");

                //Note: there's a race condition here -- the first call to RmGetList() returns
                //      the total number of process. However, when we call RmGetList() again to get
                //      the actual processes this number may have increased.
                res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);

                if (res == MORE_DATA)
                {
                    return EnumerateProcesses(pnProcInfoNeeded, handle, lpdwRebootReasons);
                }
                else if (res != 0) throw new Exception("Could not list processes locking resource. Failed to get size of result.");
            }
            finally
            {
                RmEndSession(handle);
            }

            return new List<Process>();
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct RM_UNIQUE_PROCESS
        {
            public int dwProcessId;
            public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }

        const int RmRebootReasonNone = 0;
        const int CCH_RM_MAX_APP_NAME = 255;
        const int CCH_RM_MAX_SVC_NAME = 63;

        public enum RM_APP_TYPE
        {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct RM_PROCESS_INFO
        {
            public RM_UNIQUE_PROCESS Process;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)] public string strAppName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)] public string strServiceShortName;

            public RM_APP_TYPE ApplicationType;
            public uint AppStatus;
            public uint TSSessionId;
            [MarshalAs(UnmanagedType.Bool)] public bool bRestartable;
        }

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        static extern int RmRegisterResources(uint pSessionHandle, uint nFiles, string[] rgsFilenames,
            uint nApplications, [In] RM_UNIQUE_PROCESS[] rgApplications, uint nServices,
            string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
        static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        static extern int RmEndSession(uint pSessionHandle);

        [DllImport("rstrtmgr.dll")]
        static extern int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded,
            ref uint pnProcInfo, [In, Out] RM_PROCESS_INFO[] rgAffectedApps,
            ref uint lpdwRebootReasons);

        private static List<Process> EnumerateProcesses(uint pnProcInfoNeeded, uint handle, uint lpdwRebootReasons)
        {
            var processes = new List<Process>(10);
            // Create an array to store the process results
            var processInfo = new RM_PROCESS_INFO[pnProcInfoNeeded];
            var pnProcInfo = pnProcInfoNeeded;

            // Get the list
            var res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);

            if (res != 0) throw new Exception("Could not list processes locking resource.");
            for (int i = 0; i < pnProcInfo; i++)
            {
                try
                {
                    processes.Add(Process.GetProcessById(processInfo[i].Process.dwProcessId));
                }
                catch (ArgumentException) { } // catch the error -- in case the process is no longer running
            }
            return processes;
        }
        #endregion
    }
}
