﻿using DigitalWorld.Asset;
using DigitalWorld.Behaviour;
using DigitalWorld.Proto.Game;
using DigitalWorld.Table;
using DigitalWorld.TileMap;
using Dream.Extension.Unity;
using DreamEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DigitalWorld.Game
{
    public sealed class WorldManager : Singleton<WorldManager>
    {
        #region Params
        public Camera MainCamera
        {
            get
            {
                return mainCamera;
            }
        }
        private Camera mainCamera = null;

        /// <summary>
        /// 地图
        /// </summary>
        public TileMapControl TileMapControl
        {
            get { return tileMapControl; }
        }

        private TileMapControl tileMapControl = null;

        /// <summary>
        /// 单位词典
        /// </summary>
        private readonly Dictionary<uint, ControlUnit> units = new Dictionary<uint, ControlUnit>();

        /// <summary>
        /// 单位id池
        /// </summary>
        private uint unitIdPool = 0;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            mainCamera = Camera.main;
        }

        private void Start()
        {
            this.Setup();
        }

        private void Setup()
        {
            unitIdPool = 0;
            this.units.Clear();

            this.SetupMap();
        }

        private void SetupMap()
        {
            string fullPath = "Res/Map/Map.prefab";

            GameObject gameObject = null;

            Object target = AssetManager.LoadAsset<Object>(fullPath);

            if (null != target)
            {
                gameObject = (GameObject)Instantiate(target);
                if (null != gameObject)
                {
                    gameObject.name = target.name;
                    Transform t = gameObject.transform;
                    t.position = Vector3.zero;
                }
            }

            if (null != gameObject)
            {
                tileMapControl = gameObject.GetOrAddComponent<TileMapControl>();
                if (null != tileMapControl)
                {
                    string fileName = string.Format("{0}.bytes", 1);
                    string path = Path.Combine(TileMapControl.defaultMapDataPath, fileName);

                    TextAsset mapTA = AssetManager.LoadAsset<TextAsset>(path);
                    MapData data = new MapData();
                    data.Decode(mapTA.bytes, 0);

                    tileMapControl.Setup(data);
                }
            }
        }

        public ControlTile CreateTile(TileData tileData)
        {
            TilebaseInfo info = tileData.TilebaseInfo;
            if (null == info)
                return null;

            string path = string.Format("{0}.prefab", info.prefabPath);
            GameObject obj = AssetManager.LoadAsset<GameObject>(path);
            if (null == obj)
                return null;

            GameObject go = GameObject.Instantiate(obj) as GameObject;
            ControlTile tile = ControlTile.GetOrAddControl(go, (ETileType)tileData.tileBaseId);
            UnitData unitData = new UnitData()
            {
                configId = tileData.tileId,
                unitType = EUnitType.Tile,
                level = 1,
            };

            this.RegisterUnit(tile, unitData);
            return tile;
        }

        private uint GetNewUnitId()
        {
            return ++unitIdPool;
        }

        public void RegisterUnit(ControlUnit unit, UnitData data)
        {
            uint uid = GetNewUnitId();
            unit.Setup(uid, data);

            if (this.units.ContainsKey(uid))
            {
                this.units[uid] = unit;
            }
            else
            {
                this.units.Add(unit.Uid, unit);
            }
        }

        public void UnregisterUnit(ControlUnit unit)
        {
            if (this.units.ContainsKey(unit.Uid))
            {
                this.units.Remove(unit.Uid);

                unit.Destroy();
            }
        }

        private ControlUnit CreateCharacter(string path)
        {
            GameObject gameObject = null;

            UnityEngine.Object target = AssetManager.LoadAsset<UnityEngine.Object>(path);

            if (null != target)
            {
                gameObject = (GameObject)UnityEngine.GameObject.Instantiate(target) as GameObject;
                if (null != gameObject)
                {
                    gameObject.name = target.name;
                    Transform t = gameObject.transform;
                    t.position = Vector3.zero;
                }
            }

            if (null == gameObject)
                return null;

            ControlUnit unitControl = gameObject.GetComponent<ControlUnit>();
            if (null == unitControl)
                unitControl = gameObject.AddComponent<ControlUnit>();

            return unitControl;
        }
    }
}
