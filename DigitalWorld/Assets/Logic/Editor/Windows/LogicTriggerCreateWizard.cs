using System.Xml;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    /// <summary>
    /// 创建行为
    /// </summary>
    internal class LogicTriggerCreateWizard : ScriptableWizard
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
        public static LogicTriggerCreateWizard DisplayWizard(string path)
        {
            LogicTriggerCreateWizard window = ScriptableWizard.DisplayWizard<LogicTriggerCreateWizard>("Create Trigger");
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

            Trigger behaviour = new Trigger
            {
                Name = this.behaviourName,
                RelativeFolderPath = relativeFilePath
            };

            behaviour.Save();
        }
        #endregion

        #region Menus
        [MenuItem("Assets/Logic/Create/Trigger", priority = 1)]
        private static void CreateTrigger()
        {
            string selectFolderPath = Utility.GetSelectionFolderPath();
            if (string.IsNullOrEmpty(selectFolderPath))
            {
                return;
            }

            DisplayWizard(selectFolderPath);
        }
        #endregion
    }
}
