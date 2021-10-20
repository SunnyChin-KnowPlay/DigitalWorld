using Dream.Core;
using Dream.Proto;

namespace DigitalWorld.Proto.Common
{
        	/// <summary>
    /// 
    /// </summary>
    public enum EnumTest2 : int
    {
  
        /// <summary>
        /// 
        /// </summary>
        First = 1,
  
        /// <summary>
        /// 
        /// </summary>
        Second,
    }
        	/// <summary>
    /// 
    /// </summary>
    public enum EnumErrorCode : int
    {
  
        /// <summary>
        /// 
        /// </summary>
        Success = 0,
  
        /// <summary>
        /// 
        /// </summary>
        AccountErr,
  
        /// <summary>
        /// 
        /// </summary>
        PasswordErr,
    }
        	/// <summary>
    /// 
    /// </summary>
    public enum EnumConnectResult : int
    {
  
        /// <summary>
        /// 
        /// </summary>
        Success = 0,
  
        /// <summary>
        /// 
        /// </summary>
        Failed = 1,
    }
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
            return ObjectPool<LoginReq>.Instance.Allocate();
        }

        public static LoginReq Alloc()
        {
            return ObjectPool<LoginReq>.Instance.Allocate();
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
            return ObjectPool<LoginAck>.Instance.Allocate();
        }

        public static LoginAck Alloc()
        {
            return ObjectPool<LoginAck>.Instance.Allocate();
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

            /// <summary>
    /// 错误通知
    /// </summary>
    [ProtocolID(0xF001)]
    public partial class ErrorNoti : Protocol
    {
        protected override int ValidByteSize => 1;

        public override ushort Id => 0xF001;

        private EnumErrorCode _code;
        /// <summary>
        /// 错误编码
        /// </summary>
        public EnumErrorCode code { get { return _code; } set { _code = value; } }
        private string _text;
        /// <summary>
        /// 提示文本
        /// </summary>
        public string text { get { return _text; } set { _text = value; } }
        public ErrorNoti()
        {
        }

        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            _code = default(EnumErrorCode);
            _text = default(string);
        }

        public override Protocol Allocate()
        {
            return ObjectPool<ErrorNoti>.Instance.Allocate();
        }

        public static ErrorNoti Alloc()
        {
            return ObjectPool<ErrorNoti>.Instance.Allocate();
        }

        protected override void CalculateValids()
        {
            base.CalculateValids();

            this.SetParamValid(0, this._code != default(EnumErrorCode));
            this.SetParamValid(1, this._text != default(string));
        }

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.EncodeEnum(this._code);
            if (this.CheckIsParamValid(1))
                this.Encode(this._text);
        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.DecodeEnum(ref this._code);
            if (this.CheckIsParamValid(1))
                this.Decode(ref this._text);
        }
    }

            /// <summary>
    /// 正常分手断链
    /// </summary>
    [ProtocolID(0xFF01)]
    public partial class BreakUpNoti : Protocol
    {
        protected override int ValidByteSize => 0;

        public override ushort Id => 0xFF01;

        public BreakUpNoti()
        {
        }

        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

        }

        public override Protocol Allocate()
        {
            return ObjectPool<BreakUpNoti>.Instance.Allocate();
        }

        public static BreakUpNoti Alloc()
        {
            return ObjectPool<BreakUpNoti>.Instance.Allocate();
        }

        protected override void CalculateValids()
        {
            base.CalculateValids();

        }

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

        }
    }

            /// <summary>
    /// 链接异常断开
    /// </summary>
    [ProtocolID(0xFF02)]
    public partial class InterruptionNoti : Protocol
    {
        protected override int ValidByteSize => 0;

        public override ushort Id => 0xFF02;

        public InterruptionNoti()
        {
        }

        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

        }

        public override Protocol Allocate()
        {
            return ObjectPool<InterruptionNoti>.Instance.Allocate();
        }

        public static InterruptionNoti Alloc()
        {
            return ObjectPool<InterruptionNoti>.Instance.Allocate();
        }

        protected override void CalculateValids()
        {
            base.CalculateValids();

        }

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

        }
    }

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
            return ObjectPool<ConnectResultNoti>.Instance.Allocate();
        }

        public static ConnectResultNoti Alloc()
        {
            return ObjectPool<ConnectResultNoti>.Instance.Allocate();
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