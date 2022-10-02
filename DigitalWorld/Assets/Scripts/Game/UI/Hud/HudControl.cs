using DigitalWorld.UI;
using UnityEngine;

namespace DigitalWorld.Game.UI
{
    /// <summary>
    /// Hud控制器
    /// </summary>
    public class HudControl : Control
    {
        #region Params
        /// <summary>
        /// 唯一ID
        /// </summary>
        public uint Uid
        {
            get => uid;
            set
            {
                if (uid != value)
                {
                    this.uid = value;
                    SetupUnit();
                }
            }
        }
        private uint uid;

        public UnitHandle Unit => unit;
        protected UnitHandle unit;
        #endregion

        #region Mono
        private void LateUpdate()
        {
            if (unit == default || !unit.Unit.IsRunning)
            {
                // 到这里说明对象已经
                this.Hide();
            }
            else
            {
                Camera mainCamera = CameraControl.Instance.MainCamera;
                Vector3 screenPoint = mainCamera.WorldToScreenPoint(Unit.Unit.LogicPosition);

                Vector3 uiWorldPoint = Canvas.worldCamera.ScreenToWorldPoint(screenPoint);
                this.Widget.RectTransform.position = uiWorldPoint;
            }
        }
        #endregion

        #region Logic
        private void SetupUnit()
        {
            unit = WorldManager.Instance.GetUnit(uid);
        }
        #endregion
    }

}

