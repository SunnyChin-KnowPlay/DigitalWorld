using System.Xml;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    /// <summary>
    /// 创建行为
    /// </summary>
    internal class LogicBehaviourCreateWizard : ScriptableWizard
    {
        #region Params
        /// <summary>
        /// 目标文件夹路径
        /// </summary>
        private string targetFolderPath;

        /// <summary>
        /// 行为名
        /// </summary>
        public string behaviourName;
        #endregion

        #region Window
        public static LogicBehaviourCreateWizard DisplayWizard(string path)
        {
            LogicBehaviourCreateWizard window = ScriptableWizard.DisplayWizard<LogicBehaviourCreateWizard>("Create Behaviour");
            if (null != window)
            {
                window.Show(path);
            }
            return window;
        }
        #endregion

        #region Common
        public void Show(string path)
        {
            this.targetFolderPath = path;
        }
        #endregion

        #region OnGUI
        private void OnWizardCreate()
        {
            string fullPath = targetFolderPath;

            string relativeFilePath = fullPath.Substring(DigitalWorld.Logic.Utility.ConfigsPath.Length + 1);

            Behaviour behaviour = new Behaviour();
            behaviour.Name = this.behaviourName;
            behaviour.RelativeFilePath = relativeFilePath;

            behaviour.Save();

            // 现在写到创建了，明天继续写读取后编辑随后保存的功能
            // 需要写Selection监听选择并编辑的功能
        }
        #endregion
    }
}
