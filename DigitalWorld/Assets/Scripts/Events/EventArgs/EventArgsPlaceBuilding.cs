using DigitalWorld.Table;
using System;
using UnityEngine;

namespace DigitalWorld.Events
{
    internal class EventArgsPlaceBuilding : EventArgs
    {
        /// <summary>
        /// 起始的屏幕位置
        /// </summary>
        public Vector3 screenPosition;
        /// <summary>
        /// 建筑配表条目
        /// </summary>
        public BuildingInfo buildingInfo;

        public EventArgsPlaceBuilding(Vector3 screenPosition, BuildingInfo buildingInfo)
        {
            this.screenPosition = screenPosition;
            this.buildingInfo = buildingInfo;
        }
    }
}
