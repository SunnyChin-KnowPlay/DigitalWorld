using DigitalWorld.UI;

namespace DigitalWorld.Game.UI
{
    /// <summary>
    /// 战斗游戏的主面板
    /// </summary>
    public class GamePanel : PanelControl
    {
        #region Enter
        public const string path = "Assets/Res/UI/Game/Panels/GamePanel.prefab";
        #endregion

        #region Params
        /// <summary>
        /// 玩家框架
        /// </summary>
        private UnitFramework playerUnit;
        /// <summary>
        /// 目标框架
        /// </summary>
        private UnitFramework targetUnit;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();



            playerUnit = this.GetOrAddControlComponent<UnitFramework>("Root/PlayerUnit");
            if (null != playerUnit)
            {
                playerUnit.Bind(WorldManager.Instance.PlayerUnit);
            }

            targetUnit = this.GetOrAddControlComponent<UnitFramework>("Root/TargetUnit");
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            this.RegisterEventListeners();
            this.RegisterUnitEventListeners();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            this.UnregisterEventListeners();
            this.UnregisterUnitEventListeners();
        }
        #endregion

        #region Unit Events
        private void RegisterUnitEventListeners()
        {
            UnitEventManager m = UnitEventManager.Instance;

            m.RegisterListener(EUnitEventType.Focused, OnUnitFocused);
        }

        private void UnregisterUnitEventListeners()
        {
            UnitEventManager m = UnitEventManager.Instance;

            if (null != m)
            {
                m.UnregisterListener(EUnitEventType.Focused, OnUnitFocused);
            }
        }

        private void OnUnitFocused(UnitHandle unit, System.EventArgs args)
        {
            UnityEngine.Debug.Log("OnUnitFocused");

            EventArgsTarget target = args as EventArgsTarget;
            targetUnit.Bind(target.Target);
        }
        #endregion

        #region Events
        private void RegisterEventListeners()
        {
            Events.EventManager.Instance.RegisterListener(Events.EEventType.Escape, OnEscapeToShowSetting);
        }

        private void UnregisterEventListeners()
        {
            Events.EventManager.Instance?.UnregisterListener(Events.EEventType.Escape, OnEscapeToShowSetting);
        }

        private void OnEscapeToShowSetting(Events.EEventType eventType, System.EventArgs args)
        {
            UIManager uiManager = UIManager.Instance;
            uiManager.ShowPanel<SettingPanel>(SettingPanel.path);
        }
        #endregion
    }

}

