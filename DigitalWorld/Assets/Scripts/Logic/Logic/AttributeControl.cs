using UnityEngine;

namespace DigitalWorld.Logic
{
    public class AttributeControl : LogicControl
    {
        /// <summary>
        /// 血量
        /// </summary>
        [SerializeField]
        protected int hp;

        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }

        /// <summary>
        /// 基础 初始血量
        /// </summary>
        public int BaseHp
        { get { return info.hp; } }

        /// <summary>
        /// 攻击力
        /// </summary>
        [SerializeField]
        protected int attack;

        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        /// <summary>
        /// 基础 初始攻击力
        /// </summary>
        public int BaseAttack
        { get { return info.attack; } }

        /// <summary>
        /// 防御力
        /// </summary>
        [SerializeField]
        protected int defense;

        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }

        /// <summary>
        /// 基础 初始防御力
        /// </summary>
        public int BaseDefense
        { get { return info.defense; } }

        #region Setup
        public override void Setup(UnitInfo info)
        {
            base.Setup(info);

            this.hp = info.hp;
            this.attack = info.attack;
            this.defense = info.defense;
        }
        #endregion
    }
}
