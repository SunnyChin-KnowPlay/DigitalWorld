using Dream.Core;

namespace Dream.Network
{
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
            return ObjectPool<ErrorNoti>.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<ErrorNoti>.Recycle(this);
        }

        public static ErrorNoti Alloc()
        {
            return ObjectPool<ErrorNoti>.Allocate();
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
}
