using DigitalWorld.Asset;
using DigitalWorld.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalWorld.Game.UI
{
    public class HudPanel : PanelControl
    {
        #region Params
        /// <summary>
        /// 姓名版词典
        /// </summary>
        private readonly Dictionary<uint, PlaterControl> platers = new Dictionary<uint, PlaterControl>();
        private readonly Stack<GameObject> platerStack = new Stack<GameObject>();

        private readonly static List<uint> keys = new List<uint>();

        private float updateDuration = 0;

        private const float platerMaxDistance = 50;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            RectTransform rootRectTransform = this.Panel.Root;
            if (rootRectTransform.TryGetComponent<Image>(out Image image))
            {
                image.enabled = false;
            }
        }

        private void LateUpdate()
        {
            updateDuration -= Time.deltaTime;

            if (updateDuration <= 0)
            {
                updateDuration = 1f;
                UpdatePlaters();

            }
        }

        private void OnDestroy()
        {
            foreach (var kvp in platers)
            {
                GameObject.Destroy(kvp.Value.gameObject);
            }
            platers.Clear();

            while (platerStack.Count > 0)
            {
                GameObject.Destroy(platerStack.Pop());
            }
        }
        #endregion

        #region Logic
        private void UpdatePlaters()
        {
            RecyclePlaters();
            ScanNewPlaters();
        }

        private void RecyclePlaters()
        {
            keys.Clear();
            keys.AddRange(platers.Keys);

            for (int i = 0; i < keys.Count; ++i)
            {
                if (platers.TryGetValue(keys[i], out PlaterControl control))
                {
                    if (!control.gameObject.activeSelf)
                    {
                        platers.Remove(keys[i]);
                        RecycleHudControl(control, platerStack);
                    }
                }
            }
        }

        private void ScanNewPlaters()
        {
            Dictionary<uint, UnitHandle> units = WorldManager.Instance.Units;
            Camera mainCamera = CameraControl.Instance.MainCamera;

            foreach (var kvp in units)
            {
                UnitHandle unitHandle = kvp.Value;

                if (this.platers.ContainsKey(unitHandle.Uid))
                    continue; // 说明已经有了 忽略

                Vector3 screenPoint = mainCamera.WorldToScreenPoint(unitHandle.Unit.LogicPosition);
                Ray ray = mainCamera.ScreenPointToRay(screenPoint);
                bool ret = Physics.Raycast(ray, platerMaxDistance, 1 << LayerMask.NameToLayer("Unit"));
                if (ret)
                {
                    PlaterControl control = AllocateHudControl<PlaterControl>(platerStack, PlaterControl.path);
                    control.Widget.RectTransform.SetParent(this.transform, false);
                    control.Bind(unitHandle);
                    platers.Add(unitHandle.Uid, control);
                }
            }
        }

        private void RecycleHudControl(HudControl control, Stack<GameObject> stack)
        {
            RectTransform rt = control.Widget.RectTransform;
            rt.SetParent(null);

            GameObject go = control.gameObject;
            go.SetActive(false);

            stack.Push(go);
        }

        private T AllocateHudControl<T>(Stack<GameObject> stack, string path) where T : HudControl
        {
            GameObject go = null;
            if (stack.Count <= 0)
            {
                GameObject obj = AssetManager.LoadAsset<GameObject>(path);
                go = GameObject.Instantiate(obj);
            }
            else
            {
                go = stack.Pop();
            }
            if (null != go)
            {
                if (!go.TryGetComponent<T>(out T title))
                {
                    title = go.AddComponent<T>();
                }

                return title;
            }

            return null;
        }
        #endregion
    }
}
