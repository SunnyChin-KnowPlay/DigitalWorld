using Dream.Core;

namespace Dream.Proto
{
    enum EnumProtocolID
    {
        /// <summary>
        /// 地图数据
        /// </summary>
        MapData = 0x0101,
        /// <summary>
        /// 登录请求
        /// </summary>
        ReqLogin = 0x0010,
        /// <summary>
        /// 登录响应
        /// </summary>
        AckLogin = 0x0011,
        /// <summary>
        /// 错误通知
        /// </summary>
        NotiError = 0xF001,
        /// <summary>
        /// 正常分手断链
        /// </summary>
        NotiBreakUp = 0xFF01,
        /// <summary>
        /// 链接异常断开
        /// </summary>
        NotiInterruption = 0xFF02,
        /// <summary>
        /// 链接远端结果通知
        /// </summary>
        NotiConnectResult = 0xFF03,
    }

}