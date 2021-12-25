using DigitalWorld.Asset;
using DigitalWorld.Behaviour;
using DigitalWorld.TileMap;
using Dream.Extension.Unity;
using DreamEngine;
using UnityEngine;

namespace DigitalWorld.Logic
{
    public sealed class WorldManager : Singleton<WorldManager>
    {
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

        protected override void Awake()
        {
            base.Awake();

            mainCamera = Camera.main;
        }

        private void Start()
        {
            this.SetupMap();

            //this.SetupUnits();
        }


        //private void SetupUnits()
        //{
        //    string fullPath = "Unit/Characters/Orc.prefab";

        //    ControlUnit hero = this.CreateCharacter(fullPath);
        //    if (null != hero)
        //    {
        //        InputBehaviour input = hero.GetComponent<InputBehaviour>();
        //        if (null == input)
        //            input = hero.gameObject.AddComponent<InputBehaviour>();
        //    }
        //}

        private void SetupMap()
        {
            string fullPath = "Map/NewTileMap.prefab";

            GameObject gameObject = null;

            UnityEngine.Object target = AssetManager.LoadAsset<UnityEngine.Object>(fullPath);

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

            if (null != gameObject)
            {
                tileMapControl = gameObject.GetOrAddComponent<TileMapControl>();
                if (null != tileMapControl)
                {
                    tileMapControl.Setup();
                }
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
