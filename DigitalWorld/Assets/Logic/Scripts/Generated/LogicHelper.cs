namespace DigitalWorld.Logic
{
    public sealed partial class LogicHelper
    {
        /// <summary>
        /// 通过节点类型和ID来获取对应的节点对象
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public NodeBase GetNode(ENodeType nodeType, int id)
        {
            switch (nodeType)
            {
                case ENodeType.Condition:
                {
                    return GetCondition(id);
                }
                case ENodeType.Action:
                {
                    return GetAction(id);
                }
                case ENodeType.Behaviour:
                {
                    return GetNode<Behaviour>();
                }
                default:
                    return null;
            }
        }

        public ActionBase GetAction(int id)
        {
            return id switch
            {
                1 => GetNode<ActionCreateCharacter>(),
                2 => GetNode<ActionKillCharacter>(),
                
                _ => null,
            };
        }

        public ConditionBase GetCondition(int id)
        {
            return id switch
            {
                1 => GetNode<ConditionRunningTime>(),
    
                _ => null,
            };
        }

		
    }
}
