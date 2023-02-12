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
    public partial class Requirement : ByteBuffer, ICloneable
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
        public override void OnAllocate()
        {
            base.OnAllocate();

            locationMode = ELocationMode.Name;
            nodeName = string.Empty;
            requirementState = EState.Idle;
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

        #region Serialization
        protected override void OnDecode()
        {
            base.OnDecode();

            this.DecodeEnum(ref this.locationMode);
            this.Decode(ref this.nodeName);
            this.DecodeEnum(ref this.requirementState);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.DecodeEnum(ref this.locationMode, "locationMode");
            this.Decode(ref this.nodeName, "nodeName");
            this.DecodeEnum(ref this.requirementState, "requirementState");
        }

        protected override void OnEncode()
        {
            base.OnEncode();

            this.EncodeEnum(this.locationMode);
            this.Encode(this.nodeName);
            this.EncodeEnum(this.requirementState);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.EncodeEnum(this.locationMode, "locationMode");
            this.Encode(this.nodeName, "nodeName");
            this.EncodeEnum(this.requirementState, "requirementState");
        }

        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSizeEnum(this.locationMode);
            this.CalculateSize(this.nodeName);
            this.CalculateSizeEnum(this.requirementState);
        }
        #endregion
    }
}
