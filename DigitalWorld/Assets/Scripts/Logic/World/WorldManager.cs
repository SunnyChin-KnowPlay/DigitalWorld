using DigitalWorld.Asset;
using DigitalWorld.Behaviour;
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

        protected override void Awake()
        {
            base.Awake();

            mainCamera = Camera.main;
        }

        private void Start()
        {
            this.SetupUnits();
        }

        private void SetupUnits()
        {
            string fullPath = "Unit/Characters/Orc.prefab";

            UnitControl hero = this.CreateCharacter(fullPath);
            if (null != hero)
            {
                InputBehaviour input = hero.GetComponent<InputBehaviour>();
                if (null == input)
                    input = hero.gameObject.AddComponent<InputBehaviour>();
            }
        }

        private UnitControl CreateCharacter(string path)
        {
            GameObject gameObject = null;

            UnityEngine.Object target = AssetManager.Instance.LoadAsset<UnityEngine.Object>(path);

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

            UnitControl unitControl = gameObject.GetComponent<UnitControl>();
            if (null == unitControl)
                unitControl = gameObject.AddComponent<UnitControl>();

            return unitControl;
        }
    }
}
