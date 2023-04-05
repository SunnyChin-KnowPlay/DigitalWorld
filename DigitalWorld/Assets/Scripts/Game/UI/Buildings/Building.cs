using DigitalWorld.Table;
using DigitalWorld.UI;

namespace DigitalWorld.Game.UI.Buildings
{
    /// <summary>
    /// 建筑的Icon控制器
    /// </summary>
    internal class Building : Control
    {
        #region Setup
        public void Setup(BuildingInfo info)
        {
            this.GetControlComponent<TMPro.TMP_Text>("Title").text = info.Name;
        }
        #endregion
    }
}
