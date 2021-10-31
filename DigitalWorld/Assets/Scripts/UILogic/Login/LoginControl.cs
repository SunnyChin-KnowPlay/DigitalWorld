using DigitalWorld.Net;
using DigitalWorld.Proto.Common;
using Dream.Proto;
using System.Net;
using UnityEngine.UI;

namespace DigitalWorld.UI.Logic
{
    public class LoginControl : PanelControl
    {
        public const string path = "UI/Login/LoginPanel";

        #region Param
        private InputField accountInputField;
        private InputField passwordInputField;
        private Button loginButton;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            AgentManager.Instance.AddProtocolListener(NotiConnectResult.protocolId, ProcessConnect);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            AgentManager.Instance.RemoveProtocolListener(NotiConnectResult.protocolId, ProcessConnect);
        }
        #endregion

        #region Bind
        protected override void BindWidgets()
        {
            base.BindWidgets();

            accountInputField = this.GetComponent<InputField>("Root/AccountInputField");
            passwordInputField = this.GetComponent<InputField>("Root/PasswordInputField");
            loginButton = this.GetComponent<Button>("Root/LoginButton");

            if (null != loginButton)
            {
                loginButton.onClick.AddListener(OnClickLogin);
            }
        }
        #endregion


        #region UI Event
        private void OnClickLogin()
        {
            EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 55000);
            AgentManager.Instance.Connect(AgentManager.TokenLogin, ep);
        }
        #endregion

        #region Protocol
        private void ProcessConnect(Protocol protocol)
        {
            NotiConnectResult result = protocol as NotiConnectResult;
            if (null != result)
            {
                if (result.result == EnumConnectResult.Success)
                {
                    Agent ag = AgentManager.Instance.LoginAgent;
                    if (null != ag)
                    {
                        ag.AddProtocolListener(AckLogin.protocolId, ProcessAckLogin);
                    }

                    ReqLogin req = ReqLogin.Alloc();
                    req.account = this.accountInputField.text;
                    req.password = this.passwordInputField.text;

                    AgentManager.Instance.LoginAgent.Send(req);
                }
                else
                {

                }
            }
        }

        private void ProcessAckLogin(Protocol protocol)
        {
            AckLogin ack = protocol as AckLogin;
            if (null != ack)
            {
                int x = 1;
            }
        }
        #endregion
    }
}
