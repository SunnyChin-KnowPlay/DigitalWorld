using DigitalWorld.Table;
using DigitalWorld.UI;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game.UI.Buildings
{
    public class BuildingContent : Control
    {
        #region Params
        private readonly Dictionary<int, Building> buildings = new Dictionary<int, Building>();
        private GameObject iconObject = null;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            iconObject = FirstWidget.GetObject("Building");
            iconObject.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            this.Refresh();
        }
        #endregion

        #region Logic
        private void Refresh()
        {
            this.ClearBuildings();

            BuildingTable bt = TableManager.Instance.BuildingTable;
            foreach (KeyValuePair<int, BuildingInfo> kvp in bt.Infos)
            {
                GameObject obj = GameObject.Instantiate(this.iconObject);
                if (!obj.TryGetComponent<Building>(out Building building))
                {
                    building = obj.AddComponent<Building>();
                }
                obj.transform.SetParent(this.FirstWidget.RectTransform, false);
                obj.SetActive(true);
                building.Setup(kvp.Value, OnClickBuilding);
            }
        }

        private void ClearBuildings()
        {
            foreach (var building in buildings.Values)
            {
                GameObject.Destroy(building.gameObject);
            }
            buildings.Clear();
        }
        #endregion

        #region Listen
        private void OnClickBuilding(Building building)
        {
            Events.EventArgsPlaceBuilding args = new Events.EventArgsPlaceBuilding(Vector3.zero, building.BuildingInfo);
            Events.EventManager.Instance.Invoke(Events.EEventType.Building_StartPlace, args);
        }
        #endregion
    }
}
