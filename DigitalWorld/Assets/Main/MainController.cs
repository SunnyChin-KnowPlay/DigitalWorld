using DigitalWorld.Net;
using DigitalWorld.UI;
using DigitalWorld.UI.Logic;
using Dream.Core;

namespace DigitalWorld
{
    public class MainController : DreamEngine.Singleton<MainController>
    {
        protected override void Awake()
        {
            base.Awake();

            _ = AgentManager.Instance;
            UIManager uiManager = UIManager.Instance;

            uiManager.ShowPanelAsync<LoginControl>(LoginControl.path);

            //uiManager.ShowPanel<LoginControl>(LoginControl.path);

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
