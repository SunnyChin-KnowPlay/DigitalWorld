namespace DigitalWorld.Logic
{
    public partial class NodeFSE : BaseNode
    {
        #region Params
        /// <summary>
        /// 运行状态枚举
        /// </summary>
        public enum EState
        {
            Idle = 0,
            /// <summary>
            /// 运行中
            /// </summary>
            Running,
            /// <summary>
            /// 结束中
            /// </summary>
            Ending,
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public EState State
        {
            get { return _state; }
            set
            {
                ToState(value);
            }
        }
        private EState _state;

        /// <summary>
        /// 运行的时间
        /// </summary>
        public float RunningTime
        {
            get { return _runningTime; }
        }
        private float _runningTime = 0;

        /// <summary>
        /// 已激活的持续时间
        /// </summary>
        public float EnabledDurationTime
        {
            get { return _enabledDurationTime; }
        }
        private float _enabledDurationTime = 0;
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();

            this._state = EState.Idle;
            this._runningTime = 0;
            this._enabledDurationTime = 0;
        }

        public override object Clone()
        {
            throw new System.NotImplementedException();
        }

        public override T CloneTo<T>(T obj)
        {
            NodeFSE node = base.CloneTo(obj) as NodeFSE;
            if (null != node)
            {
                node._state = this._state;
                node._runningTime = this._runningTime;
                node._enabledDurationTime = this._enabledDurationTime;
            }
            return obj;
        }
        #endregion

        #region Logic
        private void ToState(EState state)
        {
            if (this._state != state)
            {
                EState lastState = this._state;
                this._state = state;
                OnStateChanged(lastState);
            }
        }

        protected virtual void OnStateChanged(EState lastState)
        {
            switch (this.State)
            {
                case EState.Idle:
                {
                    if (lastState == EState.Ending)
                    {
                        this.OnExit();
                    }
                    this._runningTime = 0;
                    break;
                }
                case EState.Running:
                {
                    this._runningTime = 0;
                    this.OnEnter();
                   
                    break;
                }
            }
        }

        public virtual void Update(float delta)
        {
            _enabledDurationTime += delta;
            if (this._state == EState.Running)
            {
                OnUpdate(delta);
            }

            if (this._state == EState.Ending)
            {
                this.State = EState.Idle;
            }
        }

        protected virtual void OnUpdate(float delta)
        {
            this._runningTime += delta;
        }

        protected virtual void OnEnter()
        {

        }

        protected virtual void OnExit()
        {

        }
        #endregion
    }
}
