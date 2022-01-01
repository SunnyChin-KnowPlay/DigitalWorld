using UnityEngine;

namespace DigitalWorld.Game
{
    public abstract class ControlLogic : MonoBehaviour
    {
        #region Params
        protected UnitInfo info;
        protected Transform trans = null;

        public virtual UnitInfo Info
        {
            get { return info; }
        }
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

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Destroy()
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
