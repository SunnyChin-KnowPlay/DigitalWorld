using UnityEngine;

namespace DigitalWorld.Logic
{
    public class ControlAttribute : ControlLogic
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

        protected int level;
        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get { return level; }
        }

        #region Setup
        public override void Setup(UnitInfo info)
        {
            base.Setup(info);

            this.hp = info.hp;
            this.attack = info.attack;
            this.level = info.level;
        }
        #endregion
    }
}
