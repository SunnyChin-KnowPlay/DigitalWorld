using Dream.Core;

namespace Dream.Network
{
    /// <summary>
    /// 登录响应
    /// </summary>
    [ProtocolID(0x0011)]
    public partial class LoginAck : Protocol
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
        public LoginAck()
        {
            this._id = 0x0011;
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
    }
}
