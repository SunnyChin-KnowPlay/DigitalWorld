using Dream.Core;
using UnityEngine;

namespace DigitalWorld.Game
{
    public class GridControl : MonoBehaviour
    {
        #region Params
        /// <summary>
        /// 格子的数据
        /// </summary>
        public GridData gridData;

        public bool showGridLines = true; // 控制是否显示网格线
       
        #endregion

        #region Mono
        void Awake()
        {
           
        }
        #endregion

        #region Setup
        public virtual void Setup(GridData gridData)
        {
            this.gridData = gridData;
        }

        
        #endregion
    }


}
