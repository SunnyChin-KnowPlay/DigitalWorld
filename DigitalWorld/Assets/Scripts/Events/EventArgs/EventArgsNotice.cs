using System;

namespace DigitalWorld.Events
{
    public class EventArgsNotice : EventArgs
    {
        public string message;

        /// <summary>
        /// 显示时间 小于等于0则为永久显示 直到手动关闭
        /// </summary>
        public float duration;

        public EventArgsNotice(string message, float duration)
        {
            this.message = message;
            this.duration = duration;
        }
    }
}
