using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal class LogicBehaviourEditorWindow : EditorWindow
    {
        #region Params
        private Vector2 fileListScrollViewPos = Vector2.zero;

        /// <summary>
        /// 当前正在编辑的行为
        /// </summary>
        private Behaviour currentBehaviour = null;
        #endregion

        #region GUI
        public void Show(Behaviour behaviour)
        {
            base.Show();

            this.currentBehaviour = behaviour;
        }

        private void OnGUI()
        {
            fileListScrollViewPos = EditorGUILayout.BeginScrollView(fileListScrollViewPos);

            if (null != currentBehaviour)
            {
                this.currentBehaviour.OnGUI();
            }

            EditorGUILayout.EndScrollView();
        }
        #endregion

        #region Menus
        [MenuItem("Assets/Create/Logic/Behaviour", priority = 1)]
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
