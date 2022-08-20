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
                1 => GetNode<PropertyTestInt32>(),
 
                _ => null,
            };
        }

        public static T GetProperty<T>(int id) where T : PropertyBase
        {
            return GetProperty(id) as T;
        }

        public static ActionBase GetAction(int id)
        {
            return id switch
            {
                1 => GetNode<ActionCreateCharacter>(),
                2 => GetNode<ActionKillCharacter>(),
                
                _ => null,
            };
        }
    }
}
