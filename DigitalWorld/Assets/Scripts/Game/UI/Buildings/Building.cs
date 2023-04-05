using DigitalWorld.Table;
using DigitalWorld.UI;
using UnityEngine.UI;

namespace DigitalWorld.Game.UI.Buildings
{
    /// <summary>
    /// 建筑的Icon控制器
    /// </summary>
    internal class Building : Control
    {
        #region Events
        public delegate void OnClickBuildingHandle(Building building);
        private OnClickBuildingHandle OnClickBuilding;
        #endregion

        #region Params
        private Button button;
        public BuildingInfo BuildingInfo { get; private set; } = null;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            button = this.GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
        #endregion

        #region Listen
        private void OnClick()
        {
            OnClickBuilding?.Invoke(this);
        }
        #endregion

        #region Setup
        public void Setup(BuildingInfo info, OnClickBuildingHandle handle = null)
        {
            this.OnClickBuilding = handle;
            BuildingInfo = info;
            this.GetControlComponent<TMPro.TMP_Text>("Title").text = info.Name;
        }
        #endregion
    }
}
