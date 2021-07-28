using Dream.Core;

namespace Dream.Network
{
    /// <summary>
    /// 链接远端结果通知
    /// </summary>
    [ProtocolID(0xFF03)]
    public partial class ConnectResultNoti : Protocol
    {
        protected override int ValidByteSize => 1;

        public override ushort Id => 0xFF03;

        private EnumConnectResult _result;
        /// <summary>
        /// 结果
        /// </summary>
        public EnumConnectResult result { get { return _result; } set { _result = value; } }
        public ConnectResultNoti()
        {
        }

        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            _result = default(EnumConnectResult);
        }

        public override Protocol Allocate()
        {
            return ObjectPool<ConnectResultNoti>.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<ConnectResultNoti>.Recycle(this);
        }

        public static ConnectResultNoti Alloc()
        {
            return ObjectPool<ConnectResultNoti>.Allocate();
        }

        protected override void CalculateValids()
        {
            base.CalculateValids();

            this.SetParamValid(0, this._result != default(EnumConnectResult));
        }

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.EncodeEnum(this._result);
        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.DecodeEnum(ref this._result);
        }
    }
}
