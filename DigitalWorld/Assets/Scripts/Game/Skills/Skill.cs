using Dream.Core;

namespace DigitalWorld.Game
{
    public abstract class Skill : IPooledObject
    {
        //技能ID
        protected int skillId;
        public int SkillId { get { return skillId; } }

        protected string assetName = string.Empty;
        public string AssetName { get { return assetName; } }

        public void OnAllocate()
        {
            skillId = 0;
        }

        public void OnRecycle()
        {
            skillId = 0;
        }

        public abstract void Recycle();

        public abstract void SetPool(IObjectPool pool);
    }
}
