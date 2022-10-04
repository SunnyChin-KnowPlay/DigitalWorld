using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace DigitalWorld.Game.UI
{
    public class PlaterControl : HudControl
    {
        #region Enter
        public const string path = "Assets/Res/UI/Game/Plater/Plater.prefab";
        #endregion

        #region Params
        /// <summary>
        /// 姓名
        /// </summary>
        protected TMP_Text nameText;

        /// <summary>
        /// 血条
        /// </summary>
        protected Slider hpBar;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            nameText = GetControlComponent<TMP_Text>("NameText");
            hpBar = GetControlComponent<Slider>("HpBar");
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (unitHandle)
            {
                ControlProperty property = unitHandle.Unit.Property;
                this.hpBar.value = property.Hp.FactorInRange.SingleFloat;
            }
        }
        #endregion

        #region Logic
        public override void Bind(UnitHandle unit)
        {
            base.Bind(unit);

            this.nameText.text = this.unitHandle.Unit.Data.Name;
        }
        #endregion
    }
}
