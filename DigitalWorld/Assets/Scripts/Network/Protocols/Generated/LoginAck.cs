using Dream.Core;

namespace Dream.Network
{
    /// <summary>
    /// 登录响应
    /// </summary>
    [ProtocolID(0x0011)]
    public partial class LoginAck : Protocol
    {
        protected override int ValidByteSize => 1;

        public override ushort Id => 0x0011;

        private ErrorNoti _result;
        /// <summary>
        /// 错误
        /// </summary>
        public ErrorNoti result { get { return _result; } set { _result = value; } }
        public LoginAck()
        {
        }

        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            _result = default(ErrorNoti);
        }

        public override Protocol Allocate()
        {
            return ObjectPool<LoginAck>.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<LoginAck>.Recycle(this);
        }

        public static LoginAck Alloc()
        {
            return ObjectPool<LoginAck>.Allocate();
        }

        protected override void CalculateValids()
        {
            base.CalculateValids();

            this.SetParamValid(0, this._result != default(ErrorNoti));
        }

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.Encode(this._result);
        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.Decode(ref this._result);
        }
    }
}
