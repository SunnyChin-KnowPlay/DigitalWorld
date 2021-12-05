using DigitalWorld.Asset;
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
            GameObject gameObject;
            string fullPath = "Unit/Characters/Orc.prefab";
            UnityEngine.Object target = AssetManager.Instance.LoadAsset<UnityEngine.Object>(fullPath);

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
        }
    }
}
