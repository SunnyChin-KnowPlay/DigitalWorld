using DigitalWorld.Net;
using DigitalWorld.UI;
using Dream.Core;

namespace DigitalWorld
{
    public class MainController : DreamEngine.Singleton<MainController>
    {
        protected override void Awake()
        {
            base.Awake();

            _ = AgentManager.Instance;
            _ = UIManager.Instance;
        }

        private void OnApplicationQuit()
        {
            //if (null != _agentManager)
            //{
            //    AgentManager.DestroyInstance();
            //}
        }
    }
}
