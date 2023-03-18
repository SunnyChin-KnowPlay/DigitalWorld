using Dream.Core;
using Dream.Proto;
using System;
using System.Xml;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 用于逻辑检测用的要求
    /// 记录的是是否满足的情况
    /// </summary>
    [Serializable]
    public partial class Requirement : IPooledObject, ICloneable
    {
        #region Params
        /// <summary>
        /// 定位方式
        /// </summary>
        public ELocationMode locationMode;

        /// <summary>
        /// 节点名
        /// </summary>
        public string nodeName;

        /// <summary>
        /// 需求的状态
        /// </summary>
        public EState requirementState;
        #endregion

        #region Pool
        public virtual void OnAllocate()
        {
            locationMode = ELocationMode.Name;
            nodeName = string.Empty;
            requirementState = EState.Idle;
        }

        public void OnRecycle()
        {
            throw new NotImplementedException();
        }

        [Newtonsoft.Json.JsonIgnore]
        protected IObjectPool pool;

        public virtual void Recycle()
        {
            if (null != pool)
            {
                pool.ApplyRecycle(this);
            }
        }

        public virtual void SetPool(IObjectPool pool)
        {
            this.pool = pool;
        }
        #endregion

        #region Clone
        public object Clone()
        {
            Requirement req = new Requirement();
            this.CloneTo(req);
            return req;
        }

        public virtual T CloneTo<T>(T obj) where T : Requirement
        {
            obj.locationMode = this.locationMode;
            obj.nodeName = this.nodeName;
            obj.requirementState = this.requirementState;

            return obj;
        }

       
        #endregion

    }
}
