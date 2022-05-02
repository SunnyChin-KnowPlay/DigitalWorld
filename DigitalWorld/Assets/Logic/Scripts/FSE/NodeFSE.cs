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
            Running,
            End
        }

        private EState state;
        /// <summary>
        /// 运行状态
        /// </summary>
        public EState State
        {
            get { return state; }
            set
            {
                ToState(value);
            }
        }

        private float runningTime = 0;
        /// <summary>
        /// 运行的时间
        /// </summary>
        public float RunningTime
        {
            get { return runningTime; }
        }

        /// <summary>
        /// 已激活的持续时间
        /// </summary>
        public float EnabledDurationTime
        {
            get { return enabledDurationTime; }
        }
        private float enabledDurationTime = 0;
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();

            this.state = EState.Idle;
            this.runningTime = 0;
            this.enabledDurationTime = 0;
        }
        #endregion

        #region Logic
        private void ToState(EState state)
        {
            if (this.state != state)
            {
                EState lastState = this.state;
                this.state = state;
                OnStateChanged(lastState);
                if (this.state == EState.End)
                {
                    this.state = EState.Idle;
                }
            }
        }

        protected virtual void OnStateChanged(EState lastState)
        {
            switch (this.state)
            {
                case EState.Idle:
                {
                    runningTime = 0;
                    break;
                }
            }
        }

        public void Start()
        {
            this.State = EState.Running;
        }

        public void Stop()
        {
            this.State = EState.Idle;
        }

        public virtual void Update(float delta)
        {
            enabledDurationTime += delta;
            if (this.state == EState.Running)
            {
                OnUpdate(delta);
            }
        }

        protected virtual void OnUpdate(float delta)
        {
            this.runningTime += delta;
        }
        #endregion
    }
}
