using DigitalWorld.Extension.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 建筑的控制器
    /// </summary>
    [Serializable]
    public class BuildingControl : UnitControl
    {
        #region Params
        /// <summary>
        /// 占用的格子队列
        /// </summary>
        public List<GridControl> OccupiedGrids => occupieGrids;
        private readonly List<GridControl> occupieGrids = new List<GridControl>();

        public override EUnitType Type => EUnitType.Building;
        #endregion

        #region Setup
        protected override void SetupControls()
        {
            if (null == this.controls)
            {
                this.controls = new Dictionary<ELogicControlType, ControlLogic>
                {
                    { ELogicControlType.Property, this.GetOrAddComponent<ControlProperty>() },
                    { ELogicControlType.Animator, this.GetOrAddComponent<ControlAnimator>() },
                    { ELogicControlType.Skill, this.GetOrAddComponent<ControlSkill>() },
                    { ELogicControlType.Trigger, this.GetOrAddComponent<ControlTrigger>() },
                    { ELogicControlType.Situation, this.GetOrAddComponent<ControlSituation>() },
                    { ELogicControlType.Calculate, this.GetOrAddComponent<ControlCalculate>() },
                    { ELogicControlType.Event, this.GetOrAddComponent<ControlEvent>() },
                };
            }

            this.EachAllControls(OnSetupControl);
        }
        #endregion


    }
}
