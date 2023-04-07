using DigitalWorld.Asset;
using DigitalWorld.Table;
using DigitalWorld.Extension.Unity;
using DreamEngine.Core;
using System.Collections.Generic;
using UnityEngine;
using DigitalWorld.UI;
using DigitalWorld.Game.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using DigitalWorld.Behaviours;
using DigitalWorld.Notices.UI;

namespace DigitalWorld.Game
{
    public sealed class WorldManager : Singleton<WorldManager>
    {
        #region Delegates
        /// <summary>
        /// 验证单位是否需要
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>true:需要|false:不需要</returns>
        public delegate bool JudgeUnitHandle(UnitHandle unit);
        #endregion

        #region Params
        /// <summary>
        /// 管理器词典
        /// </summary>
        private readonly Dictionary<EUnitType, UnitManager> managers = new Dictionary<EUnitType, UnitManager>();

        /// <summary>
        /// 单位词典
        /// </summary>
        public Dictionary<uint, UnitHandle> Units => units;
        private readonly Dictionary<uint, UnitHandle> units = new Dictionary<uint, UnitHandle>();
        /// <summary>
        /// 跑Update用的单位队列
        /// </summary>
        private readonly List<UnitHandle> runningUnits = new List<UnitHandle>();

        /// <summary>
        /// 单位id池
        /// </summary>
        private uint unitIdPool = 0;

        /// <summary>
        /// 玩家的单元
        /// </summary>
        public UnitHandle PlayerUnit => playerUnit;
        private UnitHandle playerUnit;

        private HudPanel hudPanel;

        /// <summary>
        /// 地图
        /// </summary>
        public MapControl Map => map;
        private MapControl map;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();
            this.Setup();
        }

        protected override void OnDestroy()
        {
            if (null != hudPanel)
            {
                GameObject.Destroy(hudPanel.gameObject);
                hudPanel = null;
            }

            if (null != this.map)
            {
                GameObject.Destroy(map.gameObject);
                map = null;
            }

            base.OnDestroy();
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
            SetupMap();
            SetupManagers();
            SetupUnits();
            SetupCameras();
            // 然后开启UI
            SetupUI();
        }

        private void SetupUI()
        {
            UIManager uiManager = UIManager.Instance;
            uiManager.ShowPanel<GamePanel>(GamePanel.path);
            uiManager.ShowPanel<NoticePanel>(NoticePanel.path);
            hudPanel = uiManager.CreateWidget<HudPanel>(PanelControl.PanelPrefabPath);
            hudPanel.gameObject.name = "HudPanel";
            uiManager.SetupPanel(hudPanel.gameObject);
        }

        private void SetupUnits()
        {
            unitIdPool = 0;
            this.units.Clear();

            UnitData unitData = UnitData.CreateFromCharacter(1001);

            UnitControl unit = this.RegisterUnit(unitData);
            if (null != unit)
            {
                unit.IsPlayerControlling = true;
                playerUnit = new UnitHandle(unit);
                unit.LogicPosition = Vector3.zero;

                _ = unit.GetOrAddComponent<ControlBehaviour>();
                unit.AddControl(ELogicControlType.Test, unit.GetOrAddComponent<ControlTest>());

                SkillInfo skillInfo = TableManager.Instance.SkillTable[100001];
                if (null != skillInfo)
                {
                    unit.Skill.Study(skillInfo, 0);
                }

                //Logic.Trigger trigger = Logic.LogicHelper.AllocateTrigger("Assets/Res/Logic/Triggers/Game/Character/123.asset");
                //trigger.Invoke(Logic.Events.Event.CreateTrigger(UnitHandle.Null));
                //unit.Trigger.RunTrigger(trigger);
            }
        }

        private void SetupCameras()
        {
            // 然后设置摄像机
            CameraControl cc = CameraControl.Instance;
            if (null != cc)
            {
                cc.focused = playerUnit.Unit.transform;
            }
        }

        private void SetupMap()
        {
            MapData data = new MapData(128, 128);
            this.map = MapControl.Create(data);
        }
        #endregion

        #region UnitManager
        public CharacterManager CharacterManager => GetManager<CharacterManager>(EUnitType.Character);
        public BuildingManager BuildingManager => GetManager<BuildingManager>(EUnitType.Building);

        private void SetupManagers()
        {
            RegisterManager<CharacterManager>();
            RegisterManager<BuildingManager>();
        }

        private void RegisterManager<T>() where T : UnitManager
        {
            GameObject go = new GameObject();

            T manager = go.AddComponent<T>();
            if (null != manager)
            {
                go.name = manager.Name;
                go.transform.SetParent(this.transform, false);

                this.RegisterManager(manager);
            }
        }

        private void RegisterManager(UnitManager manager)
        {
            if (this.managers.ContainsKey(manager.Type))
            {
                this.managers[manager.Type] = manager;
            }
            else
            {
                this.managers.Add(manager.Type, manager);
            }
        }

        private UnitManager GetManager(EUnitType type)
        {
            this.managers.TryGetValue(type, out UnitManager manager);
            return manager;
        }

        public T GetManager<T>(EUnitType type) where T : UnitManager
        {
            UnitManager manager = GetManager(type);
            return manager as T;
        }
        #endregion

        #region Unit
        private uint GetNewUnitId()
        {
            return ++unitIdPool;
        }

        public UnitControl RegisterUnit(UnitData data)
        {
            UnitControl unit = this.CreateCharacter(data.PrefabPath);
            if (null != unit)
            {
                this.RegisterUnit(unit, data);
                return unit;
            }
            return null;
        }

        private void RegisterUnit(UnitControl unit, UnitData data)
        {
            uint uid = GetNewUnitId();
            unit.Setup(uid, data);

            UnitHandle handle = new UnitHandle(unit);

            if (this.units.ContainsKey(uid))
            {
                this.units[uid] = handle;
            }
            else
            {
                this.units.Add(unit.Uid, handle);
            }

            UnitManager manager = this.GetManager(unit.Type);
            manager?.RegisterUnit(handle);
        }

        private void UnregisterUnit(UnitControl unit)
        {
            if (this.units.ContainsKey(unit.Uid))
            {
                this.units.Remove(unit.Uid);

                UnitHandle handle = new UnitHandle(unit);

                UnitManager manager = this.GetManager(unit.Type);
                manager?.UnregisterUnit(handle);
            }
        }

        private UnitControl CreateCharacter(string path)
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

            gameObject.layer = LayerMask.NameToLayer("Unit");

            if (!gameObject.TryGetComponent<UnitControl>(out UnitControl unitControl))
            {
                unitControl = gameObject.AddComponent<CharacterControl>();
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
                UnitControl unit = this.runningUnits[i];
                if (unit.Status == EUnitStatus.WaitRecycle)
                {
                    this.UnregisterUnit(unit);
                    unit.Destroy();
                }
            }
        }
        #endregion

        #region Logic
        public UnitHandle GetUnit(uint targetId)
        {
            this.units.TryGetValue(targetId, out UnitHandle unit);
            return unit;
        }

        /// <summary>
        /// 获取除目标外的所有对象
        /// </summary>
        /// <param name="targetId">目标ID</param>
        /// <returns>除targetId外的所有的Unit</returns>
        public List<UnitHandle> FindOtherUnits(uint targetId)
        {
            List<UnitHandle> otherUnits = new List<UnitHandle>();

            foreach (KeyValuePair<uint, UnitHandle> kvp in units)
            {
                if (kvp.Key != targetId)
                {
                    otherUnits.Add(kvp.Value);
                }
            }

            return otherUnits;
        }

        public void FilterUnitsToList(List<UnitHandle> list, JudgeUnitHandle judgeFunction)
        {
            if (null == judgeFunction)
                return;

            foreach (KeyValuePair<uint, UnitHandle> kvp in units)
            {
                if (judgeFunction.Invoke(kvp.Value))
                {
                    list.Add(kvp.Value);
                }
            }
        }
        #endregion

        #region Exit
        /// <summary>
        /// 请求退出
        /// </summary>
        public void Exit()
        {
            StartCoroutine(ApplyExit());
        }

        private IEnumerator ApplyExit()
        {
            UIManager.Instance.UnloadAllPanels();

            SceneManager.LoadScene("Login");
            yield return new WaitForEndOfFrame();



            DestroyInstance();
        }
        #endregion

    }
}
