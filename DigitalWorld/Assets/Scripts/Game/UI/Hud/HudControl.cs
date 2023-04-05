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


        public UnitHandle Unit => unitHandle;
        protected UnitHandle unitHandle;
        #endregion

        #region Mono
        protected virtual void LateUpdate()
        {
            if (unitHandle == default || !unitHandle.Unit.IsRunning)
            {
                // 到这里说明对象已经
                this.Hide();
            }
            else
            {
                Canvas canvas = Canvas;
                if (null != canvas)
                {
                    Camera mainCamera = CameraControl.Instance.MainCamera;
                    Vector3 worldPos = Unit.Unit.LogicPosition + Utilities.Convert.ToVector3(Unit.Unit.Data.ScaleSize);
                    Vector3 screenPoint = mainCamera.WorldToScreenPoint(worldPos);

                    Vector3 uiWorldPoint = Canvas.worldCamera.ScreenToWorldPoint(screenPoint);
                    this.FirstWidget.RectTransform.position = uiWorldPoint;
                }
                else
                {
                    this.Hide();
                }
            }
        }
        #endregion

        #region Logic
        /// <summary>
        /// 绑定单位
        /// </summary>
        /// <param name="unit"></param>
        public virtual void Bind(UnitHandle unit)
        {
            this.unitHandle = unit;
            if (unit)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }
        #endregion
    }

}

