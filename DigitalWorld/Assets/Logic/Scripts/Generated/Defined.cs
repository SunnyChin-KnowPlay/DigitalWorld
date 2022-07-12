namespace DigitalWorld.Logic
{
	public enum EEvent : int
	{
		Update = 0,
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
		/// <summary>
        /// 召唤金币怪
        /// </summary>
		SummonGoldMonster = 5,
		/// <summary>
        /// 召唤能量怪
        /// </summary>
		SummonEnergyMonster = 6,
		/// <summary>
        /// 召唤石头怪
        /// </summary>
		SummonStoneMonster = 7,
		/// <summary>
        /// 召唤龙
        /// </summary>
		SummonDragonMonster = 8,
	}

	public enum ECondition : int
	{
	}

	public enum EAction : int
	{
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
				case EEvent.SummonGoldMonster:
					return "召唤金币怪";
				case EEvent.SummonEnergyMonster:
					return "召唤能量怪";
				case EEvent.SummonStoneMonster:
					return "召唤石头怪";
				case EEvent.SummonDragonMonster:
					return "召唤龙";
				default:
					return null;
			}
        }

		public static string GetActionDesc(EAction e)
        {
			switch (e)
            {
				default:
					return null;
			}
        }

		public static string GetConditionDesc(ECondition e)
        {
			switch (e)
            {
				default:
					return null;
			}
        }
    }
}
