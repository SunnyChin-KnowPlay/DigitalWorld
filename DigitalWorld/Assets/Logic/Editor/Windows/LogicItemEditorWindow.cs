using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal class LogicItemEditorWindow : EditorWindow
    {
        #region Params
        /// <summary>
        /// 原节点
        /// </summary>
        private NodeItem srcItem;
        /// <summary>
        /// 当前正在编辑的行为
        /// </summary>
        private NodeItem currentItem = null;

        /// <summary>
        /// 所有正在编辑的行为
        /// </summary>
        private static readonly Dictionary<string, LogicItemEditorWindow> editingItems = new Dictionary<string, LogicItemEditorWindow>();

        #endregion


        #region Common
        /// <summary>
        /// 检查是否有已打开正在编辑的窗口 如果有的话 直接还出去
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        internal static bool CheckHasEditing(string name, out LogicItemEditorWindow window)
        {
            if (editingItems.ContainsKey(name))
            {
                window = editingItems[name];
                return true;
            }

            window = null;
            return false;
        }

        private void OnDestroy()
        {
            if (null != this.srcItem)
            {
                editingItems.Remove(srcItem.Name);
                srcItem = null;
            }
        }

        public void Show(NodeItem item)
        {
            base.Show();

            this.srcItem = item;
            this.currentItem = item.Clone() as NodeItem;
            this.SetTitle(item.Name);


            editingItems.Add(item.Name, this);
        }
        #endregion



        #region ONGUI
        private void SetTitle(string title)
        {
            this.titleContent = new GUIContent(title);
        }

        private void OnGUI()
        {

            if (null != currentItem)
            {
                currentItem.OnGUIParams(true);
                currentItem.OnGUIBody();
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Confirm"))
            {
                this.OnClickConfirm();
            }

            if (GUILayout.Button("Cancel"))
            {
                OnClickCancel();
            }

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region Listen
        private void OnClickConfirm()
        {
            _ = this.currentItem.CloneTo(this.srcItem);
            this.srcItem.SetDirty();

            this.currentItem = null;

            EditorWindow.GetWindow<LogicItemsEditorWindow>().Repaint();

            this.Close();
        }

        private void OnClickCancel()
        {
            this.currentItem = null;
            this.Close();
        }
        #endregion
    }
}
