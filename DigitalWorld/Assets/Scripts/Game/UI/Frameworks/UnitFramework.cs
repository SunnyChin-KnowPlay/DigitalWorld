using UnityEngine;
using UnityEngine.UI;
using DigitalWorld.UI;
using TMPro;

namespace DigitalWorld.Game.UI
{
    /// <summary>
    /// ��λ���
    /// </summary>
    public class UnitFramework : Widget
    {
        #region Params
        /// <summary>
        /// ��Ԫ
        /// </summary>
        protected UnitHandle unitHandle;

        /// <summary>
        /// ͷ��
        /// </summary>
        protected Image headImage;
        /// <summary>
        /// ����
        /// </summary>
        protected TMP_Text nameText;

        /// <summary>
        /// Ѫ��
        /// </summary>
        protected Slider hpBar;
        protected TMP_Text hpText;
        /// <summary>
        /// ������
        /// </summary>
        protected Slider mpBar;
        protected TMP_Text mpText;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            headImage = GetWidgetComponent<Image>("HeadImage");
            nameText = GetWidgetComponent<TMP_Text>("NameText");
            hpBar = GetWidgetComponent<Slider>("HpBar");
            mpBar = GetWidgetComponent<Slider>("MpBar");
            hpText = GetWidgetComponent<TMP_Text>("HpBar/ValueText");
            mpText = GetWidgetComponent<TMP_Text>("MpBar/ValueText");

        }

        protected virtual void LateUpdate()
        {
            if (unitHandle)
            {
                ControlProperty property = unitHandle.Unit.Property;
                this.hpBar.value = property.Hp.FactorInRange.SingleFloat;
                this.hpText.text = string.Format($"{property.Hp.Value}/{property.Hp.MaxV}");
            }
        }
        #endregion

        #region Logic
        /// <summary>
        /// �󶨵�λ
        /// </summary>
        /// <param name="unit"></param>
        public virtual void Bind(UnitHandle unit)
        {
            this.unitHandle = unit;

            this.nameText.text = this.unitHandle.Unit.Data.Name;
        }
        #endregion
    }
}

