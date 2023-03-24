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
        #endregion

        #region Mono
        void OnDrawGizmos()
        {
            Color c = Gizmos.color;
            Gizmos.color = Color.white * .5f;
            Gizmos.DrawCube(transform.position, new Vector3(1, 0.2f, 1));
            Gizmos.color = c;
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
