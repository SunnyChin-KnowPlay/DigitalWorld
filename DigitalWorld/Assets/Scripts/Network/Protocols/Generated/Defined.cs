using Dream.Core;

namespace Dream.Network
{
	/// <summary>
    /// 协议ID
    /// </summary>
    public enum EnumProtocolID : ushort
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

}