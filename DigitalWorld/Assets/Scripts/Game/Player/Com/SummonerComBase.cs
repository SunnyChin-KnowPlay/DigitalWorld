using UnityEngine;

namespace DigitalWorld.Game
{
    internal abstract partial class SummonerComBase : MonoBehaviour
    {
        public SummonerBase Summoner
        {
            get { return summoner; }
        }
        protected SummonerBase summoner = null;

        public virtual void Init(SummonerBase summoner)
        {
            this.summoner = summoner;
        }

        public virtual string ComName()
        {
            return "SummonerComBase";
        }

    }
}
