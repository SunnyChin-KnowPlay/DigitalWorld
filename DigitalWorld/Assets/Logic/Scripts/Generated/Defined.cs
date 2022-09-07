/**
 * 该文件通过代码生成器生成
 * 请不要修改这些代码
 * 当然，修改了也没什么用，如果你有兴趣你可以试试。
 */
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
        /// 杀掉角色
        /// </summary>
		Game_Unit_KillCharacter = 1,
		/// <summary>
        /// 游戏中的创建对象，位置是基于世界坐标的
        /// </summary>
		Game_CreateCharacter = 2,
		/// <summary>
        /// 基于单位的创建角色，位置使用相对关系
        /// </summary>
		Game_Unit_CreateCharacter = 3,
		/// <summary>
        /// 单位造成伤害
        /// </summary>
		Game_Unit_Damage = 4,
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
				case EAction.Game_Unit_KillCharacter:
					return "杀掉角色";
				case EAction.Game_CreateCharacter:
					return "游戏中的创建对象，位置是基于世界坐标的";
				case EAction.Game_Unit_CreateCharacter:
					return "基于单位的创建角色，位置使用相对关系";
				case EAction.Game_Unit_Damage:
					return "单位造成伤害";
				default:
					return null;
			}
        }
    }
}
