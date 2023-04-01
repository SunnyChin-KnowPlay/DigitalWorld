using UnityEngine;
using System.Collections.Generic;
using DigitalWorld.Table;
using DigitalWorld.Asset;
using DigitalWorld.Inputs;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 建筑摆放器
    /// </summary>
    public class BuildingPlacement : MonoBehaviour
    {
        #region Params
        /// <summary>
        /// 建筑的配置ID
        /// </summary>
        protected int buildingCfgId = 0;
        /// <summary>
        /// 正在准备摆放的建筑
        /// </summary>
        protected GameObject selectedBuildingObject = null;
        /// <summary>
        ///  正在准备摆放的建筑的Transform
        /// </summary>
        protected Transform selectedBuildingTransform = null;

        /// <summary>
        /// 相机控制器
        /// </summary>
        private CameraControl cameraControl = null;

        private MapControl mapControl = null;
        #endregion

        #region Mono

        protected virtual void OnEnable()
        {
            BuildingInfo info = TableManager.Instance.BuildingTable[buildingCfgId];
            if (null == info)
            {
                this.enabled = false;
                return;
            }

            GameObject go = AssetManager.LoadAsset<GameObject>(info.PrefabPath);
            selectedBuildingObject = GameObject.Instantiate(go);

            cameraControl = CameraControl.Instance;
            mapControl = WorldManager.Instance.Map;

        }

        protected virtual void OnDisable()
        {
            if (null != selectedBuildingObject)
            {
                GameObject.Destroy(selectedBuildingObject);
                selectedBuildingObject = null;
            }

            cameraControl = null;
        }

        protected virtual void Update()
        {
            if (InputManager.IsMouseInGameWindow())
            {
                SyncBuildingPosition();


            }
        }
        #endregion

        #region Logic
        /// <summary>
        /// 同步建筑的位置，发射线到目标位置
        /// 这里的逻辑将由上至下的判定，先看看碰到的是不是建筑，如果是建筑的话看看是不是同类型的，应该怎么做...
        /// 如果不是建筑的话，则看看是不是地板，地板的话就是准备建造，先只实现建造功能
        /// </summary>
        private void SyncBuildingPosition()
        {
            Camera camera = cameraControl.MainCamera;
            if (null != camera)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                // 这里直接发射射线，先看看有没有碰到任何东西
                bool ret = Physics.Raycast(ray, out RaycastHit hit);
                if (ret)
                {
                    // 如果有东西的话，则要看碰到的东西的层，到底是建筑还是什么，先只考虑Terrain

                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                    {
                        // 这里是碰到地形了
                        // 同步位置

                        GridControl grid = mapControl.GetGrid(hit.point);
                        if (null != grid)
                        {
                            selectedBuildingTransform.position = grid.Position;
                        }
                    }
                }
            }
        }

        public void Setup(int buildingCfgId)
        {
            this.buildingCfgId = buildingCfgId;
        }

        private bool CanPlaceBuildingAtGrids(List<GridControl> targetGrids)
        {
            foreach (GridControl grid in targetGrids)
            {

            }
            return true;
        }
        #endregion
    }
}
