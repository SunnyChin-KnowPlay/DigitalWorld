using DigitalWorld.UI;

namespace DigitalWorld.Game.UI
{
    /// <summary>
    /// 战斗游戏的主面板
    /// </summary>
    public class GamePanel : PanelControl
    {
        #region Enter
        public const string path = "Assets/Res/UI/Game/GamePanel.prefab";
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



            playerUnit = this.GetOrAddWidgetComponent<UnitFramework>("Root/PlayerUnit");
            if (null != playerUnit)
            {
                playerUnit.Bind(WorldManager.Instance.PlayerUnit);
            }

            targetUnit = this.GetOrAddWidgetComponent<UnitFramework>("Root/TargetUnit");
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            this.RegisterListeners();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            this.UnregisterListeners();
        }
        #endregion

        #region Listener
        private void RegisterListeners()
        {
            UnitEventManager m = UnitEventManager.Instance;

            m.AddListener(EUnitEventType.Focused, OnUnitFocused);
        }

        private void UnregisterListeners()
        {
            UnitEventManager m = UnitEventManager.Instance;

            if (null != m)
            {
                m.RemoveListener(EUnitEventType.Focused, OnUnitFocused);
            }
        }

        private void OnUnitFocused(UnitHandle unit, System.EventArgs args)
        {
            UnityEngine.Debug.Log("OnUnitFocused");

            EventArgsTarget target = args as EventArgsTarget;
            targetUnit.Bind(target.Target);
        }
        #endregion
    }

}

