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
        /// 建筑的配置信息
        /// </summary>
        protected BuildingInfo buildingInfo = null;
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
        /// <summary>
        /// 地图
        /// </summary>
        private MapControl mapControl = null;

        /// <summary>
        /// 启动世界坐标
        /// </summary>
        private Vector3 startWorldPosition = Vector3.zero;
        /// <summary>
        /// 节点队列
        /// </summary>
        private List<PlaceNode> placeNodes = new List<PlaceNode>();
        #endregion

        #region Status
        /// <summary>
        /// 是否正在放置过程中
        /// </summary>
        public bool IsPlacing { get; private set; } = false;
        #endregion

        #region Mono

        protected virtual void OnEnable()
        {
            if (null == buildingInfo)
            {
                this.enabled = false;
                return;
            }

            GameObject go = AssetManager.LoadAsset<GameObject>(buildingInfo.PrefabPath);
            selectedBuildingObject = GameObject.Instantiate(go);
            selectedBuildingTransform = selectedBuildingObject.transform;

            cameraControl = CameraControl.Instance;
            mapControl = WorldManager.Instance.Map;

            Events.EventManager.Instance.RegisterListener(Events.EEventType.Escape, OnEscape);

        }

        protected virtual void OnDisable()
        {
            Events.EventManager.Instance.UnregisterListener(Events.EEventType.Escape, OnEscape);

            if (null != selectedBuildingObject)
            {
                GameObject.Destroy(selectedBuildingObject);
                selectedBuildingObject = null;

                selectedBuildingTransform = null;
            }

            cameraControl = null;
        }

        protected virtual void Update()
        {
            if (InputManager.IsMouseInGameWindow())
            {
                SyncBuildingPosition();

                if (Input.GetMouseButtonDown(0))
                {
                    OnPlaceWillStart();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    OnPlaceDidFinished();
                }

                if (Input.GetMouseButtonUp(1))
                {
                    this.gameObject.SetActive(false);
                }
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

        /// <summary>
        /// 申请开始放置
        /// </summary>
        /// <param name="buildingCfgId"></param>
        public void StartPlace(BuildingInfo buildingInfo)
        {
            this.buildingInfo = buildingInfo;
            this.enabled = true;
        }

        private bool CanPlaceBuildingAtGrids(List<GridControl> targetGrids)
        {
            foreach (GridControl grid in targetGrids)
            {

            }
            return true;
        }
        #endregion

        #region Events
        /// <summary>
        /// 当接收到退出事件时 关闭放置器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        protected virtual void OnEscape(Events.EEventType type, System.EventArgs args)
        {
            this.gameObject.SetActive(false);
        }

        private void OnPlaceWillStart()
        {
            Camera camera = cameraControl.MainCamera;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            // 这里直接发射射线，先看看有没有碰到任何东西
            bool ret = Physics.Raycast(ray, out RaycastHit hit);
            if (ret)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    startWorldPosition = hit.point;
                }
            }
        }

        private void OnPlaceDidFinished()
        {


            this.gameObject.SetActive(false);
        }
        #endregion
    }
}
