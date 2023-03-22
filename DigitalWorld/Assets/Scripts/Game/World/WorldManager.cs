using DigitalWorld.Asset;
using DigitalWorld.Inputs;
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
        private Map map;
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
            hudPanel = uiManager.CreateWidget<HudPanel>("Assets/Res/UI/Elements/Panel/Panel.prefab");
            hudPanel.gameObject.name = "HudPanel";
            uiManager.SetupPanel(hudPanel.gameObject);
        }

        private void SetupUnits()
        {
            unitIdPool = 0;
            this.units.Clear();

            UnitData unitData = UnitData.CreateFromCharacter(1001);

            ControlUnit unit = this.RegisterUnit(unitData);
            if (null != unit)
            {
                unit.IsPlayerControlling = true;
                playerUnit = new UnitHandle(unit);
                unit.LogicPosition = Vector3.zero;

                _ = unit.GetOrAddComponent<ControlBehaviour>();
                unit.AddControl(ELogicControlType.Test, unit.GetOrAddComponent<ControlTest>());

                Logic.Trigger trigger = Logic.LogicHelper.AllocateTrigger("Assets/Res/Logic/Triggers/Game/Character/123.asset");
                trigger.Invoke(Logic.Events.Event.CreateTrigger(UnitHandle.Null));
                unit.Trigger.RunTrigger(trigger);

                SkillInfo skillInfo = TableManager.Instance.SkillTable[100001];
                if (null != skillInfo)
                {
                    unit.Skill.Study(skillInfo, 0);
                }
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
            this.map = new Map(256, 256);

        }
        #endregion

        #region Create & Register
        private uint GetNewUnitId()
        {
            return ++unitIdPool;
        }

        public ControlUnit RegisterUnit(UnitData data)
        {
            ControlUnit unit = this.CreateCharacter(data.CharacterInfo.PrefabPath);
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

            UnitHandle handle = new UnitHandle(unit);

            if (this.units.ContainsKey(uid))
            {
                this.units[uid] = handle;
            }
            else
            {
                this.units.Add(unit.Uid, handle);
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

            gameObject.layer = LayerMask.NameToLayer("Unit");

            if (!gameObject.TryGetComponent<ControlUnit>(out ControlUnit unitControl))
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
