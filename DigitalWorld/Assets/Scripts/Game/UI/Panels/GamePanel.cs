using DigitalWorld.Events;
using DigitalWorld.UI;
using System;
using UnityEngine.UI;

namespace DigitalWorld.Game.UI
{
    /// <summary>
    /// ս����Ϸ�������
    /// </summary>
    public class GamePanel : PanelControl
    {
        #region Enter
        public const string path = "Assets/Res/UI/Game/Panels/GamePanel.prefab";
        #endregion

        #region Params
        /// <summary>
        /// ��ҿ��
        /// </summary>
        private UnitFramework playerUnit;
        /// <summary>
        /// Ŀ����
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
            this.RegisterUIEventListeners();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            this.UnregisterEventListeners();
            this.UnregisterUnitEventListeners();
            this.UnregisterUIEventListeners();
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
            EventArgsTarget target = args as EventArgsTarget;
            targetUnit.Bind(target.Target);
        }
        #endregion

        #region UI Events
        private void RegisterUIEventListeners()
        {
            Button button = this.GetControlComponent<Button>("Root/SettingButton");
            if (null != button)
            {
                button.onClick.AddListener(OnClickSetting);
            }
        }

        private void UnregisterUIEventListeners()
        {
            Button button = this.GetControlComponent<Button>("Root/SettingButton");
            if (null != button)
            {
                button.onClick.RemoveListener(OnClickSetting);
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        private void OnClickSetting()
        {
            UIManager uiManager = UIManager.Instance;
            uiManager.ShowPanel<SettingPanel>(SettingPanel.path);
        }
        #endregion

        #region Events
        private void RegisterEventListeners()
        {
            // ע���˳��¼� 
            Events.EventManager.Instance.RegisterListener(Events.EEventType.Escape, OnEscape);
        }

        private void UnregisterEventListeners()
        {
            Events.EventManager.Instance?.UnregisterListener(Events.EEventType.Escape, OnEscape);
        }

        /// <summary>
        /// Game��岻���˳� �յ�Escape�¼�ʱ���Ǵ��������
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        protected override void OnEscape(EEventType type, EventArgs args)
        {
            UIManager uiManager = UIManager.Instance;
            uiManager.ShowPanel<SettingPanel>(SettingPanel.path);
        }
        #endregion
    }

}

