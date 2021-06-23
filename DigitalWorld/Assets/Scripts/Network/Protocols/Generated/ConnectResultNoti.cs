using Dream.Core;

namespace Dream.Network
{
    /// <summary>
    /// 链接远端结果通知
    /// </summary>
    [ProtocolID(0xFF03)]
    public partial class ConnectResultNoti : Protocol
    {
        private EnumConnectResult _result;
        /// <summary>
        /// 结果
        /// </summary>
        public EnumConnectResult result { get { return _result; } set { _result = value; } }
        public ConnectResultNoti()
        {
            this._id = 0xFF03;
        }

        public override void OnAllocate()
        {
            base.OnAllocate();

            _result = default(EnumConnectResult);
        }

        public override void OnRecycle()
        {

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

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

            this.EncodeEnum(this._result);
        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

            this.DecodeEnum(ref this._result);
        }
    }
}
