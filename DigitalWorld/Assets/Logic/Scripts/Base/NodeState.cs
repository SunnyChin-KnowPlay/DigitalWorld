using System.Xml;

namespace DigitalWorld.Logic
{
    public abstract partial class NodeState : NodeBase
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
        protected float _runningTime = 0;

        /// <summary>
        /// 总运行时长
        /// </summary>
        public float TotalTime
        {
            get { return _totalTime; }
        }
        private float _totalTime = 0;
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();

            this._state = EState.Idle;
            this._runningTime = 0;
            this._totalTime = 0;
        }

        public override object Clone()
        {
            throw new System.NotImplementedException();
        }

        public override T CloneTo<T>(T obj)
        {
            NodeState node = base.CloneTo(obj) as NodeState;
            if (null != node)
            {
                node._totalTime = this._totalTime;
            }
            return obj;
        }
        #endregion

        #region Logic
        /// <summary>
        /// 转换状态
        /// 状态不一致时才有转换必要
        /// 先把新的状态记录下来
        /// 然后调用OnStateChanged传入旧状态
        /// </summary>
        /// <param name="state">新状态</param>
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
                    if (lastState == EState.Idle)
                    {
                        this.OnEnter();
                    }
                    break;
                }
            }
        }


        /// <summary>
        /// 仅当激活时
        /// 才会进入迭代回调
        /// </summary>
        /// <param name="delta"></param>
        protected override void OnUpdate(float delta)
        {
            if (this._state == EState.Running)
            {
                OnRunning(delta);

                if (this._runningTime >= this._totalTime)
                {
                    this.State = EState.Ending;
                }
            }

            if (this._state == EState.Ending)
            {
                this.State = EState.Idle;
            }
        }

        /// <summary>
        /// 常规循环
        /// </summary>
        /// <param name="delta"></param>
        protected virtual void OnRunning(float delta)
        {
            this.UpdateChildren(delta);

            this._runningTime = System.MathF.Min(this._runningTime + delta, this._totalTime);
        }

        protected virtual void OnEnter()
        {

        }

        protected virtual void OnExit()
        {

        }
        #endregion

        #region Proto
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            this.Encode(this._totalTime);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._totalTime, "totalTime");
        }

        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            this.Decode(ref this._totalTime);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._totalTime, "totalTime");
        }
        #endregion
    }
}
