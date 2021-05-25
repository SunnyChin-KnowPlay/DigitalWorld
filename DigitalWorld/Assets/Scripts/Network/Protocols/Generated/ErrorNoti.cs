using Dream.Core;

namespace Dream.Network
{
    /// <summary>
    /// 错误通知
    /// </summary>
    [ProtocolID(0xF001)]
    public partial class ErrorNoti : Protocol
    {
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
            this._id = 0xF001;
        }

        public override void OnAllocate()
        {
            base.OnAllocate();

            _code = default(EnumErrorCode);
            _text = default(string);
        }

        public override void OnRecycle()
        {

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
    }
}
