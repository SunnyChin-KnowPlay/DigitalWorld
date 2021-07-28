using Dream.Core;

namespace Dream.Network
{
    /// <summary>
    /// 登录请求
    /// </summary>
    [ProtocolID(0x0010)]
    public partial class LoginReq : Protocol
    {
        protected override int ValidByteSize => 1;

        public override ushort Id => 0x0010;

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
        public LoginReq()
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
        }

        public override Protocol Allocate()
        {
            return ObjectPool<LoginReq>.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<LoginReq>.Recycle(this);
        }

        public static LoginReq Alloc()
        {
            return ObjectPool<LoginReq>.Allocate();
        }

        protected override void CalculateValids()
        {
            base.CalculateValids();

            this.SetParamValid(0, this._account != default(string));
            this.SetParamValid(1, this._password != default(string));
        }

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.Encode(this._account);
            if (this.CheckIsParamValid(1))
                this.Encode(this._password);
        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.Decode(ref this._account);
            if (this.CheckIsParamValid(1))
                this.Decode(ref this._password);
        }
    }
}
