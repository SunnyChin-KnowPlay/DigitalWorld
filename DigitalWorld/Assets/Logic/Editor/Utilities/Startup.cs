using UnityEditor;

namespace DigitalWorld.Logic.Editor
{
    [InitializeOnLoad]
    internal class Startup
    {
        /// <summary>
        /// 启动时，call一下单例，做准备工作
        /// </summary>
        static Startup()
        {
            _ = NodeController.instance;
            _ = BehaviourController.instance;
        }
    }

}
