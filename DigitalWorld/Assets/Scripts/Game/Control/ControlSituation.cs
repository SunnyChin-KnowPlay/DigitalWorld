using UnityEngine;
using DigitalWorld.Extension.Unity;

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
