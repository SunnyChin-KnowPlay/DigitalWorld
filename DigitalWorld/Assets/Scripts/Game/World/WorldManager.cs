using DigitalWorld.Asset;
using DigitalWorld.Behaviour;
using DigitalWorld.Proto.Game;
using DigitalWorld.Table;
using DigitalWorld.Extension.Unity;
using DreamEngine.Core;
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
        /// 单位词典
        /// </summary>
        private readonly Dictionary<uint, ControlUnit> units = new Dictionary<uint, ControlUnit>();
        /// <summary>
        /// 跑Update用的单位队列
        /// </summary>
        private readonly List<ControlUnit> runningUnits = new List<ControlUnit>();

        /// <summary>
        /// 单位id池
        /// </summary>
        private uint unitIdPool = 0;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            mainCamera = Camera.main;
        }

        private void Start()
        {
            this.Setup();
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            UpdateUnits(delta);
        }
        #endregion

        #region Setup
        private void Setup()
        {
            unitIdPool = 0;
            this.units.Clear();

            UnitData unitData = UnitData.CreateFromCharacter(1001);

            ControlUnit unit = this.RegisterUnit(unitData);
            if (null != unit)
            {
                unit.LogicPosition = Vector3.zero;

                _ = unit.GetOrAddComponent<InputBehaviour>();
                unit.AddControl(ELogicControlType.Test, unit.GetOrAddComponent<ControlTest>());

                Logic.Behaviour behaviour = Logic.LogicHelper.AllocateBehaviour("Assets/Res/Logic/Behaviours/123.asset");
                unit.Behaviour.AddBehaviour(behaviour);
            }


            // 最后添加摄像机

            GameObject mainCameraObj = AssetManager.LoadAsset<GameObject>("Assets/Res/Cameras/BattleMainCamera.prefab");
            if (null != mainCameraObj)
            {
                GameObject obj = GameObject.Instantiate(mainCameraObj);
                if (null != obj)
                {
                    obj.name = "MainCamera";
                    CameraControl cc = obj.GetOrAddComponent<CameraControl>();
                    if (null != cc)
                    {
                        cc.focused = unit.transform;
                    }
                }
            }

        }
        #endregion

        #region Create & Register
        private uint GetNewUnitId()
        {
            return ++unitIdPool;
        }

        public ControlUnit RegisterUnit(UnitData data)
        {
            ControlUnit unit = this.CreateCharacter(data.CharacterInfo.prefabPath);
            if (null != unit)
            {
                this.RegisterUnit(unit, data);
                return unit;
            }
            return null;
        }

        private void RegisterUnit(ControlUnit unit, UnitData data)
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

        private void UnregisterUnit(ControlUnit unit)
        {
            if (this.units.ContainsKey(unit.Uid))
            {
                this.units.Remove(unit.Uid);
            }
        }

        private ControlUnit CreateCharacter(string path)
        {
            Object target = AssetManager.LoadAsset<Object>(path);
            GameObject gameObject = null;

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

            if (null == gameObject)
                return null;

            ControlUnit unitControl = gameObject.GetComponent<ControlUnit>();
            if (null == unitControl)
            {
                unitControl = gameObject.AddComponent<ControlCharacter>();
            }


            return unitControl;
        }
        #endregion

        #region Update
        private void UpdateUnits(float dt)
        {
            this.runningUnits.Clear();
            foreach (var kvp in this.units)
                this.runningUnits.Add(kvp.Value);

            for (int i = 0; i < this.runningUnits.Count; ++i)
            {
                ControlUnit unit = this.runningUnits[i];
                if (unit.Status == EUnitStatus.WaitRecycle)
                {
                    this.UnregisterUnit(unit);
                    unit.Destroy();
                }
            }
        }
        #endregion

    }
}
