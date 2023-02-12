using System.Xml;

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
        protected override void OnDecode()
        {
            base.OnDecode();
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);
        }

        protected override void OnEncode()
        {
            base.OnEncode();
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);
        }

        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();
        }
        #endregion
    }
}
