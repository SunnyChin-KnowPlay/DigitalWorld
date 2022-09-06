using UnityEngine;
using System;
using System.Xml;

namespace DreamEngine.Log
{
    /// <summary>
    /// 日志记录条目
    /// </summary>
    internal class Record
    {
        #region Params
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType LogType { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 系统时间戳
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 自程序开始时的时间
        /// </summary>
        public float TimeSinceBeginning { get; set; }
        #endregion

        #region Construction
        internal Record(LogType logType, string condition, string stackTrace, DateTime time, float timeSinceBeginning)
        {
            LogType = logType;
            Condition = condition;
            StackTrace = stackTrace;
            Time = time;
            TimeSinceBeginning = timeSinceBeginning;
        }
        #endregion

        #region Serialization
        public void Serialize(XmlElement root)
        {
            root.SetAttribute("logType", LogType.ToString());
            root.SetAttribute("condition", Condition);
            root.SetAttribute("time", Time.ToString());
            root.SetAttribute("timeSinceBeginning", TimeSinceBeginning.ToString());

            root.InnerText = string.Format("stackTrace:\r{0}", StackTrace);
        }

        public void Deserialize(XmlElement root)
        {

        }
        #endregion

        //string condition, string stackTrace, LogType type
    }
}
