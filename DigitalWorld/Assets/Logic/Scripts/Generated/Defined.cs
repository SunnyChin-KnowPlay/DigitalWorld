namespace DigitalWorld.Logic
{
	public enum EEvent : int
	{
		/// <summary>
        /// 游戏初始化
        /// </summary>
		GameInit = 1,
		/// <summary>
        /// 游戏启动
        /// </summary>
		GameStart = 2,
		/// <summary>
        /// 游戏结束
        /// </summary>
		GameEnd = 3,
		/// <summary>
        /// 角色激活
        /// </summary>
		CharacterActivate = 4,
	}

	public enum EAction : int
	{
		/// <summary>
        /// 创建角色
        /// </summary>
		CreateCharacter = 1,
		/// <summary>
        /// 杀死角色
        /// </summary>
		KillCharacter = 2,
	}

	public static class Defined
	{
        public static string GetEventDesc(EEvent e)
        {
			switch (e)
            {
				case EEvent.GameInit:
					return "游戏初始化";
				case EEvent.GameStart:
					return "游戏启动";
				case EEvent.GameEnd:
					return "游戏结束";
				case EEvent.CharacterActivate:
					return "角色激活";
				default:
					return null;
			}
        }

		public static string GetActionDesc(EAction e)
        {
			switch (e)
            {
				case EAction.CreateCharacter:
					return "创建角色";
				case EAction.KillCharacter:
					return "杀死角色";
				default:
					return null;
			}
        }
    }
}
