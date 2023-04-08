using DigitalWorld.Game.Datas;
using Dream.Core;
using Dream.FixMath;
using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 格子控制
    /// </summary>
    public class GridControl : MonoBehaviour
    {
        #region Params
        /// <summary>
        /// 格子的数据
        /// </summary>
        public GridData gridData;
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position => transform.position;

        public bool showGridLines = true; // 控制是否显示网格线

        #endregion

        #region Mono
        protected virtual void Awake()
        {

        }
        #endregion

        #region Setup
        public virtual void Setup(GridData gridData)
        {
            this.gridData = gridData;
        }
        #endregion

        #region Occupied
        /// <summary>
        /// 占领的记录值
        /// </summary>
        protected int occupied;

        public int Occupied => occupied;

        /// <summary>
        /// 占据该层
        /// </summary>
        /// <param name="flag">标记，就是层的按位运算值 1层就是1<<0 2层就是1<<1 以此类推 至多31层(1 << 30)</param>
        public void Occupy(int flag)
        {
            occupied |= flag;
        }

        /// <summary>
        /// 取消占据
        /// </summary>
        /// <param name="flag">标记，就是层的按位运算值 1层就是1<<0 2层就是1<<1 以此类推 至多31层(1 << 30)</param>
        public void Pullout(int flag)
        {
            occupied &= ~flag;
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            occupied = 0;
        }

        /// <summary>
        /// 是否为干净的
        /// </summary>
        public bool IsClear()
        {
            return occupied == 0;
        }

        /// <summary>
        /// 检测该层是否被占用了
        /// </summary>
        /// <param name="flag">标记，就是层的按位运算值 1层就是1<<0 2层就是1<<1 以此类推 至多31层(1 << 30)</param>
        /// <returns></returns>
        public bool IsOccupied(int flag)
        {
            return (occupied & flag) == flag;
        }

        /// <summary>
        /// 是否在任意层上有被占用了
        /// </summary>
        /// <returns>只要有一层被占用了，就返回true不然就是false</returns>
        public bool IsOccupiedFromAnyLayer()
        {
            return occupied != 0;
        }
        #endregion
    }


}
