using Dream.Core;

namespace Dream.Network
{
    /// <summary>
    /// 登录请求
    /// </summary>
    [ProtocolID(0x0010)]
    public partial class LoginReq : Protocol
    {
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
            this._id = 0x0010;
        }

        public override void OnAllocate()
        {
            base.OnAllocate();

            _account = default(string);
            _password = default(string);
        }

        public override void OnRecycle()
        {

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

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

            this.Encode(this._account);
            this.Encode(this._password);
        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

            this.Decode(ref this._account);
            this.Decode(ref this._password);
        }
    }
}
