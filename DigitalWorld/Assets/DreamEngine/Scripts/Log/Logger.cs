using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace DreamEngine.Log
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Logger : Core.Singleton<Logger>
    {
        #region Params
        private readonly List<Record> records = new List<Record>();

        /// <summary>
        /// 自动记录日志
        /// </summary>
        public bool autoRecordLog = true;
        #endregion

        #region Mono
        private void OnEnable()
        {
            Application.logMessageReceived += OnLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= OnLog;

            if (autoRecordLog)
            {
                this.RecordLogToLogs();
            }

            this.records.Clear();
        }
        #endregion

        #region Process
        private void OnLog(string condition, string stackTrace, LogType type)
        {
            Record record = new Record(type, condition, stackTrace, System.DateTime.Now, UnityEngine.Time.time);
            this.records.Add(record);
        }
        #endregion

        #region File
        public void RecordLogToLogs()
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlDeclaration dec = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(dec);

            XmlElement root = xmlDocument.CreateElement("records");
            xmlDocument.AppendChild(root);


            this.EncodeXml(root);

            System.DateTime time = System.DateTime.Now;
            string timeString = time.ToString("yyyy-MM-dd_HH-mm-ss");

            string logFullFilePath = string.Empty;
#if UNITY_EDITOR
            logFullFilePath = Application.dataPath.Replace("/Assets", "");
            logFullFilePath = System.IO.Path.Combine(logFullFilePath, "Logs");
            logFullFilePath += "/" + timeString + ".log";
#endif
            if (!string.IsNullOrEmpty(logFullFilePath))
            {
                xmlDocument.Save(logFullFilePath);
            }
           

        }

        private void EncodeXml(XmlElement root)
        {
            XmlDocument doc = root.OwnerDocument;
            foreach (Record record in this.records)
            {
                XmlElement element = doc.CreateElement("record");
                record.Serialize(element);
                root.AppendChild(element);
            }
        }
        #endregion
        //        #region GUI
        //#if UNITY_EDITOR
        //        [UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
        //        private static bool OnOpenAsset(int instanceID, int line)
        //        {
        //            string stackTrace = GetStackTrace();
        //            if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("Logger.cs"))
        //            {
        //                System.Text.RegularExpressions.Match matches = System.Text.RegularExpressions.Regex.Match(stackTrace, @"\(at (.+)\)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //                string pathLine = "";
        //                while (matches.Success)
        //                {
        //                    pathLine = matches.Groups[1].Value;

        //                    if (!pathLine.Contains("DreamEngine/Scripts/Log/Logger.cs"))
        //                    {
        //                        int splitIndex = pathLine.LastIndexOf(":");
        //                        string path = pathLine.Substring(0, splitIndex);
        //                        line = System.Convert.ToInt32(pathLine.Substring(splitIndex + 1));
        //                        string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
        //                        fullPath = fullPath + path;
        //                        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);
        //                        break;
        //                    }
        //                    matches = matches.NextMatch();
        //                }
        //                return true;
        //            }
        //            return false;
        //        }

        //        private static string GetStackTrace()
        //        {
        //            var ConsoleWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
        //            var fieldInfo = ConsoleWindowType.GetField("ms_ConsoleWindow", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        //            var consoleInstance = fieldInfo.GetValue(null);
        //            if (consoleInstance != null)
        //            {
        //                if ((object)UnityEditor.EditorWindow.focusedWindow == consoleInstance)
        //                {
        //                    fieldInfo = ConsoleWindowType.GetField("m_ActiveText", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //                    string activeText = fieldInfo.GetValue(consoleInstance).ToString();
        //                    return activeText;
        //                }
        //            }
        //            return null;
        //        }
        //#endif
        //        #endregion



        public static void InfoFormat(UnityEngine.Object context, string format, params object[] args)
        {
            LogFormat(LogType.Log, context, format, args);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            LogFormat(LogType.Log, null, format, args);
        }

        public static void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            Debug.unityLogger.LogFormat(logType, context, format, args);
        }

        public static void Info(object message)
        {
            Debug.Log(message);
        }

        public static void Info(object message, UnityEngine.Object context)
        {
            Debug.Log(message, context);
        }

        public static void Warning(object message)
        {
            Debug.LogWarning(message);
        }

        public static void Warning(object message, UnityEngine.Object context)
        {
            Debug.LogWarning(message, context);
        }

        public static void Error(object message)
        {
            Debug.LogError(message);
        }

        public static void Error(object message, UnityEngine.Object context)
        {
            Debug.LogError(message, context);
        }


    }
}
