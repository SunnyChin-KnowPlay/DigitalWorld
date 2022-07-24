using Dream.Core;
using System.Collections.Generic;
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
       
        #endregion

        #region Common
        /// <summary>
        /// 开始编辑行为
        /// </summary>
        /// <param name="behaviour"></param>
        public void ApplyEditBehaviour(Behaviour behaviour)
        {

        }
        #endregion
    }
}
