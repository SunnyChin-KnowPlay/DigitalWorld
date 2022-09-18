using System;

namespace DigitalWorld.Events
{
    /// <summary>
    /// 事件特性
    /// </summary>
    public class EventAttribute : Attribute
    {
        public EEventType Category
        {
            get;
            private set;
        }

        public EventAttribute(EEventType category)
        {
            Category = category;
        }
    }
}
