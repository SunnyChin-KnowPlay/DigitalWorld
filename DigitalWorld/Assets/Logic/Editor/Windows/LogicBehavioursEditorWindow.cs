using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    /// <summary>
    /// 行为组管理窗口
    /// </summary>
    internal class LogicBehavioursEditorWindow : EditorWindow
    {
        #region Params
        /// <summary>
        /// 编辑中的行为
        /// </summary>
        private List<Behaviour> editingBehaviours = new List<Behaviour>();
        public List<Behaviour> EditingBehaviours { get => editingBehaviours; }

        /// <summary>
        /// 渲染GUI时运行的行为，因为中间可能有增减，必须先放到一个队列中处理才行
        /// </summary>
        private List<Behaviour> runningBehaviours = new List<Behaviour>();
        /// <summary>
        /// 正在编辑中的行为
        /// </summary>
        private Behaviour editingBehaviour = null;
        #endregion

        #region GUI
        public new void Show()
        {
            Behaviour behaviour = new Behaviour();
            behaviour.Name = "123";

            this.editingBehaviours.Add(behaviour);
        }
        private void OnGUI()
        {
            this.runningBehaviours.Clear();
            this.runningBehaviours.AddRange(editingBehaviours);

            GUILayout.BeginHorizontal();
            // 首先 把top上的tab菜单渲染一遍 决定当前正在编辑的行为
            for (int i = 0; i < runningBehaviours.Count; ++i)
            {
                Behaviour behaviour = runningBehaviours[i];
                GUILayout.Button(behaviour.Name);
            }
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}
