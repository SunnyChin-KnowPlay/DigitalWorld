using DigitalWorld.Net;
using Dream.Core;

namespace DigitalWorld
{
    public class MainController : DreamEngine.Singleton<MainController>
    {
        private AgentManager _agentManager;

        protected override void Awake()
        {
            base.Awake();

            this._agentManager = AgentManager.instance;
        }

        private void OnApplicationQuit()
        {
            if (null != _agentManager)
            {

            }
        }
    }
}
