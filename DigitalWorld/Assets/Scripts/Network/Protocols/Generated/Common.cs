using Dream.Core;
using Dream.Proto;
using System;
using System.Text;

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
    public partial class ReqLogin : Protocol
    {
        protected override int ValidByteSize => 1;

        public const ushort protocolId = 0x0010;

        public override ushort Id => protocolId;

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
        public ReqLogin()
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
            return ObjectPool<ReqLogin>.Instance.Allocate();
        }

        public static ReqLogin Alloc()
        {
            return ObjectPool<ReqLogin>.Instance.Allocate();
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

        private void AssertOffsetAndLength(int offset, int length)
        {
            if (offset < 0 ||
                offset >= _buffer.Length ||
                offset + length > _buffer.Length)
                throw new ArgumentOutOfRangeException();
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
    public partial class AckLogin : Protocol
    {
        protected override int ValidByteSize => 1;

        public const ushort protocolId = 0x0011;

        public override ushort Id => protocolId;

        private NotiError _result;
        /// <summary>
        /// 结果
        /// </summary>
        public NotiError result { get { return _result; } set { _result = value; } }
        public AckLogin()
        {
        }

        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            _result = default(NotiError);
        }

        public override Protocol Allocate()
        {
            return ObjectPool<AckLogin>.Instance.Allocate();
        }

        public static AckLogin Alloc()
        {
            return ObjectPool<AckLogin>.Instance.Allocate();
        }

        protected override void CalculateValids()
        {
            base.CalculateValids();

            this.SetParamValid(0, this._result != default(NotiError));
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
    public partial class NotiError : Protocol
    {
        protected override int ValidByteSize => 1;

        public const ushort protocolId = 0xF001;

        public override ushort Id => protocolId;

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
        public NotiError()
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
            return ObjectPool<NotiError>.Instance.Allocate();
        }

        public static NotiError Alloc()
        {
            return ObjectPool<NotiError>.Instance.Allocate();
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
    public partial class NotiBreakUp : Protocol
    {
        protected override int ValidByteSize => 0;

        public const ushort protocolId = 0xFF01;

        public override ushort Id => protocolId;

        public NotiBreakUp()
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
            return ObjectPool<NotiBreakUp>.Instance.Allocate();
        }

        public static NotiBreakUp Alloc()
        {
            return ObjectPool<NotiBreakUp>.Instance.Allocate();
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
    public partial class NotiInterruption : Protocol
    {
        protected override int ValidByteSize => 0;

        public const ushort protocolId = 0xFF02;

        public override ushort Id => protocolId;

        public NotiInterruption()
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
            return ObjectPool<NotiInterruption>.Instance.Allocate();
        }

        public static NotiInterruption Alloc()
        {
            return ObjectPool<NotiInterruption>.Instance.Allocate();
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
    public partial class NotiConnectResult : Protocol
    {
        protected override int ValidByteSize => 1;

        public const ushort protocolId = 0xFF03;

        public override ushort Id => protocolId;

        private string _token;
        /// <summary>
        /// 口令
        /// </summary>
        public string token { get { return _token; } set { _token = value; } }
        private EnumConnectResult _result;
        /// <summary>
        /// 结果
        /// </summary>
        public EnumConnectResult result { get { return _result; } set { _result = value; } }
        public NotiConnectResult()
        {
        }

        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            _token = default(string);
            _result = default(EnumConnectResult);
        }

        public override Protocol Allocate()
        {
            return ObjectPool<NotiConnectResult>.Instance.Allocate();
        }

        public static NotiConnectResult Alloc()
        {
            return ObjectPool<NotiConnectResult>.Instance.Allocate();
        }

        protected override void CalculateValids()
        {
            base.CalculateValids();

            this.SetParamValid(0, this._token != default(string));
            this.SetParamValid(1, this._result != default(EnumConnectResult));
        }

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.Encode(this._token);
            if (this.CheckIsParamValid(1))
                this.EncodeEnum(this._result);
        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.Decode(ref this._token);
            if (this.CheckIsParamValid(1))
                this.DecodeEnum(ref this._result);
        }
    }

}