using System.Collections.Generic;
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

        /// <summary>
        /// 所有正在编辑的行为
        /// </summary>
        private static Dictionary<string, LogicBehaviourEditorWindow> editingBehaviours = new Dictionary<string, LogicBehaviourEditorWindow>();
        #endregion

        #region Common
        /// <summary>
        /// 检查是否有已打开正在编辑的窗口 如果有的话 直接还出去
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        internal static bool CheckHasEditing(string relativePath, out LogicBehaviourEditorWindow window)
        {
            if (editingBehaviours.ContainsKey(relativePath))
            {
                window = editingBehaviours[relativePath];
                return true;
            }

            window = null;
            return false;
        }

        private void OnDestroy()
        {
            if (null != this.currentBehaviour)
            {
                editingBehaviours.Remove(currentBehaviour.RelativeAssetFilePath);
            }
        }
        #endregion

        #region GUI
        private void SetTitle(string title)
        {
            this.titleContent = new GUIContent(title);
        }

        public void Show(Behaviour behaviour)
        {
            base.Show();
            string fileName = System.IO.Path.GetFileNameWithoutExtension(behaviour.RelativeAssetFilePath);
            this.SetTitle(fileName);

            this.currentBehaviour = behaviour;
            editingBehaviours.Add(behaviour.RelativeAssetFilePath, this);
        }

        private void OnGUI()
        {
            if (null != currentBehaviour)
            {
                fileListScrollViewPos = EditorGUILayout.BeginScrollView(fileListScrollViewPos);
                this.currentBehaviour.OnGUI();
                EditorGUILayout.EndScrollView();

                OnGUIBottomMenus();
            }
        }

        /// <summary>
        /// GUI底部菜单
        /// </summary>
        private void OnGUIBottomMenus()
        {
            GUIStyle style = new GUIStyle("Tooltip");
            EditorGUILayout.BeginHorizontal(style);
            if (GUILayout.Button("Save"))
            {
                this.currentBehaviour.Save();
            }

            if (GUILayout.Button("Close"))
            {
                this.Close();
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion


    }
}
