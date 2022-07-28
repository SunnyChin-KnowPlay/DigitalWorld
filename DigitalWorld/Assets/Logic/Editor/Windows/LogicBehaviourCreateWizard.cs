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
            behaviour.RelativeFolderPath = relativeFilePath;

            behaviour.Save();
        }
        #endregion

        #region Menus
        [MenuItem("Assets/Logic/Create/Behaviour", priority = 1)]
        private static void CreateBehaviour()
        {
            string selectFolderPath = Utility.GetSelectionFolderPath();
            if (string.IsNullOrEmpty(selectFolderPath))
            {
                return;
            }

            LogicBehaviourCreateWizard.DisplayWizard(selectFolderPath);
        }
        #endregion
    }
}
