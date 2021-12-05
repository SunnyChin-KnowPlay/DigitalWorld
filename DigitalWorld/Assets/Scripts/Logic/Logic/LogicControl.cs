using UnityEngine;

namespace DigitalWorld.Logic
{
    public class LogicControl : MonoBehaviour
    {
        #region Params
        protected UnitInfo info;
        protected Transform trans = null;
        #endregion

        #region Behaviour
        protected virtual void Awake()
        {
            trans = this.GetComponent<Transform>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }
        #endregion

        #region Setup
        public virtual void Setup(UnitInfo info)
        {
            this.info = info;
        }
        #endregion
    }
}
