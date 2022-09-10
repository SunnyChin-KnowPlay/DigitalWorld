using UnityEngine;
using DigitalWorld.Extension.Unity;
using System.Collections.Generic;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 战场态势控件
    /// </summary>
    public class ControlSituation : ControlLogic
    {
        #region Params
        /// <summary>
        /// 当前锁定目标
        /// </summary>
        public UnitHandle Target => target;
        protected UnitHandle target;

        /// <summary>
        /// 用于识别目标的队列
        /// </summary>
        private readonly List<UnitHandle> selectUnits = new List<UnitHandle>();
        #endregion

        #region Logic
        public virtual void SelectTarget(UnitHandle target)
        {
            if (this.target == target)
            {
                // 如果选择的是同样的目标的话
            }


            this.target = target;
        }

        /// <summary>
        /// 自动搜索目标
        /// 先写一个寻找周围单位的逻辑
        /// 排除掉当前的目标
        /// 选一个在自己面前的(Dot)最近的目标
        /// </summary>
        public virtual void AutoSelectTarget()
        {
            WorldManager wm = WorldManager.Instance;

            selectUnits.Clear();
            wm.AddOtherUnitsToList(this.Unit.Uid, selectUnits);

            if (this.selectUnits.Count == 0)
                return;

            if (this.selectUnits.Count == 1)
            {
                this.SelectTarget(this.selectUnits[0]);
                return;
            }

            // 首先 移除所有不在"正面"的
            for (int i = this.selectUnits.Count - 1; i >= 0; --i)
            {
                bool isInForward = CheckTargetIsInForward(selectUnits[i]);
                if (!isInForward)
                {
                    this.selectUnits.RemoveAt(i);
                }
            }

            // 接下来 根据位置进行排序
            this.selectUnits.Sort(OnSortByDistance);

            this.SelectTarget(this.selectUnits[0]);
        }

        /// <summary>
        /// 根据距离进行排序
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        protected int OnSortByDistance(UnitHandle l, UnitHandle r)
        {
            float distanceL = (l.Unit.LogicPosition - this.Unit.LogicPosition).sqrMagnitude;
            float distanceR = (r.Unit.LogicPosition - this.Unit.LogicPosition).sqrMagnitude;

            return distanceL >= distanceR ? -1 : 1;
        }

        /// <summary>
        /// 检测目标是否在自己的"正面"
        /// Dot值是否大于0
        /// </summary>
        /// <param name="target">目标单位</param>
        /// <returns></returns>
        protected bool CheckTargetIsInForward(UnitHandle target)
        {
            if (default == target)
                return false;
            ControlUnit unit = target.Unit;
            return Vector3.Dot(this.trans.forward, (unit.LogicPosition - Unit.LogicPosition)) > 0;
        }
        #endregion


        #region Gizmos
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if (target)
            {
                this.GizmosDrawCircle(target.Unit.transform, 1f, 0.02f, Color.red);
            }
        }
        #endregion

    }
}
