using UnityEngine;

namespace DigitalWorld.Game
{
    public abstract partial class BaseSummonerCom : MonoBehaviour
    {
        public BaseSummoner Summoner
        {
            get { return summoner; }
        }
        protected BaseSummoner summoner = null;

        public virtual void Init(BaseSummoner sumer)
        {
            summoner = sumer;
        }

        public virtual string ComName()
        {
            return "SummonerComBase";
        }

    }
}
