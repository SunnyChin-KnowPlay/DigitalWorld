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
                case ENodeType.Trigger:
                {
                    return GetNode<Trigger>();
                }
                case ENodeType.Property:
                {
                    return GetProperty(id);
                }
                default:
                    return null;
            }
        }

        /// <summary>
        /// 通过枚举来获取对应的注释
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetActionDesc(int id)
        {
            switch (id)
            {
                case 0:
                    return "出生怪物";
                case 2:
                    return "检测2个int32的值";
                case 1:
                    return "设置状态";
                case 3:
                    return "注册任务";
                case 4:
                    return "注销任务";

                default:
                    return null;
            }
        }

        /// <summary>
        /// 通过事件ID获取对应的注释
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <returns></returns>
        public static string GetEventDesc(int id)
        {
            switch (id)
            {
                case 5:
                    return "角色出生";
                case 6:
                    return "角色死亡";
                case 4:
                    return "游戏结束";
                case 2:
                    return "游戏初始化";
                case 3:
                    return "游戏启动";
                case 0:
                    return "触发";
                case 1:
                    return "迭代";

                default:
                    return null;
            }
        }

        /// <summary>
        /// 通过ID来获取对应的注释
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetPropertyDesc(int id)
        {
            switch (id)
            {
                case 11:
                    return "任务进度";
                case 8:
                    return "节点的运行次数";
                case 10:
                    return "怪物数量上限";
                case 9:
                    return "怪物数量";
                case 3:
                    return "布尔型变量";
                case 7:
                    return "阵营变量";
                case 1:
                    return "浮点型小数变量";
                case 0:
                    return "Int32型变量";
                case 4:
                    return "1维向量变量";
                case 5:
                    return "2维向量变量";
                case 6:
                    return "3维数组变量";
                case 2:
                    return "字符串型变量";

                default:
                    return null;
            }
        }

        public static Actions.ActionBase GetAction(int id)
        {
            return null;
        }

        public static Properties.PropertyBase GetProperty(int id)
        {
            return null;
        }
    }
}
