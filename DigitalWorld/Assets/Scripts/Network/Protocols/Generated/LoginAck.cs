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

        private string _account;
        /// <summary>
        /// 账号
        /// </summary>
        public string account { get { return _account; } set { _account = value; } }
        private string _password;
        /// <summary>
        /// 密码md5
        /// </summary>
        public string password { get { return _password; } set { _password = value; } }
        private ErrorNoti _error;
        /// <summary>
        /// 错误
        /// </summary>
        public ErrorNoti error { get { return _error; } set { _error = value; } }
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

            _account = default(string);
            _password = default(string);
            _error = default(ErrorNoti);
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

            this.SetParamValid(0, this._account != default(string));
            this.SetParamValid(1, this._password != default(string));
            this.SetParamValid(2, this._error != default(ErrorNoti));
        }

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.Encode(this._account);
            if (this.CheckIsParamValid(1))
                this.Encode(this._password);
            if (this.CheckIsParamValid(2))
                this.Encode(this._error);
        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.Decode(ref this._account);
            if (this.CheckIsParamValid(1))
                this.Decode(ref this._password);
            if (this.CheckIsParamValid(2))
                this.Decode(ref this._error);
        }
    }
}
