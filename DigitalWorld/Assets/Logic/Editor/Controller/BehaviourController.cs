using Dream.Core;
using UnityEditor;

namespace DigitalWorld.Logic.Editor
{
    internal class BehaviourController : Singleton<BehaviourController>
    {
        #region Events
        public delegate void SelectBehaviourHandle(Behaviour behaviour);
        public event SelectBehaviourHandle OnSelectBehaviour;
        #endregion

        #region Params
        public Behaviour CurrentBehaviour => currentBehaviour;
        private Behaviour currentBehaviour = null;

        private static string[] switchLevelTips = new string[3] {
            "当前 {0} 正在编辑，你确定要直接切换？",
            "当前 {0} 正在编辑，你知道你现在在干嘛嘛？！！",
            "不要乱动！冷静！当前 {0} 正在编辑，所以你想怎样？"
        };
        #endregion

        #region Common
        /// <summary>
        /// 选择关卡节点
        /// </summary>
        /// <param name="node"></param>
        public void SelectBehaviourNode(Behaviour behaviour)
        {
            if (currentBehaviour == behaviour)
                return;

            if (null != currentBehaviour)
            {
                string switchTips = string.Format(switchLevelTips[UnityEngine.Random.Range(0, switchLevelTips.Length)], currentBehaviour.Name);
                int ret = EditorUtility.DisplayDialogComplex("警告", switchTips, "保存并切换", "切换", "取消");

                if (ret == 2)
                    return;

                if (ret == 1)
                {
                    currentBehaviour.ResetDirty();
                }
                else
                if (ret == 0)
                {
                    currentBehaviour.Save();
                }

                currentBehaviour.Recycle();
                currentBehaviour = null;
            }

            currentBehaviour = behaviour;
            if (null != OnSelectBehaviour)
            {
                OnSelectBehaviour.Invoke(currentBehaviour);
            }
        }
        #endregion
    }
}
