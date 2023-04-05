using DigitalWorld.Events;

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

        private void OnEnable()
        {
            EventManager.Instance.RegisterListener(EEventType.Building_StartPlace, OnBuilding_StartPlace);
        }

        private void OnDisable()
        {
            EventManager.Instance.UnregisterListener(EEventType.Building_StartPlace, OnBuilding_StartPlace);
        }
        #endregion

        #region Events
        private void OnBuilding_StartPlace(Events.EEventType type, System.EventArgs args)
        {
            if (args is EventArgsPlaceBuilding buildingArgs)
            {
                if (Placement.enabled)
                    Placement.enabled = false;

                Placement.StartPlace(buildingArgs.buildingInfo.Id);
            }
        }
        #endregion
    }
}
