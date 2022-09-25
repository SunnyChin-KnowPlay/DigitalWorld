using UnityEngine;
using DigitalWorld.Extension.Unity;
using System.Collections.Generic;
using Dream.Core;
using Dream.FixMath;

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
        /// 预选目标队列
        /// </summary>
        private readonly List<UnitHandle> preselectUnits = new List<UnitHandle>();

        /// <summary>
        /// 当前预选队列索引号
        /// </summary>
        private int currentPreselectIndex = 0;

        private float TargetNearestSqrDistance
        {
            get { return 30 * 30; }
        }
        #endregion

        #region Mono
        private void OnEnable()
        {
            currentPreselectIndex = -1;
        }

        private void OnDisable()
        {
            Events.EventManager.Instance?.UnregisterListener(Events.EEventType.Escape, OnUnselectTarget);
        }
        #endregion

        #region Listen
        /// <summary>
        /// 根据距离进行排序
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        protected int OnSortByDistance(UnitHandle l, UnitHandle r)
        {
            int distanceL = Mathf.RoundToInt((l.Unit.LogicPosition - this.Unit.LogicPosition).sqrMagnitude);
            int distanceR = Mathf.RoundToInt((r.Unit.LogicPosition - this.Unit.LogicPosition).sqrMagnitude);

            return distanceL - distanceR;
        }

        private bool OnFilterNeighboursJudgeUnitHandle(UnitHandle unitHandle)
        {
            if (unitHandle == target || unitHandle == this.UnitHandle)
                return false;

            if (unitHandle.Unit.Status != EUnitStatus.Running)
                return false;

            float distance = (unitHandle.Unit.LogicPosition - this.Unit.LogicPosition).sqrMagnitude;
            if (distance > TargetNearestSqrDistance)
                return false;

            for (int i = 0; i < preselectUnits.Count; ++i)
            {
                if (unitHandle == preselectUnits[i])
                    return false;
            }

            bool isInForward = CheckTargetIsInForward(unitHandle);
            if (!isInForward)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 放弃选择目标
        /// </summary>
        /// <param name="args"></param>
        private void OnUnselectTarget(Events.EEventType type, System.EventArgs args)
        {
            this.SelectTargetInternal(UnitHandle.Null);
        }
        #endregion

        #region Logic
        private void SelectTargetInternal(UnitHandle target)
        {
            if (this.target == target)
            {
                // 如果选择的是同样的目标的话
                return;
            }

            this.target = target;
            this.Unit.Event.Invoke(EUnitEventType.Focused, new EventArgsTarget(this.Target));

            if (UnitHandle.Null == this.target)
            {
                this.currentPreselectIndex = -1;
                Events.EventManager.Instance.UnregisterListener(Events.EEventType.Escape, OnUnselectTarget);
            }
            else
            {
                if (Unit.IsPlayerControlling)
                {
                    Events.EventManager.Instance.RegisterListener(Events.EEventType.Escape, OnUnselectTarget);
                }

            }
        }

        private void SelectTargetInternal(int index)
        {
            if (this.preselectUnits.Count < 1)
            {
                this.SelectTargetInternal(UnitHandle.Null);
                return;
            }

            if (index >= this.preselectUnits.Count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = this.preselectUnits.Count - 1;
            }


            UnitHandle target = this.preselectUnits[index];

            if (index == this.currentPreselectIndex || target.Unit.Status != EUnitStatus.Running)
            {
                CalculatePreselects();
                this.SelectTargetInternal(index);
            }
            else
            {
                this.currentPreselectIndex = index;
                SelectTargetInternal(target);
            }
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

        /// <summary>
        /// 计算预选队列
        /// </summary>
        private void CalculatePreselects()
        {
            preselectUnits.Clear();

            WorldManager wm = WorldManager.Instance;
            wm.FilterUnitsToList(this.preselectUnits, OnFilterNeighboursJudgeUnitHandle);

            this.preselectUnits.Sort(OnSortByDistance);
        }

        public virtual void SelectTarget(UnitHandle target)
        {
            int index = -1;
            for (int i = 0; i < preselectUnits.Count; ++i)
            {
                if (target == preselectUnits[i])
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                SelectTargetInternal(index);
            }
            else
            {
                SelectTargetInternal(target);
            }
        }

        public virtual void SelectPrevTarget()
        {
            if (this.currentPreselectIndex < 0 || UnitHandle.Null == this.target)
            {
                CalculatePreselects();
            }

            SelectTargetInternal(this.currentPreselectIndex - 1);
        }

        public virtual void SelectNextTarget()
        {
            if (this.currentPreselectIndex < 0 || UnitHandle.Null == this.target)
            {
                CalculatePreselects();
            }

            SelectTargetInternal(this.currentPreselectIndex + 1);
        }

        /// <summary>
        /// 自动搜索目标
        /// 直接寻找队列中的下一个目标
        /// </summary>
        public virtual void AutoSelectTarget()
        {
            this.SelectNextTarget();
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
