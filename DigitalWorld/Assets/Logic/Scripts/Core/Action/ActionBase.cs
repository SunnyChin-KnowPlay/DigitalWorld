using System.Xml;
using System.Xml.Serialization;

namespace DigitalWorld.Logic.Actions
{
    
    public abstract partial class ActionBase : NodeState
    {
        #region Params
        public override ENodeType NodeType => ENodeType.Action;
        public Trigger Trigger
        {
            get
            {
                return this._parent as Trigger;
            }
        }

        /// <summary>
        /// 启动时间
        /// </summary>
        protected int startTime;

        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();

            _requirementLogic = ECheckLogic.And;
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

        }
        #endregion

        #region Logic
        protected override void OnUpdate(int delta)
        {
            if (this.State == EState.Idle)
            {
                if (this.CheckRequirement())
                {
                    this.State = EState.Running;
                }
            }

            base.OnUpdate(delta);
        }
        #endregion

        #region Serialization
        
        #endregion
    }
}
