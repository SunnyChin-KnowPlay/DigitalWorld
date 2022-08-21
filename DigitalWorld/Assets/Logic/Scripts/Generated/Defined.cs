namespace DigitalWorld.Logic
{
	public enum EEvent : int
	{
		/// <summary>
        /// 迭代事件
        /// </summary>
		Update = 0,
	}

	public enum EAction : int
	{
		/// <summary>
        /// 空行动
        /// </summary>
		None = 0,
		/// <summary>
        /// 创建角色
        /// </summary>
		CreateCharacter = 1,
	}

	public static class Defined
	{
        public static string GetEventDesc(EEvent e)
        {
			switch (e)
            {
				case EEvent.Update:
					return "迭代事件";
				default:
					return null;
			}
        }

		public static string GetActionDesc(EAction e)
        {
			switch (e)
            {
				case EAction.None:
					return "空行动";
				case EAction.CreateCharacter:
					return "创建角色";
				default:
					return null;
			}
        }
    }
}
