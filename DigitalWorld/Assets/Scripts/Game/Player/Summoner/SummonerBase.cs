using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    internal partial class SummonerBase : MonoBehaviour
    {
        #region Params
        /// <summary>
        /// 组件队列
        /// </summary>
        public List<SummonerComBase> Coms => coms;
        protected List<SummonerComBase> coms = new List<SummonerComBase>();
        #endregion

        #region Mono
        protected virtual void Update()
        {
            
        }
        #endregion

        #region Logic
      
        #endregion
    }
}
