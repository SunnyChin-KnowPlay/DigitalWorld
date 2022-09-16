using DigitalWorld.UI;

namespace DigitalWorld.Game.UI
{
    /// <summary>
    /// ս����Ϸ�������
    /// </summary>
    public class GamePanel : PanelControl
    {
        #region Enter
        public const string path = "Assets/Res/UI/Game/GamePanel.prefab";
        #endregion

        #region Params
        private UnitFramework playerUnit;
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
        }
        #endregion
    }

}

