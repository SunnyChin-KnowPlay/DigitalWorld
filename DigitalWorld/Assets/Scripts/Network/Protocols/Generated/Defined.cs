using Dream.Core;

namespace Dream.Proto
{
    enum EnumProtocolID
    {
        /// <summary>
        /// 登录请求
        /// </summary>
        LoginReq = 0x0010,
        /// <summary>
        /// 登录响应
        /// </summary>
        LoginAck = 0x0011,
        /// <summary>
        /// 错误通知
        /// </summary>
        ErrorNoti = 0xF001,
        /// <summary>
        /// 正常分手断链
        /// </summary>
        BreakUpNoti = 0xFF01,
        /// <summary>
        /// 链接异常断开
        /// </summary>
        InterruptionNoti = 0xFF02,
        /// <summary>
        /// 链接远端结果通知
        /// </summary>
        ConnectResultNoti = 0xFF03,
    }

}