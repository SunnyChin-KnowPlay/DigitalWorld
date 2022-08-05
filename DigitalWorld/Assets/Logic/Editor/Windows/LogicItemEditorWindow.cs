using UnityEditor;
using UnityEngine;
using static DigitalWorld.Logic.Editor.NodeItem;

namespace DigitalWorld.Logic.Editor
{
    internal class LevelItemEditorWindow : EditorWindow
    {
        public enum ShowModeEnum
        {
            Add = 0,
            Edit = 1,
        }


        #region Params
        public ShowModeEnum currentMode;
        private NodeItem initNode;
        private NodeItem currentNode;
        private EItemType type;

        private OnCallbackModifyItem removeHandle;
        private OnCallbackModifyItem addHandle;
        #endregion

        public void Show(EItemType type, OnCallbackModifyItem add)
        {
            base.ShowPopup();

            this.type = type;
            currentMode = ShowModeEnum.Add;
            initNode = null;
            currentNode = NodeController.instance.CreateItem(type);

            currentNode.Id = NodeController.instance.GetNewId(type);

            this.addHandle = add;

            string v = NodeController.GetTitleWithType(type);
            this.titleContent.text = currentMode == ShowModeEnum.Add ? "Create " + v : "Edit " + v;
        }

        public void Show(EItemType type, NodeItem node, OnCallbackModifyItem remove, OnCallbackModifyItem add)
        {
            base.ShowPopup();

            this.type = type;
            currentMode = ShowModeEnum.Edit;
            this.initNode = node;
            this.currentNode = (NodeItem)node.Clone();

            this.removeHandle = remove;
            this.addHandle = add;

            string v = NodeController.GetTitleWithType(type);
            this.titleContent.text = currentMode == ShowModeEnum.Add ? "Create " + v : "Edit " + v;
        }

        #region ONGUI
        private void OnGUI()
        {

            EditorGUILayout.BeginHorizontal();
            string v = NodeController.GetTitleWithType(type);
            EditorGUILayout.LabelField(v + " Type");
            EditorGUILayout.EndHorizontal();

            if (null != currentNode)
            {
                currentNode.OnGUIParams(this.currentMode == ShowModeEnum.Edit);
                currentNode.OnGUIBody();
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Save"))
            {
                this.OnClickSave();
            }

            if (GUILayout.Button("Cancel"))
            {
                this.OnClickCancel();
            }

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region Listen
        private void OnClickSave()
        {
            if (this.currentMode == ShowModeEnum.Add)
            {
                NodeItem exitsNode = null;
                if (NodeController.instance.CheckItemExits(this.type, currentNode.Id, ref exitsNode))
                {
                    string title = NodeController.GetTitleWithType(type);
                    string v = string.Format("{0}已存在，使用该ID({1})的{0}为({2})", title, currentNode.Id, exitsNode.Name);
                    EditorUtility.DisplayDialog("错误，无法保存", v, "Ok");
                    return;
                }

                if (NodeController.instance.CheckItemExits(this.type, currentNode.Name, ref exitsNode))
                {
                    string title = NodeController.GetTitleWithType(type);
                    string v = string.Format("{0}已存在，使用该名字({1})的{0}为({2})", title, currentNode.Name, exitsNode.Name);
                    EditorUtility.DisplayDialog("错误，无法保存", v, "Ok");
                    return;
                }

                if (null != addHandle)
                {
                    addHandle.Invoke(this.type, this.currentNode);
                }
            }
            else
            {
                if (null != removeHandle)
                {
                    removeHandle.Invoke(this.type, this.initNode);
                }
                if (null != addHandle)
                {
                    addHandle.Invoke(this.type, this.currentNode);
                }
            }

            this.currentNode.SetDirty();
            EditorWindow.GetWindow<LogicItemsEditorWindow>().Repaint();

            //ItemController.instance.SortItems(this.type);

            this.Close();
        }

        private void OnClickCancel()
        {
            this.Close();
        }
        #endregion
    }
}
