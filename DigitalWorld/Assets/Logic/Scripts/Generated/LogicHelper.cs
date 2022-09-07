/**
 * 该文件通过代码生成器生成
 * 请不要修改这些代码
 * 当然，修改了也没什么用，如果你有兴趣你可以试试。
 */
namespace DigitalWorld.Logic
{
    public static partial class LogicHelper
    {
        /// <summary>
        /// 通过节点类型和ID来获取对应的节点对象
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NodeBase GetNode(ENodeType nodeType, int id)
        {
            switch (nodeType)
            {
                case ENodeType.Action:
                {
                    return GetAction(id);
                }
                case ENodeType.Property:
                {
                    return GetProperty(id);
                }
                case ENodeType.Behaviour:
                {
                    return GetNode<Behaviour>();
                }
                default:
                    return null;
            }
        }

        public static PropertyBase GetProperty(int id)
        {
            return id switch
            {
 
                _ => null,
            };
        }

        public static T GetProperty<T>(int id) where T : PropertyBase
        {
            return GetProperty(id) as T;
        }

        /// <summary>
        /// 通过枚举来获取对应的注释
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetActionDesc(int id)
        {
            return id switch
            {
                0 => "空行动",
                1 => "杀掉角色",
                2 => "游戏中的创建对象，位置是基于世界坐标的",
                3 => "基于单位的创建角色，位置使用相对关系",
                4 => "单位造成伤害",
  
                _ => null,
            };
        }

        public static Actions.ActionBase GetAction(int id)
        {
            return id switch
            {
                0 => GetNode<Actions.None>(),
                1 => GetNode<Actions.Game.Unit.KillCharacter>(),
                2 => GetNode<Actions.Game.CreateCharacter>(),
                3 => GetNode<Actions.Game.Unit.CreateCharacter>(),
                4 => GetNode<Actions.Game.Unit.Damage>(),
                
                _ => null,
            };
        }
    }
}
