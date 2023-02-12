using System;

namespace DigitalWorld.Logic.Editor
{
    /// <summary>
    /// 设置的条目
    /// </summary>
    internal class SettingItem
    {
        /// <summary>
        /// 条目的类型
        /// </summary>
        public Type itemType;

        /// <summary>
        /// PlayerPrefs的key
        /// </summary>
        public string playerPrefKey;

        /// <summary>
        /// 条目名
        /// </summary>
        public string name;

        internal SettingItem(Type itemType, string playerPrefKey, string name)
        {
            this.itemType = itemType;
            this.playerPrefKey = playerPrefKey;
            this.name = name;
        }
    }
}
