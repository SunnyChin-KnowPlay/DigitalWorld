using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DigitalWorld.Logic
{
    [Serializable]
    public abstract partial class NodeState : NodeBase
    {
        #region Params
        /// <summary>
        /// 状态
        /// </summary>
        public EState State
        {
            get { return _state; }
            set
            {
                ToState(value);
            }
        }
        private EState _state = EState.Idle;

        public bool IsRunning
        {
            get => _state == EState.Running;
        }

        public bool IsEnded
        {
            get => _state == EState.Succeeded || _state == EState.Failed;
        }

        /// <summary>
        /// 运行的时间
        /// </summary>
        public int RunningTime
        {
            get { return _runningTime; }
        }
        protected int _runningTime = 0;

        /// <summary>
        /// 总时长
        /// 如果本节点的总时长是可计算的，则重载此属性并返回预估的总时长
        /// </summary>
        public virtual int TotalTime
        {
            get => 0;
        }

        /// <summary>
        /// 运行次数，每次进到RunningState时，这个就会加一
        /// </summary>
        public int RunningCount => _runningCount;
        private int _runningCount = 0;

        protected List<Requirement> _requirements = new List<Requirement>();

        /// <summary>
        /// 检测逻辑
        /// </summary>
        public ECheckLogic RequirementLogic
        {
            get => _requirementLogic;
            set => _requirementLogic = value;
        }
        protected ECheckLogic _requirementLogic = ECheckLogic.And;

        /// <summary>
        /// 运行模式
        /// </summary>
        public EMotionMode MotionMode { get => _motionMode; }
        private EMotionMode _motionMode;

        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();

            this.State = EState.Idle;
            this._requirementLogic = ECheckLogic.And;
            this._motionMode = EMotionMode.Once;
            this._runningTime = 0;
            this._runningCount = 0;
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            _requirements.Clear();

        }

        public override T CloneTo<T>(T obj)
        {
            if (base.CloneTo(obj) is NodeState node)
            {
                node._requirementLogic = this._requirementLogic;
                node._requirements.Clear();
                foreach (var req in this._requirements)
                {
                    node._requirements.Add(req.Clone() as Requirement);
                }
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
                case EState.Succeeded:
                {
                    if (lastState == EState.Running)
                    {
                        this.OnExit();
                    }
                    break;
                }
                case EState.Failed:
                {
                    if (lastState == EState.Running)
                    {
                        this.OnExit();
                    }
                    break;
                }
                case EState.Running:
                {
                    this.OnEnter();
                    break;
                }
                case EState.Idle:
                {
                    if (lastState == EState.Running)
                    {
                        this.OnExit();
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
        protected override void OnUpdate(int delta)
        {
            if (this.IsEnded)
            {
                if (this.MotionMode == EMotionMode.Loop)
                {
                    this.State = EState.Idle;
                }
                else if (this.MotionMode == EMotionMode.Repeat)
                {
                    this.State = EState.Running;
                }
            }

            if (this._state == EState.Running)
            {
                this._runningTime += delta;
                OnRunning(delta);
            }
        }

        /// <summary>
        /// 常规循环
        /// </summary>
        /// <param name="delta"></param>
        protected virtual void OnRunning(int delta)
        {
            this.UpdateChildren(delta);
        }

        protected virtual void OnEnter()
        {
            this._runningTime = 0;
            this._runningCount += 1;

            UnityEngine.Debug.Log(string.Format("StateNode Enter, Name is:{0}", this.Name));
        }

        protected virtual void OnExit()
        {
            UnityEngine.Debug.Log(string.Format("StateNode Exit, Name is:{0}", this.Name));

            for (int i = 0; i < this._children.Count; ++i)
            {
                if (this._children[i] is NodeState child && null != child && child.Enabled)
                {
                    child.State = EState.Idle;
                }
            }
        }

        /// <summary>
        /// 检查是否符合运行要求
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckRequirement()
        {
            if (null == this.Parent)
            {
                // 没有父节点的话 肯定是符合要求的
                return true;
            }

            bool ret = true;
            ECheckLogic checkLogic = this._requirementLogic;
            switch (checkLogic)
            {
                case ECheckLogic.And:
                {
                    ret = true;

                    foreach (Requirement req in _requirements)
                    {
                        if (FindNodeByRequirement(req) is NodeState node)
                        {
                            if (!node.HasState(req.requirementState))
                            {
                                ret = false;
                                break;
                            }
                        }
                    }
                    break;
                }
                case ECheckLogic.Or:
                {
                    if (_requirements.Count > 0)
                    {
                        ret = false;

                        foreach (Requirement req in _requirements)
                        {
                            if (FindNodeByRequirement(req) is NodeState node)
                            {
                                if (node.HasState(req.requirementState))
                                {
                                    ret = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }

            return ret;
        }

        protected NodeBase FindNodeByRequirement(Requirement requirement)
        {
            NodeBase node = null;
            switch (requirement.locationMode)
            {
                case ELocationMode.Previous:
                {
                    node = this.Parent.GetChild(this.Index - 1);
                    break;
                }
                case ELocationMode.Next:
                {
                    node = this.Parent.GetChild(this.Index + 1);
                    break;
                }
                case ELocationMode.Name:
                {
                    node = this.Parent.Find(requirement.nodeName);
                    break;
                }
            }
            return node;
        }

        /// <summary>
        /// 是否存在状态
        /// </summary>
        /// <returns></returns>
        public bool HasState(EState state)
        {
            return (this._state & state) == state;
        }
        #endregion

        #region Serialization
        protected NodeState(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this._requirementLogic = (ECheckLogic)info.GetValue("_requirementLogic", typeof(ECheckLogic));
            this._requirements = (List<Requirement>)info.GetValue("_requirements", typeof(List<Requirement>));
            this._motionMode = (EMotionMode)info.GetValue("_motionMode", typeof(EMotionMode));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("_requirementLogic", _requirementLogic);
            info.AddValue("_requirements", _requirements);
            info.AddValue("_motionMode", _motionMode);

            // 二进制的序列化已经写好了
            // 
        }
        #endregion

    }
}
