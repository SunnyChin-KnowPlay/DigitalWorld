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
