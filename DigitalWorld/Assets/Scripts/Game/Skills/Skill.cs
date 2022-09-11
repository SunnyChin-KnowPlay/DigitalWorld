using DigitalWorld.Table;
using Dream.Core;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 技能文件
    /// </summary>
    public class Skill : IPooledObject
    {
        #region Params
        /// <summary>
        /// 技能ID
        /// </summary>
        public int SkillId
        {
            get { return skillId; }
            set { skillId = value; }
        }
        protected int skillId;

        public SkillInfo SkillInfo { get => TableManager.Instance.SkillTable[skillId]; }

        protected IObjectPool pool;
        #endregion

        #region Pool
        public void OnAllocate()
        {
            skillId = 0;
        }

        public void OnRecycle()
        {
            skillId = 0;
        }

        public virtual void Recycle()
        {
            if (null != pool)
            {
                pool.ApplyRecycle(this);
            }
        }

        public virtual void SetPool(IObjectPool pool)
        {
            this.pool = pool;
        }
        #endregion

        #region Logic

        #endregion
    }
}
