using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Game
{
    public class BuildingManager : UnitManager
    {
        #region Override
        public override string Name => "Buildings";
        public override EUnitType Type => EUnitType.Building;

        /// <summary>
        /// 放置器
        /// </summary>
        public BuildingPlacement Placement { get; private set; }
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();
            Placement = this.gameObject.AddComponent<BuildingPlacement>();
            Placement.enabled = false;
        }
        #endregion
    }
}
